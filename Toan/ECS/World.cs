using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Resources;
using Toan.ECS.Systems;
using Toan.Logging;
using Toan.Rendering;
using Toan.UI;

namespace Toan.ECS;

public class World
{
    public required Logger Log { get; init; }

    private readonly ComponentRepository _componentRepo = new();
    private readonly HashSet<Guid> _entities = new();
	private readonly HashSet<Guid> _toBeDestroyed = new();
    private readonly Dictionary<Guid, Resource> _resources = new();

    private readonly SystemRepository _systems = new();
	private readonly HashSet<Action<World>> _startupSystems = new();

    public readonly Events Events = new();

	private float Timestamp => (float)Time.TotalGameTime.TotalSeconds;

    public bool IsDirty { get; private set; } = false;

    public GameTime Time { get; private set; } = new();

    public void Dirty() => IsDirty = true;
    public void Dirty(Guid entityId)
    {
        Dirty();
        Events.ChangeEntity(entityId);
    }
    public void Dirty<T>(Guid entityId)
        where T : struct
    {
        Dirty();
        Events.ChangeEntity<T>(entityId);
    }

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
        Time = gameTime;

        _systems.Update(this);

        Events.Clear();

		if (_toBeDestroyed.Count > 0)
		{
			foreach (var toDestroy in _toBeDestroyed)
			{
				DestroyEntity(toDestroy);
			}

			_toBeDestroyed.Clear();
            Dirty();
		}
    }
    /// <summary>
    /// Main Render loop
    /// </summary>
    public void Draw(Renderer renderer, GameTime gameTime)
    {
        Time = gameTime;
        _systems.Render(this, renderer);
    }

    /// <summary>
    /// Adds a new entity to the world and returns an <see cref="ECS.Entity">Entity</see> representing it
    /// </summary>
    /// <returns>An <see cref="ECS.Entity">Entity</see> representing the newly added entity</returns>
    public IEntity CreateEntity()
    {
        Guid entityId = AddNewEntity();
        return new VirtualEntity()
        {
            Id         = entityId,
            World      = this,
        };
    }
    /// <summary>
    /// Adds a new Entity to the world with a <see cref="Transform"/> component at position <paramref name="pos"/>, and returns an <see cref="ECS.Entity">Entity</see> representing it
    /// </summary>
    /// <param name="pos">The position in world-space to initialize the <see cref="Transform"/> component to</param>
    /// <returns>An <see cref="ECS.Entity">Entity</see> representing the newly added entity</returns>
    public IEntity CreateEntity(Vector2 pos)
    {
        Transform transform = new() { Position = pos };
        return CreateEntity().With(transform);
    }

    public void MakeEntity(IEntity entity)
    {
        if (entity.IsReal)
            return;

        Entity worldEntity;
        if (!HasEntity(entity.Id))
            AddNewEntity(entity.Id);

        worldEntity = Entity(entity.Id);
        if (worldEntity.Count() > 1)
            throw new InvalidOperationException($"Tried to make entity but placeholder entity {entity.Id} has components");

        object[] components = entity.GetAll();
        _componentRepo.AddAll(entity.Id, components);
    }

    public UIEntity CreateUI()
        => CreateUI(null);
    public UIEntity CreateUI(Guid? parentId)
    {
        Guid entityId = AddNewEntity();

        return new UIEntity()
        {
            Components = _componentRepo,
            Id         = entityId,
            World      = this,
        }.WithBundle(new UIBundle { ParentNodeId = parentId });
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
        return new Entity()
        {
            Components = _componentRepo,
            Id         = entityId,
            World      = this,
        };
    }

    /// <summary>
    /// Checks whether an entity with the given Id exists in the world
    /// </summary>
    /// <param name="entityId">The Id of the entity to look for</param>
    /// <returns>True if an entity with the given Id exists, false otherwise</returns>
    public bool HasEntity(Guid entityId)
        => _entities.Contains(entityId);

    public IEntity DuplicateEntity(Guid entityId)
    {
        var entity = Entity(entityId);
        var newEntity = CreateEntity();

        var components = _componentRepo.GetAll(entity.Id);
        _componentRepo.AddAll(newEntity.Id, components);

        return newEntity;
    }

    public SystemBuilder Systems()
    => new()
    {
        Systems = _systems,
        World   = this,
    };

    public Resource Resource(Guid resourceId)
    {
        if (!_resources.ContainsKey(resourceId)) throw new ArgumentException($"Resource {resourceId} does not exist in scene");
        return _resources[resourceId];
    }

    public TResource Resource<TResource>(Guid resourceId)
        where TResource : Resource
    {
        var resource = Resource(resourceId);
        if (resource is TResource res) return res;
        else throw new ArgumentException($"Resource {resourceId} is not of type {typeof(TResource).Name}");
    }
    public TResource Resource<TResource>()
        where TResource : Resource
    {
        foreach (var kvp in _resources)
        {
            var resource = kvp.Value;
            if (resource is TResource res) return res;
        }
        throw new ArgumentException($"No resource of type {typeof(TResource).Name} found");
    }

    #region Add
    /// <summary>
    /// Adds a new entity to the world and returns its Id
    /// </summary>
    /// <returns>The Id of the newly added entity</returns>
    public Guid AddNewEntity(Guid? id = null)
    {
        Guid entityId = id ?? GetNewGuid();

        if (!_entities.Add(entityId))
            throw new InvalidOperationException($"Tried to add entity with id {entityId}, but id is in use");
        Events.AddEntity(entityId);
        _componentRepo.Add(entityId, new EntityData { CreatedAt = Timestamp });

        Dirty();
        return entityId;
    }

    public Guid AddResource(Resource resource)
    {
        _resources.Add(resource.Id, resource);
        return resource.Id;
    }

    public void AddStartupSystem(Action<World> startupSystem)
        => _startupSystems.Add(startupSystem);

    public void AddPlugin<TPlugin>()
        where TPlugin : Plugin, new()
    => AddPlugin(new TPlugin());
    public void AddPlugin(Plugin plugin)
        => plugin.Build(this);
    #endregion

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

	private void DestroyEntity(Guid entityId)
	{
        _componentRepo.RemoveAll(entityId);
		_entities.Remove(entityId);
        Events.RemoveEntity(entityId);
	}
}

