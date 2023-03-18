using Toan.ECS;
using Toan.ECS.Query;
using Toan.ECS.Systems;
using Toan.Rendering;

namespace Toan.Test.Stub;

public class StubSystem
{
    public bool UpdateCalled { get; private set; }
    public bool RenderCalled { get; private set; }
    public bool UpdateComponentsCalled { get; private set; }

    public WorldQuery<StubComponent> Archetype { get; private set; } = new();

    [UpdateSystem]
    public void Update(World _world)
    {
        UpdateCalled = true;
    }

    [RenderSystem]
    public void Render(World _world, Renderer _renderer)
    {
        RenderCalled = true;
    }

    [EntitySystem(nameof(Archetype))]
    public void UpdateComponents(World _world, IReadOnlySet<Guid> _entities)
    {
        UpdateComponentsCalled = true;
    }

    public SystemInfo SystemInfo
    => new SystemInfo()
    {
        System = this,
        EntitySystem = typeof(StubSystem).GetMethod(nameof(UpdateComponents)),
        EntityQuery = typeof(StubSystem).GetProperty(nameof(Archetype)),
        RenderSystem = typeof(StubSystem).GetMethod(nameof(Render)),
        UpdateSystem = typeof(StubSystem).GetMethod(nameof(Update)),
    };
}
