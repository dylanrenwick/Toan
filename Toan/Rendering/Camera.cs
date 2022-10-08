﻿using Microsoft.Xna.Framework;

using GameComponent = Toan.ECS.Components.GameComponent;

namespace Toan.Rendering;

public class Camera : GameComponent
{
    public CameraAnchor Anchor { get; init; } = CameraAnchor.TopLeft;

    public Vector2 WorldPosition { get; set; }

    public Vector2 ViewOffset => AnchorOffset + WorldPosition;

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