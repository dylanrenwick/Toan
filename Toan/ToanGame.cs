using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Toan.Rendering;
using Toan.ECS;
using Toan.ECS.Resources;
using Toan.Logging;
using Toan.Logging.Color;

namespace Toan;

public abstract class ToanGame : Game
{
    private Renderer? _renderer;
    public Renderer Renderer => _renderer ?? throw new Exception("Tried to access Renderer before initialized!");

    private readonly GraphicsDeviceManager _graphics;
    private bool _isFirstUpdate;

    protected readonly World World;
    protected readonly Logger Log;

    public ToanGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Log = new Logger
        {
            Destinations = new List<ILogDestination<LogMessage>>()
            {
                new ColoredDestination
                {
                    Color       = new AnsiColorConverter(),
                    Destination = new ConsoleDestination()
                },
            },
            Label = "TOAN"
        };

        World = new()
        {
            Log = Log.GetChildLogger("WRLD")
        };
    }

    protected void AddPlugin<TPlugin>()
        where TPlugin : Plugin, new()
    => AddPlugin(new TPlugin());
    protected void AddPlugin(Plugin plugin) => plugin.Build(World);

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
        World.AddResource(new ContentServer { Content = Content });
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

