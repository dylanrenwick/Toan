using System;

using Microsoft.Xna.Framework;

namespace Toan.ECS.Components;

public class Motor : GameComponent, ICloneable<Motor>
{
    public MotorMode Mode { get; set; } = MotorMode.Local;

    public Vector2 Input
    {
        get => _input;
        set
        {
            _input = value;
            if (_input.LengthSquared() > 0f)
                _input.Normalize();
        }
    }
    public Vector2 Velocity { get; set; }

    public bool HasInput => Input.LengthSquared() > 0f;

    public float MaxSpeed { get; set; } = 1f;
    public float AccelerationFactor { get; set; } = 0f;
    public float DeccelerationDrag { get; set; } = 1f;

    public Vector2 Acceleration
    {
        get => _acceleration;
        set
        {
            _acceleration.X = Math.Clamp(value.X, -1f, 1f);
            _acceleration.Y = Math.Clamp(value.Y, -1f, 1f);
        }
    }

    private Vector2 _acceleration = Vector2.Zero;
    private Vector2 _input        = Vector2.Zero;

    public Motor Clone() => new()
    {
        Acceleration       = Acceleration,
        AccelerationFactor = AccelerationFactor,
        Input              = Input,
        MaxSpeed           = MaxSpeed,
        Mode               = Mode,
        Velocity           = Velocity,
    };
}

public enum MotorMode
{
    Local,
    Global
}
