namespace Toan.Physics;

public struct CollisionMask
{
    public static CollisionMask All => new() { Mask = ulong.MaxValue };

    public required ulong Mask { get; init; }

    public bool Has(ulong flags)
        => (Mask & flags) != 0;

    public static CollisionMask From(params ulong[] flags)
    {
        ulong mask = 0;
        foreach (ulong flag in flags)
        {
            mask |= flag;
        }

        return new() { Mask = mask };
    }
}
