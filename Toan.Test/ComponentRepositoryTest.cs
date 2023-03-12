﻿
using Toan.ECS.Components;
using Toan.Test.Stub;

namespace Toan.Test;

public class ComponentRepositoryTest
{
    private readonly ComponentRepository _repository;
    private readonly Guid _entityId;
    private readonly StubComponent _component;

    public ComponentRepositoryTest()
    {
        _repository = new ComponentRepository();
        _entityId = Guid.NewGuid();
        _component = new StubComponent { StubData = 42 };
    }

    [Fact]
    public void Add_AddsComponentToRepository()
    {
        _repository.Add(_entityId, _component);

        Assert.True(_repository.Has<StubComponent>(_entityId));
    }

    [Fact]
    public void Add_EntityHasComponent_OverwritesComponent()
    {
        var component2 = new StubComponent { StubData = 43 };

        _repository.Add(_entityId, _component);
        _repository.Add(_entityId, component2);

        var foundComponent = _repository.Get<StubComponent>(_entityId);

        Assert.Equal(
            component2.StubData,
            foundComponent.StubData
        );
    }

    [Fact]
    public void Remove_ComponentExists_RemovesComponent()
    {
        _repository.Add(_entityId, _component);

        bool success = _repository.Remove<StubComponent>(_entityId);

        Assert.True(success);
        Assert.False(_repository.Has<StubComponent>(_entityId));
    }

    [Fact]
    public void Remove_ComponentNotExists_ReturnsFalse()
    {
        bool success = _repository.Remove<StubComponent>(_entityId);

        Assert.False(success);
    }

    [Fact]
    public void RemoveAll_ComponentsExist_RemovesAllComponents()
    {
        _repository.Add(_entityId, _component);

        bool success = _repository.RemoveAll(_entityId);

        Assert.True(success);
        Assert.False(_repository.Has<StubComponent>(_entityId));
    }

    [Fact]
    public void RemoveAll_ComponentsNotExist_ReturnsFalse()
    {
        bool success = _repository.RemoveAll(_entityId);

        Assert.False(success);
    }

    [Fact]
    public void Get_ComponentExists_ReturnsComponent()
    {
        _repository.Add(_entityId, _component);

        StubComponent retrievedComponent = _repository.Get<StubComponent>(_entityId);

        Assert.Equal(_component, retrievedComponent);
    }

    [Fact]
    public void Get_ComponentNotExists_Throws()
    {
        Assert.Throws<ArgumentException>(() => _repository.Get<StubComponent>(_entityId));
    }
}