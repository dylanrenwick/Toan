using System;

using Microsoft.Xna.Framework;

using Toan.Rendering;
using Toan.ECS;
using Toan.ECS.Resources;
using Toan.ECS.Systems;

namespace Toan;

public abstract class ToanGame : Game
{
    private readonly GraphicsDeviceManager _graphics;

    private Renderer? _renderer;
    public Renderer Renderer => _renderer ?? throw new Exception("Tried to access Renderer before initialized!");

    protected World World;

    private bool _isFirstUpdate;

    public ToanGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        World = new();
    }

    protected void AddPlugin<TPlugin>()
        where TPlugin : Plugin, new()
    => AddPlugin(new TPlugin());
    protected void AddPlugin(Plugin plugin) => plugin.Build(World);

    protected Guid AddResource<TResource>()
        where TResource : Resource, new()
    => AddResource(new TResource());
    protected Guid AddResource(Resource resource) => World.AddResource(resource);

    protected void AddSystem<TSystem>()
        where TSystem : IGameSystem, new()
    => AddSystem(new TSystem());
    protected void AddSystem(IGameSystem system) => World.AddSystem(system);

    protected Entity CreateEntity() => World.CreateEntity();
    protected Entity CreateEntity(Vector2 pos) => World.CreateEntity(pos);

    /// <summary>
    /// Called before the first update tick
    /// Triggers startup systems
    /// </summary>
    protected virtual void Awake()
    {
		World.Awake();
    }

    /// <summary>
    /// Runs during MonoGame's LoadContent stage
    /// Adds Resources, Systems, and Plugins to the world
    /// </summary>
    protected virtual void Build()
    {
        AddResource(new ContentServer { Content = Content });
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        
        _isFirstUpdate = true;
        _renderer = new(_graphics, GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Build();
    }

    protected override void Update(GameTime gameTime)
    {
        if (_isFirstUpdate)
        {
            Awake();
            _isFirstUpdate = false;
        }

        World?.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_renderer == null) return;

        _renderer.Clear(Color.CornflowerBlue);
        _renderer.Begin();

        World?.Draw(_renderer, gameTime);

        _renderer.End();
        base.Draw(gameTime);
    }
}
