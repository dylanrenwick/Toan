using Toan.Debug;
using Toan.ECS;
using Toan.ECS.Systems;
using Toan.Input;
using Toan.Physics;
using Toan.Rendering;

namespace Toan;

/// <summary>
/// Loads the following core set of plugins and systems:
/// <example>
/// Toan.Input.InputPlugin
/// Toan.Rendering.CameraPlugin
/// Toan.Physics.PhysicsPlugin
/// Toan.ECS.Systems.MotorSystem
/// Toan.ECS.Systems.TextSystem
/// Toan.ECS.Systems.SpriteSystem
/// </example>
/// If compiled in debug mode, the following plugins are also loaded:
/// <example>
/// Toan.Debug.DebugPlugin
/// </example>
/// </summary>
public class CorePlugins : Plugin
{
    /// <summary>
    /// Whether the <see cref="Debug.Debug"/> component should be automatically added to compatible entities
    /// </summary>
	public bool AutoApplyDebug { get; init; } = false;

    public override void Build(World world)
    {
        world.AddPlugin<InputPlugin>();
        world.AddPlugin<CameraPlugin>();

        world.Systems()
            .Add<MotorSystem>()
            .Add<TextSystem>()
            .Add<SpriteSystem>();

        world.AddPlugin<PhysicsPlugin>();

#if DEBUG
        world.AddPlugin(new DebugPlugin { AutoApply = AutoApplyDebug });
#endif
    }
}
