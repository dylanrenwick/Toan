# Toan
Toan is a relatively simple ECS engine implemented on top of MonoGame.

## Usage
* Create a MonoGame desktop project as you would usually
* Add a reference to Toan to the project
* Change the project's default game class from extending `Microsoft.Xna.Framework.Game` to extending `Toan.ToanGame`
* Remove all contents of the game class
* Override `Build` and add any desired plugins there

Add the `CorePlugins` plugin to add all of Toan's core functionality such as physics and rendering.