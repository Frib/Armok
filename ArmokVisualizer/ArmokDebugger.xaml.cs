using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ArmokLanguage;
using System.Diagnostics;
using System.Windows.Threading;

namespace ArmokVisualizer
{
    /// <summary>
    /// Interaction logic for ArmokDebugger.xaml
    /// </summary>
    public partial class ArmokDebugger : Window
    {
        private TraceListener TraceListener { get; set; }

        public ArmokDebugger()
        {
            TraceListener = new MyTraceListener();
            InitializeComponent();
            Trace.Listeners.Add(TraceListener);
            debugText.SetBinding(TextBox.TextProperty, new Binding("Trace") { Source = TraceListener });
            
            eventSelection.Items.Add("Is dead");
            eventSelection.Items.Add("Stops mining/dumping");
            eventSelection.Items.Add("Takes 100 turns");
            eventSelection.Items.Add("Starts work task");
            eventSelection.Items.Add("Does something else");
            eventSelection.Items.Add("Trades something");

            eventSelection.SelectedIndex = 0;

            dt = new DispatcherTimer();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = TimeSpan.FromSeconds(0.1d);
        }

        void dt_Tick(object sender, EventArgs e)
        {
            stepButton_Click(sender, null);
        }
        
        public World World { get; set; }
        public int Turn { get; set; }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            World.input = this.inputText.Text.ToArray();
            int count = 0;
            while (World.DebugStep() && count < 10000)
            {
                Turn++;
                count++;
            }
            UpdateView();
        }

        public void UpdateView()
        {
            this.turnLabel.Content = "T " + Turn;
            this.inputText.Text = new string(World.input);
            this.outputText.Text = World.Output;
            
            writeCaveText();

            int index = dwarfList.SelectedIndex;
            dwarfList.Items.Clear();
            foreach (var d in World.Dwarves)
            {
                dwarfList.Items.Add(d);
            }
            dwarfList.SelectedIndex = index;

            index = managerList.SelectedIndex;
            managerList.Items.Clear();
            foreach (var r in World.Routines)
            {
                managerList.Items.Add(r.Key);
            }
            managerList.SelectedIndex = index;

            outputText.Text = World.Output;


            index = dwarfSelection.SelectedIndex >= 0 ? dwarfSelection.SelectedIndex : 0;
            dwarfSelection.Items.Clear();
            dwarfSelection.Items.Add("Any dwarf");
            foreach (var d in World.Dwarves)
            {
                dwarfSelection.Items.Add(d.Name);
            }
            dwarfSelection.SelectedIndex = index;
        }
                
        private void stepButton_Click(object sender, RoutedEventArgs e)
        {
            World.input = this.inputText.Text.ToArray();
            World.DebugStep();
            if (World.Dwarves.All(x => x.Dead))
            {
                dt.Stop();
            }
            Turn++;
            UpdateView();
        }

        private void unleashTheDaleks_Click(object sender, RoutedEventArgs e)
        {
            foreach (var d in World.Dwarves.Where(x => !x.Dead))
            {
                d.Dead = true;
                Trace.WriteLine(d.Name + " was possessed and killed by unknown forces!");
            }

            UpdateView();
        }

