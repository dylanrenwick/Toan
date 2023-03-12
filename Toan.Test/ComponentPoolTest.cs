using Toan.ECS.Components;

namespace Toan.Test;

public class ComponentPoolTest
{
    private struct StubComponent { }

    private struct StubComponent
    {
        public byte StubData { get; set; }
    }

    private readonly ComponentPool<StubComponent> _componentPool;

    public ComponentPoolTest()
    {
        _componentPool = new();
    }

    [Fact]
    public void Add_WithIncorrectType_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => _componentPool.Add(
                Guid.NewGuid(),
                new object()
            )
        );
    }

    [Fact]
    public void Get_WithNewGuid_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => _componentPool.Get(Guid.NewGuid())
        );
    }

    [Fact]
    public void Remove_WithNewGuid_ReturnsFalse()
    {
        Assert.False(_componentPool.Remove(Guid.NewGuid()));
    }

    [Fact]
    public void HasEntity_WithNewGuid_ReturnsFalse()
    {
        Assert.False(_componentPool.HasEntity(Guid.NewGuid()));
    }
}