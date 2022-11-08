using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Resources;
using Toan.ECS.Systems;
using Toan.Rendering;

namespace Toan.ECS;

public class World
{
    private readonly ComponentRepository _componentRepo = new();
    private readonly HashSet<Guid> _entities = new();
	private readonly HashSet<Guid> _toBeDestroyed = new();
    private readonly Dictionary<Guid, Resource> _resources = new();

    private readonly HashSet<IUpdatable> _updateSystems = new();
    private readonly HashSet<IRenderable> _renderSystems = new();
    private readonly HashSet<IEntitySystem> _entitySystems = new();
	private readonly HashSet<Action<World>> _startupSystems = new();

	private GameTime? _lastGameTime;
	private float Timestamp => (float)(_lastGameTime?.TotalGameTime.TotalSeconds ?? 0.0);

    private bool _isDirty = false;

    public void Dirty() => _isDirty = true;

	public void Awake()
	{
		foreach (var startupSystem in _startupSystems)
		{
			startupSystem.Invoke(this);
		}
	}

    /// <summary>
    /// Main Update loop
    /// </summary>
    public void Update(GameTime gameTime)
    {
		_lastGameTime = gameTime;

        if (_isDirty) UpdateComponents();

        foreach (var system in _updateSystems)
        {
            system.Update(this, gameTime);
        }

		if (_toBeDestroyed.Any())
		{
			foreach (var toDestroy in _toBeDestroyed)
			{
				DestroyEntity(toDestroy);
			}

			UpdateComponents();
			_toBeDestroyed.Clear();
		}
    }
    /// <summary>
    /// Main Render loop
    /// </summary>
    public void Draw(Renderer renderer, GameTime gameTime)
    {
        foreach (var system in _renderSystems)
        {
            system.Render(this, renderer, gameTime);
        }
    }

    /// <summary>
    /// Adds a new Entity to the world and returns an <see cref="ECS.Entity">Entity</see> representing it
    /// </summary>
    /// <returns>An <see cref="ECS.Entity">Entity</see> representing the newly added entity</returns>
    public Entity CreateEntity()
    {
        Guid entity = AddNewEntity();
        return new
        (
            entity,
            this,
            _componentRepo
		);
    }
    /// <summary>
    /// Adds a new Entity to the world with a <see cref="Transform"/> component at position <paramref name="pos"/>, and returns an <see cref="ECS.Entity">Entity</see> representing it
    /// </summary>
    /// <param name="pos">The position in world-space to initialize the <see cref="Transform"/> component to</param>
    /// <returns>An <see cref="ECS.Entity">Entity</see> representing the newly added entity</returns>
    public Entity CreateEntity(Vector2 pos)
    {
        Transform transform = new() { Position = pos };
        return CreateEntity().With(transform);
    }

    /// <summary>
    /// Adds a new entity to the world and returns it's Id
    /// </summary>
    /// <returns>The Id of the newly added entity</returns>
    public Guid AddNewEntity()
    {
        Guid entityId = GetNewGuid();
        _entities.Add(entityId);
        _componentRepo.Add(entityId, new EntityData { CreatedAt = Timestamp });
        _isDirty = true;
        return entityId;
    }

    /// <summary>
    /// Attempts to destroy an item with the Id <paramref name="destroyId"/>.
    /// First checks if the Id is associated with a Resource
    /// If it is, removes it, otherwise, check if the Id is associated with an Entity
    /// If it is, flag it to be destroyed, otherwise, do nothing
    /// </summary>
    /// <param name="destroyId">The Id to destroy</param>
	public void Destroy(Guid destroyId)
	{
        if (!_resources.Remove(destroyId))
            if (_entities.Contains(destroyId))
                _toBeDestroyed.Add(destroyId);
    }

    /// <summary>
    /// Creates an <see cref="ECS.Entity">Entity</see> representing the entity indicated by <paramref name="entityId"/>
    /// </summary>
    /// <param name="entityId">The Id of the entity to fetch</param>
    /// <returns>The created <see cref="ECS.Entity">Entity</see></returns>
    /// <exception cref="ArgumentException">The entityId does not correspond to an entity in the world</exception>
    public Entity Entity(Guid entityId)
    {
        if (!_entities.Contains(entityId)) throw new ArgumentException($"Entity {entityId} does not exist in scene!");
        return new Entity
        (
            entityId,
            this,
            _componentRepo
        );
    }

    public Resource Resource(Guid resourceId)
    {
        if (!_resources.ContainsKey(resourceId)) throw new ArgumentException($"Resource {resourceId} does not exist in scene");
        return _resources[resourceId];
    }

    public T Resource<T>(Guid resourceId)
        where T : Resource
    {
        var resource = Resource(resourceId);
        if (resource is T res) return res;
        else throw new ArgumentException($"Resource {resourceId} is not of type {typeof(T).Name}");
    }
    public T Resource<T>()
        where T : Resource
    {
        foreach (var kvp in _resources)
        {
            var resource = kvp.Value;
            if (resource is T res) return res;
        }
        throw new ArgumentException($"No resource of type {typeof(T).Name} found");
    }

    public Guid AddResource(Resource resource)
    {
        _resources.Add(resource.Id, resource);
        return resource.Id;
    }

    public void AddSystem<TSystem>()
        where TSystem : IGameSystem, new()
    {
        AddSystem(new TSystem());
    }
    public void AddSystem(IGameSystem system)
    {
        if (system is IUpdatable updateSystem) _updateSystems.Add(updateSystem);
        else if (system is IRenderable renderSystem) _renderSystems.Add(renderSystem);

        if (system is IEntitySystem entitySystem) _entitySystems.Add(entitySystem);
        else if (system is not IUpdatable or IRenderable)
			throw new ArgumentException($"System type of {system.GetType().Name} is not renderable or updatable");

    }
	public void AddStartupSystem(Action<World> startupSystem)
	{
		_startupSystems.Add(startupSystem);
	}

    public void AddPlugin<TPlugin>()
        where TPlugin : Plugin, new()
    => AddPlugin(new TPlugin());
    public void AddPlugin(Plugin plugin)
    {
        plugin.Build(this);
    }

    public QueryExecutor GetQueryExecutor(IReadOnlySet<Type> types)
    => new()
    {
        Components = _componentRepo,
        Entities   = _entities,
        Types      = types,
        World      = this,
    };

    private Guid GetNewGuid()
    {
        Guid guid;
        do guid = Guid.NewGuid();
        while (_entities.Contains(guid));
        return guid;
    }

    private void UpdateComponents()
    {
        foreach (var system in _entitySystems)
        {
            var query = system.Archetype;
            system.UpdateComponents(this, query.GetEntities(this));
        }

        _isDirty = false;
    }

	private void DestroyEntity(Guid entityId)
	{
		_entities.Remove(entityId);
	}
}

