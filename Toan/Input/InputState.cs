using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

using Toan.ECS.Resources;

namespace Toan.Input;

public class InputState : Resource
{
    private HashSet<Keys> _keysPressed = new();
    private HashSet<Keys> _keysPressedLastFrame = new();

    private HashSet<Keys> _keysJustPressed = new();
    private HashSet<Keys> _keysJustReleased = new();

    public KeyboardState Keyboard { get; private set; }

    public bool KeyDown(Keys key) => _keysPressed.Contains(key);
    public bool KeyUp(Keys key) => !KeyDown(key);
    public bool KeyPressed(Keys key) => _keysJustPressed.Contains(key);
    public bool KeyReleased(Keys key) => _keysJustReleased.Contains(key);

    public void Update(KeyboardState keyboardState)
    {
        Keyboard = keyboardState;

        _keysPressedLastFrame = _keysPressed;
        _keysPressed = new(Keyboard.GetPressedKeys());

        _keysJustPressed = new(_keysPressed);
        _keysJustPressed.ExceptWith(_keysPressedLastFrame);
        _keysJustReleased = new(_keysPressedLastFrame);
        _keysJustReleased.ExceptWith(_keysPressed);
    }
}

