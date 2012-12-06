Armok
=============

This is a programming language made by Armok himself. Well, not really. However, it is inspired by Dwarf Fortress.
With this language, you can program your dwarves to do tasks. There are 5 tasks in total.
It is up to you to manage your dwarves (yes! plural! Multitasking!) and create the ultimate fortress that does arithmetic and other fancy stuff.

'Hello World!' can be written as followed:

	->mmwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>d<d<
	->w<
	-dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd>w<
	+>>mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmddddd
		ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd>mm<w>mmmdddm
		mwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>d<
		<wwwwwwwwwwwwwwwwwwddddddddddddddddddddddddddddddddddddddddddddddddddddd
		ddddddd<mmm>wddw<<mmm>>>>ddd<ww<<<mmmmmmmm>>w>www<<<mmmmm>>w>www<<<mmmmm
		mmmmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmm>>w>w<w>ww<<<mmmmm
		mmmmmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmmmmm>
		>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmm>>w>w<<<m>>w<<<
	-<<<mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>>>
	
For more code examples and an explanation of what is happening here, see the lower half of this readme.

The world
-------

Armok simulates a cave, with any number of dwarves in it. The cave is like a tape, but there are some differences.
* It starts at position 0, and cannot go further left.
* Position 0 is magma. Anything going in there will never come back out.
* All your dwarves spawn on position 1
* Positions 2 and 3 are open as well.
* From position 4 onward, the cave is solid. You will have to mine first until you can continue.

Your dwarves are handling rock. These rocks are used for various tasks. Dwarves can carry any amount of rocks.
There are currently two ways to acquire rock: Mining and trading.
Your dwarves can construct workshops, which will use rocks in one way or another to do more complicated things.

Instructions
-------

There are 7 instructions in Armok. 5 of them control the dwarves:
* > Move right, moves the dwarf 1 tile to the right. If he hits a solid wall, he dies.
* < Move left, moves the dwarf 1 tile to the left. If he walks into magma, he dies.
* m Mine, if it's there, he turns a solid wall to the right of him into a pile of 64 rocks. He also takes a rock from the pile to his right, if there are any.
* d Dump, if he has a rock in his inventory, he will dump it to the tile on his left.
* w Work, will construct a workshop on that tile if there is none. Will work in a workshop if there is. Which workshop is constructed depends on how many rocks he's carrying.

The two remaining are for setting things up:
* + Creates a new dwarf. Every instruction coming after that, which is not + or -, will be his tasks.
* - Creates a new subroutine. Every instruction coming after that, which is not + or -, will be the tasks.

Each dwarf starts with an instruction set. Each subroutine is also an instruction set. 
The order in your source code defines the number assigned to the instruction set, starting with 1. This is useful for later.
But what that means is, that if you create a dwarf, it will have subroutine number 1. If you then create another subroutine, it will have number 2. Another dwarf? number 3. And so on.

Workshops
-------

Currently, there are three workshops implemented, but this number will increase as Armok is further developed. 
Workshops are built with the work command if a tile doesn't contain one. Workshops are then worked by using the work command while on one.

1. Trader. This is your input/output. If you dwarf is carrying rocks and he works at a trader, he will output them as a character. If your dwarf is not carrying rocks and he works, then he takes the next character from the input. If there is no further input, the traders will murder him.
2. Manager's office. This is your manager! Dwarves that issue a work instruction on a manager's office will start a subroutine. Which subroutine depends on the amount of rocks dumped on the office. So if there are 2 rocks on it, instruction set #2 will be assinged to the dwarf. Working at a manager with 0 rocks will destroy the office.
3. Appraiser. In this office, your dwarves will compare their rocks to the rocks on the ground here. If they are carrying more, they will dump one to the tile on the left. If they are carrying the same amount or less, nothing happens. 

More is yet to come, such as a way for dwarves to interact with each other for more parallel goodness. Speaking of which...

Parallellism
-------

