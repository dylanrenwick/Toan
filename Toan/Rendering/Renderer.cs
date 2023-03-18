using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.Rendering;

public partial class Renderer
{
    private static Texture2D? _pixel;

    public Texture2D Pixel
    {
        get
        {
            _pixel ??= CreatePixel();
            return _pixel;
        }
    }

    private readonly GraphicsDeviceManager _graphics;
    private readonly GraphicsDevice _device;
    private readonly SpriteBatch _spriteBatch;

    public float RenderScale { get; set; }
    public Camera? MainCamera { get; set; }

    public Vector2 ScreenSize => new(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

    public Renderer(GraphicsDeviceManager graphics, GraphicsDevice device)
    {
        _graphics = graphics;
        _device = device;

        _spriteBatch = new(_device);
    }

    public void Clear(Color color)
    {
        _device.Clear(color);
    }

    public void Begin()
    {
        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );
    }
    public void End()
    {
        _spriteBatch.End();
    }

    #region Draw Overloads

    public void Draw(DrawTextureCall drawCall)
    => _spriteBatch.Draw(
        texture              : drawCall.Texture,
        destinationRectangle : ScaleAndOffsetRect(drawCall.DestRect),
        sourceRectangle      : drawCall.SourceRect,
        color                : drawCall.Color,
        rotation             : drawCall.Rotation,
        origin               : drawCall.Origin,
        effects              : drawCall.SpriteEffects,
        layerDepth           : drawCall.LayerDepth
    );

    public void DrawString(DrawStringCall drawCall)
    => _spriteBatch.DrawString(
        spriteFont : drawCall.Font,
        text       : drawCall.Text,
        position   : drawCall.Position * RenderScale,
        color      : drawCall.Color,
        rotation   : drawCall.Rotation,
        origin     : drawCall.Origin,
        scale      : drawCall.Scale,
        effects    : drawCall.SpriteEffects,
        layerDepth : drawCall.LayerDepth
    );

    #endregion

    private Rectangle ScaleAndOffsetRect(Rectangle rect)
    {
        Rectangle newRect = new(
            (rect.Location.ToVector2() * RenderScale).ToPoint(),
            (rect.Size.ToVector2() * RenderScale).ToPoint()
        );

        var cameraOffset = MainCamera?.ViewOffset ?? Vector2.Zero;
        newRect.Offset(cameraOffset);
        return newRect;
    }

    private Texture2D CreatePixel()
    {
        Texture2D pixel = new(_device, 1, 1, false, SurfaceFormat.Color);
        pixel.SetData(new[] { Color.White });
        return pixel;
    }
}
