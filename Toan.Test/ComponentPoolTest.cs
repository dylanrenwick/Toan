using Toan.ECS.Components;

namespace Toan.Test;

public class ComponentPoolTest
{
    private readonly static Random _random = new();

    private struct StubComponent
    {
        public int StubData { get; set; }
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

    [Fact]
    public void Count_WithEmptyPool_ReturnsZero()
    {
        Assert.Empty(_componentPool);
        Assert.Equal(0, _componentPool.Count);
    }

    [Theory]
    [MemberData(nameof(GetRandomStubData), parameters: 1000)]
    public void Add_Get_ReturnsCorrectComponent(int stubData)
    {
        Guid guid = Guid.NewGuid();
        var component = new StubComponent
        {
            StubData = stubData,
        };

        _componentPool.Add(guid, component);

        var foundComponent = _componentPool.Get(guid);

        Assert.Equal(
            component.StubData,
            foundComponent.StubData
        );
    }

    [Theory]
    [MemberData(nameof(GetRandomStubData), parameters: 1000)]
    public void Add_HasEntity_ReturnsTrue(int stubData)
    {
        Guid guid = Guid.NewGuid();
        var component = new StubComponent
        {
            StubData = stubData,
        };

        Assert.False(_componentPool.HasEntity(guid));

        _componentPool.Add(guid, component);

        Assert.True( _componentPool.HasEntity(guid));
    }

    [Theory]
    [MemberData(nameof(GetRandomStubData), parameters: 1000)]
    public void Add_Remove_ReturnsTrue(int stubData)
    {
        Guid guid = Guid.NewGuid();
        var component = new StubComponent
        {
            StubData = stubData,
        };

        _componentPool.Add(guid, component);

        Assert.True(_componentPool.Remove(guid));
    }

    public static IEnumerable<object[]> GetRandomStubData(int count)
    {
        List<object[]> data = new List<object[]>();
        for (int i = 0; i < count; i++)
        {
            data.Add(new object[] { _random.Next() });
        }
        return data;
    }
}