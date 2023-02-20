using Microsoft.Xna.Framework;

using Toan.ECS.Components;

namespace Toan.Rendering;

public struct Camera : IComponent
{
    public CameraAnchor Anchor { get; init; } = CameraAnchor.TopLeft;

    public Vector2 WorldPosition { get; set; }

    public Vector2 ViewOffset => AnchorOffset + WorldPosition;

    public float WorldScale { get; set; }

    public Vector2 ScreenSize { get; set; }

    public Vector2 AnchorOffset => Anchor switch
    {
        CameraAnchor.TopLeft => Vector2.Zero,
        CameraAnchor.Left => new(0f, 0.5f),
        CameraAnchor.BottomLeft => new(0f, 1f),
        CameraAnchor.Top => new(0.5f, 0f),
        CameraAnchor.Center => new(0.5f, 0.5f),
        CameraAnchor.Bottom => new(0.5f, 1f),
        CameraAnchor.TopRight => new(1f, 0f),
        CameraAnchor.Right => new(1f, 0.5f),
        CameraAnchor.BottomRight => new(1f, 1f),
        _ => Vector2.Zero,
    };

    public Camera() { }

    public Vector2 ScreenToWorld(Vector2 screenSpace)
        => (screenSpace / ScreenSize) * (ScreenSize / WorldScale) - ViewOffset;
}
public enum CameraAnchor
{
    TopLeft,
    Left,
    BottomLeft,
    Top,
    Center,
    Bottom,
    TopRight,
    Right,
    BottomRight,
}
