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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ArmokLanguage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyTraceListener TraceListener { get; set; }

        public MainWindow()
        {
            TraceListener = new MyTraceListener();
            InitializeComponent();
            Trace.Listeners.Add(TraceListener);
            uristinfo.SetBinding(TextBox.TextProperty, new Binding("Trace") { Source = TraceListener });
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Language p = new Language();
            try
            {
                p.ShortParse(program.Text);
                Trace.WriteLine(p.Execute(input.Text).Output);
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Program terminated");
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
