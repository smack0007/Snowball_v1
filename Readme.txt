Snowball
========
Snowball is a 2D game framework written in C# using SharpDX.

Philosophy
==========
Though this project is called a framework, the goal is to make the framework as
flexible as possible. Most components speak to other components through
interfaces and can therefore be swapped out as needed. For example, a custom
window class can be created for use with the Game class by either subclassing
the standard GameWindow class or implementing the interface IGameWindow.