        private void dwarfList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dwarfList.SelectedItem != null)
            {
               dwarfText.Text = (dwarfList.SelectedItem as Dwarf).Instructions.Aggregate<Instruction, string>("", (x, y) => x.ToString() + y.ToString());
            }
        }

        private void writeCaveText()
        {
            StringBuilder cave = new StringBuilder();
            StringBuilder rocks = new StringBuilder();
            StringBuilder workshops = new StringBuilder();

            cave.Append("i:");
            rocks.Append("r:");
            workshops.Append("w:");

            for (int i = 0; i < World.Cave.Count; i++)
            {
                int caveLength = i.ToString().Length;                
                int longest = 2;
                if (caveLength > longest)
                {
                    longest = caveLength;
                }
                int rockCount = World.Cave[i].Rocks;
                int rockLength = rockCount >= 0 ? rockCount.ToString().Length : 1;
                int workshopLength = 0;
                if (rockLength > longest)
                {
                    longest = rockLength;
                }

                var anyDorfs = World.Dwarves.Any(d => !d.Dead && d.Position == i);
                var anyWorkshop = World.Cave[i].Workshop != null;
                if (anyWorkshop && anyDorfs)
                {
                    if (longest < 2)
                    {
                        longest = 2;
                    }
                    workshopLength = 2;
                }
                else if (anyWorkshop || anyDorfs)
                {
                    workshopLength = 1;
                }

                cave.Append("[");
                rocks.Append("[");
                workshops.Append("[");

                for (int x = 0; x < longest - caveLength; x++)
                {
                    cave.Append(" ");
                }
                for (int x = 0; x < longest - rockLength; x++)
                {
                    rocks.Append(" ");
                }
                for (int x = 0; x < longest - workshopLength; x++)
                {
                    workshops.Append(" ");
                }
                cave.Append(i.ToString());
                rocks.Append(rockCount >= 0 ? rockCount.ToString() : "#");

                if (anyDorfs)
                {
                    workshops.Append("d");
                }
                if (anyWorkshop)
                {
                    workshops.Append("w");
                }

                cave.Append("]");
                rocks.Append("]");
                workshops.Append("]");
            }

            caveBox.Text = cave.ToString() + Environment.NewLine + rocks.ToString() + Environment.NewLine + workshops.ToString();
        }

        private void managerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (managerList.SelectedIndex >= 0)
            {
                managerText.Text = World.Routines[managerList.SelectedItem.ToString()].Aggregate<Instruction, string>("", (x, y) => x.ToString() + y.ToString());
            }
        }

        private void runUntilButton_Click(object sender, RoutedEventArgs e)
        {
            bool conditionMet = false;
            World.input = this.inputText.Text.ToArray();

            if (dwarfSelection.SelectedIndex < 0) return;

            var dorfs = dwarfSelection.SelectedIndex == 0 ? World.Dwarves.Where(x => !x.Dead).ToList() : new List<Dwarf>() { World.Dwarves.First(x => x.Name == dwarfSelection.SelectedItem.ToString()) };
            
            int turnStart = Turn;

            Dictionary<Dwarf, Type> lastInstructions = new Dictionary<Dwarf,Type>();

            foreach (var d in dorfs)
            {
                if (d.Instructions.Any())
                {
                    lastInstructions.Add(d, d.Instructions.First().GetType());
                }
                else
                {
                    lastInstructions.Add(d, null);
                }
            }

            int count = 0;
            while (!conditionMet)
            {      
                World.DebugStep();
                Turn++;
                count++;

                foreach (var d in dorfs)
                {
                    string eventToCheck = eventSelection.SelectedItem.ToString();

                    switch(eventToCheck)
                    {
                        case "Is dead": conditionMet |= d.Dead; break;
                        case "Stops mining/dumping": conditionMet |= !(d.Instructions.FirstOrDefault() is Mine || d.Instructions.FirstOrDefault() is Dump); break;
                        case "Takes 100 turns": conditionMet |= Turn - turnStart >= 100; break;
                        case "Starts work task": conditionMet |= d.Instructions.FirstOrDefault() is Work; break;
                        case "Does something else": 
                            {
                                var current = d.Instructions.FirstOrDefault();     
                                if (current != null && lastInstructions[d] != null)
                                {
                                    conditionMet |= (current.GetType() == lastInstructions[d]);
                                }
                                else if (lastInstructions[d] != null && current == null)
                                {
                                    conditionMet = true;
                                }
                                else if (lastInstructions[d] == null && current != null)
                                {
                                    conditionMet = true;
                                }
                            }
                            break;
                        case "Trades something": conditionMet |= d.Instructions.FirstOrDefault() != null && d.Instructions.First().GetType() == typeof(Work) && World.Cave[d.Position].Workshop != null && World.Cave[d.Position].Workshop.GetType() == typeof(Trader); break;
                            
                    }
                }
                conditionMet |= dorfs.All(d => d.Dead);
                conditionMet |= count >= 10000;
            }

            UpdateView();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (dt.IsEnabled)
            {
                dt.Stop();
            }
            else
            {
                dt.Start();
            }
        }

        private DispatcherTimer dt;

        private void dwarfOrderSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (dt.IsEnabled || dwarfList.SelectedItem == null)
            {
                return;
            }

            var di = ((Dwarf)dwarfList.SelectedItem).Instructions;

            di.Clear();

            foreach (char c in dwarfText.Text)
            {
                var i = ArmokLanguage.Language.ParseChar(c);
                if (i != null)
                {
                    di.Add(i);
                }
            }
        }

        private void routineSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (dt.IsEnabled || managerList.SelectedItem == null)
            {
                return;
            }

            var ml = World.Routines[managerList.SelectedItem.ToString()];

            ml.Clear();

            foreach (char c in managerText.Text)
            {
                var i = ArmokLanguage.Language.ParseChar(c);
                if (i != null)
                {
                    ml.Add(i);
                }
            }
        }

        private void dwarfAdd_Click(object sender, RoutedEventArgs e)
        {
            var instructions = new List<Instruction>();
            var name = (World.Routines.Count + 1).ToString();
            World.Dwarves.Add(new Dwarf(name, instructions));
            World.Routines.Add(name, new List<Instruction>());
            UpdateView();
        }

        private void managerAdd_Click(object sender, RoutedEventArgs e)
        {
            World.Routines.Add((World.Routines.Count + 1).ToString(), new List<Instruction>());
            UpdateView();
        }
    }
}
