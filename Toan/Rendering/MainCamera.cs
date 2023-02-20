using Toan.ECS.Components;

namespace Toan.Rendering;

public struct MainCamera : IComponent
{
    public static Camera MainCameraEntity { get; set; }
}

