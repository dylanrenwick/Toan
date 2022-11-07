using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;

namespace Toan.ECS.Systems;

public class MotorSystem : EntityUpdateSystem
{
    public override WorldQuery<Motor, Transform> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime time)
    {
        ref var motor     = ref entity.Get<Motor>();
        ref var transform = ref entity.Get<Transform>();
        var deltaTime = (float)time.ElapsedGameTime.TotalSeconds;

        UpdateAcceleration(ref motor, deltaTime);
        
        UpdateVelocity(ref motor, deltaTime);
        UpdatePosition(ref transform, ref motor);
    }

    private static void UpdateAcceleration(ref Motor motor, float deltaTime)
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
    private static void UpdateVelocity(ref Motor motor, float deltaTime)
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

    private static void UpdatePosition(ref Transform transform, ref Motor motor)
    {
        switch (motor.Mode)
        {
            case MotorMode.Local:
                transform.Position += motor.Velocity;
                break;
            case MotorMode.Global:
                transform.Position += motor.Velocity;
                break;
        }
    }
}
