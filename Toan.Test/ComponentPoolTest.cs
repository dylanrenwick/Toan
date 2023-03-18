using Toan.ECS.Components;
using Toan.Test.Stub;

namespace Toan.Test;

public class ComponentPoolTest
{
    private readonly static Random _random = new();

    private readonly ComponentPool<StubComponent> _componentPool;
    private readonly Guid _entityId;
    private readonly StubComponent _component;

    public ComponentPoolTest()
    {
        _componentPool = new();
        _entityId = Guid.NewGuid();
        _component = new StubComponent()
        {
            StubData = _random.Next()
        };
    }

    [Fact]
    public void Add_AddsComponentToPool()
    {
        _componentPool.Add(_entityId, _component);

        Assert.True( _componentPool.HasEntity(_entityId));
    }

    [Fact]
    public void Add_EntityHasComponent_OverwritesComponent()
    {
        var component2 = new StubComponent()
        {
            StubData = _random.Next()
        };

        _componentPool.Add(_entityId, _component);
        _componentPool.Add(_entityId, component2);

        var foundComponent = _componentPool.Get(_entityId);

        Assert.Equal(1, _componentPool.Count);
        Assert.Equal(
            component2.StubData,
            foundComponent.StubData
        );
    }

    [Fact]
    public void Add_TypeNotStruct_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => _componentPool.Add(
                _entityId,
                new StubComponent2()
            )
        );
    }

    [Fact]
    public void Get_EntityNotExists_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => _componentPool.Get(_entityId)
        );
    }

    [Fact]
    public void Remove_EntityNotExists_ReturnsFalse()
    {
        Assert.False(_componentPool.Remove(_entityId));
    }

    [Fact]
    public void Remove_ComponentExists_ReturnsTrue()
    {
        _componentPool.Add(_entityId, _component);

        Assert.True(_componentPool.Remove(_entityId));
    }

    [Fact]
    public void HasEntity_EntityNotExists_ReturnsFalse()
    {
        Assert.False(_componentPool.HasEntity(_entityId));
    }

    [Fact]
    public void Count_EmptyPool_ReturnsZero()
    {
        Assert.Empty(_componentPool);
        Assert.Equal(0, _componentPool.Count);
    }

    [Fact]
    public void Get_ComponentExists_ReturnsComponent()
    {
        _componentPool.Add(_entityId, _component);

        var foundComponent = _componentPool.Get(_entityId);

        Assert.Equal(
            _component.StubData,
            foundComponent.StubData
        );
    }

    [Fact]
    public void Count_ReturnsCorrectCount()
    {
        int expectedCount = _random.Next(511) + 1;
        for (int i = 0; i < expectedCount; i++)
        {
            var component = new StubComponent()
            {
                StubData = _random.Next(),
            };
            _componentPool.Add(Guid.NewGuid(), component);
        }

        Assert.Equal(expectedCount, _componentPool.Count);
    }
}