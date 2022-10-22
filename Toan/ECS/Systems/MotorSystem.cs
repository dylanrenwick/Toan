using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;

namespace Toan.ECS.Systems;

public class MotorSystem : EntityUpdateSystem
{
    public override WorldQuery<Motor, Transform> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime time)
    {
        var motor     = entity.Components.Get<Motor>();
        var transform = entity.Components.Get<Transform>();
        var deltaTime = (float)time.ElapsedGameTime.TotalSeconds;

        UpdateAcceleration(motor, deltaTime);
        
        UpdateVelocity(motor, deltaTime);
        UpdatePosition(transform, motor);
    }

    private static void UpdateAcceleration(Motor motor, float deltaTime)
    {
        bool accelerating = motor.HasInput;
        Vector2 input = (accelerating ? motor.Input : motor.Velocity);
        float deccelerationFlag = (accelerating ? 1f : -1f * motor.DeccelerationDrag);

        if (motor.AccelerationFactor > 0f)
        {
            Vector2 accel = input * deltaTime / motor.AccelerationFactor;
            motor.Acceleration += accel * deccelerationFlag;
        }
        else
        {
            motor.Acceleration = input * deccelerationFlag;
        }
    }
    private static void UpdateVelocity(Motor motor, float deltaTime)
    {
        float moveDelta = motor.MaxSpeed * deltaTime;
        motor.Velocity = moveDelta * motor.Acceleration;

        if (float.IsNaN(motor.Velocity.X) || float.IsNaN(motor.Velocity.Y))
        {
            motor.Velocity = new(
                float.IsNaN(motor.Velocity.X) ? 0 : motor.Velocity.X,
                float.IsNaN(motor.Velocity.Y) ? 0 : motor.Velocity.Y
            );
        }
    }

    private static void UpdatePosition(Transform transform, Motor motor)
    {
        switch (motor.Mode)
        {
            case MotorMode.Local:
                transform.LocalPosition += motor.Velocity;
                break;
            case MotorMode.Global:
                transform.GlobalPosition += motor.Velocity;
                break;
        }
    }
}