Dwarves work in parallel. Sort of. Every turn, the dwarves (that aren't dead) do one task. They do this in order.
This means that you can create multiple dwarves, and have them do specific tasks repeatedly. Or time their turns just right so they always help each other. 
Make your fort more efficient! Instead of writing the shortest 'Hello World!', it is now a challenge to write the one that takes the least amount of turns.

Death
-------

Your dwarves can die! Currently there are 5 ways they can die.
* If they run out of work, they will be stricken by melancholy.
* If they fail to construct a workshop, they will go stark raving mad.
* If they run into a wall, they will die.
* If they run into magma, they will die.
* If they want input from the traders, but there isn't any, they will be murdered by the filthy backstabbing elves.

When all your dwarves are dead, the program terminates.

Example programs
=======

HELL
-------

When we look at the bare basics, this is how you write 'Hello world!':

	+>>mwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>mmmmmmmmm<w>mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>mmmmmmmmmmmmmm<<w>>mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>mmmmmmmmmmmmmmmmmmmmmmmmmm<<<w>>>mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm<<<<w<<<

Actually, I'm lying. That's not 'Hello World!', that's 'HELL'. And your dwarf will then promptly walk into magma and die.
Truth be told, I got bored after that second L, because rocks are a limited resource per tile. And I can't show the fancy stuff yet.

So what's happening here? Well, let's look at the first few characters of the code: +>>mw
Here we define a new dwarf. His first tasks will be to move right twice, then mine, and then work.
When he works, he notices there is no workshop yet. So he creates one using the rock he just mined. One rock creates a trader!

So after he has created the trader, he starts mining like crazy, occasionally moving right when he runs out of rock to mine, and then he goes back to the trader to work.
Since he has rocks, he outputs those as a character. He does this 4 times before going mad and running all the way left into the magma.

Copycat
-------

So let's cheat a little. Let's create Hello World!, but this time we use the trader to supply the letters.

	+>>mwwwwwwwwwwwwwwwwwwwwwwwww<<<

This time, we create a dwarf, move him to the right twice, and then we mine once.
We then create the trading depot. After that, we work for another 24 turns. So what does this do?
The first time we work the depot, we don't have any rocks. So the dwarf asks for input.
Once he has that, he will work again. Since he now has rocks, he will output them.

So if we supply 'Hello World!' as input, he will output that as well. Yay!
However, currently we are limited by length. If the input is shorter, it's no problem. But if it's longer, then he will wander off and die before he can output everything.
Which brings me to the next point

The Manager
-------

Let's write a program that takes input of any length, and outputs that again.

	+>>mwmm<w>mmdd<w->ww<w

It's even shorter than the one above! So what's happening here?
The dwarf first creates a trader, and then creates a manager's office to the left of that. 
Finally, he dumps two rocks on the manager's office, moves to it, and works.

But you'll notice that there is a subroutine in this line of code: ->ww<w
Since this is the second subroutine (the first being the instructions for the dwarf itself), we've placed two rocks on the manager's office.
When the dwarf gets to work at the office, he'll do the subroutine next.
So he then moves to the right, works twice (gets input from trader, then outputs it again), moves left, and works again.
Since his final step is to work on the manager, he'll be assigned routine #2 again. This repeats until the traders kill him when there is no more input.

Fun
-------

Let's kill 5 dwarves in 5 different ways!

	++<+w+>>>+>>mww

The first dwarf has no instructions. So he turns melancholic and dies.
The second dwarf moves left and walks into magma.
The third dwarf tries to create a workshop, but he has no rock, so he goes stark raving mad.
The fourth dwarf runs head-long into a wall and dies.
The fifth dwarf goes to the wall, mines a rock, builds a trader, then trades. But if there's no input, the traders murder him.

Infinite mining and hauling to a specific tile
------

Since each cave wall contains 64 rocks, we need to keep mining if we want more to work with.
But we also want a single source tile of rock. How can we achieve this? With managers!

	->mmwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>d<d<
	->w<
	-dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd>w<w
	+>>mm<w>mmmdddmmwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>d<<w

This code will create a dwarf that will mine indefinitely and dump it all on tile 1. So what's going on here?
You'll notice that the subroutines are defined before the dwarf. This is to save rocks. 
Even though we're going to mine an infinite amount of them, we don't want to waste any if we can help it.

What we need is the following:
* A drop point
* A (moving) mining point
* A path between those two.

That's exactly what those 3 subroutines are. 
The first is the mining point. It will mine and create a new manager with the mining subroutine for next time. It will then drop a rock on the mining manager before, turning it into a path manager, extending the chain
The second is the path in between. Move right, work at the next, then move left. Chained together, these will make a dwarf move on and on until he finds something else to do.
The third is the drop point. Dump all your rock, then move right (to either a mining point or a path towards one), work, move left, and work here again to keep things infinite.
In order to remove infinity from this, simply remove the last work order from the drop subroutine and add manual work tasks to the dwarf itself.

The dwarf itself will set things up to drop everything at position 1. When he's done, he'll have created two workshops. One for dropping, one for mining.

A big drawback from this is that the entire pathway will be filled with manager offices. 

True Hello World!
-------

With the knowledge we have right now, it's possible to create an actual 'Hello World!' program!
For it we'll need 1031 rocks for the output. We'll need a few more for various things. If we do the mining routine 20 times, we should have enough rocks to work with.

	->mmwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>d<d<
	->w<
	-dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd>w<
	+>>mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd>mm<w>mmmdddmmwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>d<<wwwwwwwwwwwwwwwwwwdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd<mmm>wddw<<mmm>>>>ddd<ww<<<mmmmmmmm>>w>www<<<mmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmm>>w>w<w>ww<<<mmmmmmmmmmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmm>>w>w<<<m>>w<<<
	-<<<mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>>>
	
Lots of things happening here right now. First of all, we're going to mine roughly 1200 rocks. Because we can. They will all be dumped on tile #2.
The first half of the dwarf's code is just that. Once that's done, we'll do the following:

	<mmm>wddw<<mmm>>>>ddd<ww<<<mmmmmmmm>>w>www<<<mmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmm>>w>w<w>ww<<<mmmmmmmmmmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmmmmmmmm>>w>www<<<mmmmmmmmmmmm>>w>www<<<mmmm>>w>w<<<m>>w<<<
	
This part will remove the manager's office and replace it with a trader. Then we'll add rocks to the manager next to it, so it points to routine #5.
Routine #5 basically means 'pick up 32 rocks from the rock stockpile we have'. We want 32, because capitals 'start' at the 64 range, and normal letters at the 96 range. The remainder of the dwarf's code is to gather additional rocks in order to form the letters, then trade.

Finally, we end up with 'Hello World!' :D
And it only takes 4600 turns. We can probably shave off a bunch of wasted time on movement by swapping the trader and the manager, mining a few rocks less, etc.
But I think the main turn advantage will be in parallellism. But that's for later!

Visualizer
=======

The source code now has its user interface separated from the language itself. Sort of. It is theoretically possible to just build the language and run that from console with your code as input. It will then ask you for your text input and run it with that.
However, if you are a filthy elf like me, you'd prefer the use of a fancy visualizer. Simply start the visualizer program, enter your source code, then hit the debug button. A fancy new window will appear!

In this new window, you can view the state of your world. In the box on the top, the cave is shown. The first line is the index, the second is the rock count, and the third line are if any dorfs or workshops are there.
TODO: give more info about the dorfs and workshops. And a way to view more detailed cell information.

Then you'll have dwarf information. The box on the left shows all your dwarfs and a quick overview of their state. Select one to view his remaining orders.

After that comes the routine list. Select one to view the details. The number is the one that managers will match their rock count to.
Finally, you can view the output of your program and some trace information.

Below that are some fancy input buttons.
First, the lower left label is the turn count.
The small textbox holds the input for traders. You can change this at any time when it is not actively running.
The step button makes the fort step once.
The run button makes it run until all dorfs are dead, or for 10.000 turns.
The run until button makes it run until a specific condition is met, or until they are all dead or 10k turns have passed.
The exterminate button exterminates all the dorfs. Handy if you're stuck in an infinite loop.

The future
-------

I've got some interesting ideas for this. This is the first time I'm creating my own language, just for fun, so I have no idea what I'll end up with.
However, I'd like to focus on making the dorfs work in parallel. Some ideas are: 

* Jailing a dorf (skipping his turns) until he is freed. Then you can create mathematical mini-routines that start whenever you want.
* A mayor's office, mandating whatever subroutine to every dwarf that walks on that tile.
* A lever. Linked to magma floodgates. Fun for the whole fortress. Basically kills every dwarf and terminates the program.
* Breeding. Create new dwarves starting with a specific subroutine.

Stuff like that. But the main priority is to add some way of doing maths with the rocks. Comparisons at least.
