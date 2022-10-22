namespace Toan.Physics;

public class CircleCollider : Collider
{
    public required float Radius { get; set; }

    public override FloatRect AxisAlignedBoundingBox => new(Origin.X - Radius, Origin.Y - Radius, Radius * 2, Radius * 2);
}
