using Toan.ECS.Components;
using Toan.Test.Stub;

namespace Toan.Test;

public class ComponentPoolTest
{
    private readonly static Random _random = new();

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

    [Fact]
    public void Add_HasEntity_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        var component = new StubComponent
        {
            StubData = 0,
        };

        Assert.False(_componentPool.HasEntity(guid));

        _componentPool.Add(guid, component);

        Assert.True( _componentPool.HasEntity(guid));
    }

    [Fact]
    public void Add_Remove_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        var component = new StubComponent
        {
            StubData = 0,
        };

        _componentPool.Add(guid, component);

        Assert.True(_componentPool.Remove(guid));
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
    [MemberData(nameof(GetRandomSizedPools), parameters: new object[] { 512, 1000 })]
    public void Add_Count_ReturnsCorrectCount(params int[] data)
    {
        int expectedCount = data.Length;
        var components = data.Select(stub => new StubComponent { StubData = stub });
        foreach (var component in components)
        {
            _componentPool.Add(Guid.NewGuid(), component);
        }

        Assert.Equal(_componentPool.Count, expectedCount);
    }

    public static IEnumerable<object[]> GetRandomStubData(int count)
    {
        List<object[]> data = new();
        for (int i = 0; i < count; i++)
        {
            data.Add(new object[] { _random.Next() });
        }
        return data;
    }

    public static IEnumerable<object[]> GetRandomSizedPools(int maxSize, int count)
    {
        List<object[]> data = new();
        for (int i = 0; i < count; i++)
        {
            int poolSize = _random.Next(maxSize - 1) + 1;
            object[] pool = new object[poolSize];
            for (int j = 0; j < poolSize; j++)
            {
                pool[j] = _random.Next();
            }
            data.Add(pool);
        }
        return data;
    }
}