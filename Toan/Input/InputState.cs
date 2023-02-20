using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Toan.ECS.Resources;

namespace Toan.Input;

public class InputState : Resource
{
    public void Update(KeyboardState keyboardState, MouseState mouseState)
    {
        Keyboard = keyboardState;

        _keysPressedLastFrame = _keysPressed;
        _keysPressed = new(Keyboard.GetPressedKeys());

        _keysJustPressed = new(_keysPressed);
        _keysJustPressed.ExceptWith(_keysPressedLastFrame);
        _keysJustReleased = new(_keysPressedLastFrame);
        _keysJustReleased.ExceptWith(_keysPressed);

        Mouse = mouseState;

        _lastMousePosition = MousePosition;
        _lastScrollWheel = ScrollWheel;
        _lastHorizontalScrollWheel = HorizontalScrollWheel;

        MousePosition = Mouse.Position;
        ScrollWheel = Mouse.ScrollWheelValue;
        HorizontalScrollWheel = Mouse.HorizontalScrollWheelValue;

        _lastMouseStates = _mouseStates;
        _mouseStates = CreateButtomStateMap();

        _mouseStates[MouseButton.Left] = Mouse.LeftButton;
        _mouseStates[MouseButton.Right] = Mouse.RightButton;
        _mouseStates[MouseButton.Middle] = Mouse.MiddleButton;
        _mouseStates[MouseButton.Mouse4] = Mouse.XButton1;
        _mouseStates[MouseButton.Mouse5] = Mouse.XButton2;
    }

    public KeyboardState Keyboard { get; private set; }
    #region Keyboard
    private HashSet<Keys> _keysPressed = new();
    private HashSet<Keys> _keysPressedLastFrame = new();

    private HashSet<Keys> _keysJustPressed = new();
    private HashSet<Keys> _keysJustReleased = new();

    public bool KeyDown(Keys key) => _keysPressed.Contains(key);
    public bool KeyUp(Keys key) => !KeyDown(key);
    public bool KeyPressed(Keys key) => _keysJustPressed.Contains(key);
    public bool KeyReleased(Keys key) => _keysJustReleased.Contains(key);
    #endregion

    public MouseState Mouse { get; private set; }
    #region Mouse
    public Point MousePosition { get; private set; }
    public Point MouseDelta
        => MousePosition - _lastMousePosition;

    private Point _lastMousePosition;

    public int ScrollWheel { get; private set; }
    public int ScrollWheelDelta
        => ScrollWheel - _lastScrollWheel;
    public int HorizontalScrollWheel { get; private set; }
    public int HorizontalScrollWheelDelta
        => HorizontalScrollWheel - _lastHorizontalScrollWheel;

    private int _lastScrollWheel;
    private int _lastHorizontalScrollWheel;

    private Dictionary<MouseButton, ButtonState> _mouseStates = CreateButtomStateMap();
    private Dictionary<MouseButton, ButtonState> _lastMouseStates = new();

    public bool MouseDown(MouseButton button) => _mouseStates[button] == ButtonState.Pressed;
    public bool MouseUp(MouseButton button) => !MouseDown(button);
    public bool MousePressed(MouseButton button) => MouseDown(button) && _lastMouseStates[button] == ButtonState.Released;
    public bool MouseReleased(MouseButton button) => MouseUp(button) && _lastMouseStates[button] == ButtonState.Pressed;

    private static Dictionary<MouseButton, ButtonState> CreateButtomStateMap()
    => new()
    {
        [MouseButton.Left] = ButtonState.Released,
        [MouseButton.Right] = ButtonState.Released,
        [MouseButton.Middle] = ButtonState.Released,
        [MouseButton.Mouse4] = ButtonState.Released,
        [MouseButton.Mouse5] = ButtonState.Released,
    };
    #endregion

    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        Mouse4,
        Mouse5,
    }
}

