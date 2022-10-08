using Microsoft.Xna.Framework.Content;

namespace Toan.ECS.Resources;

public class ContentServer : Resource
{
    public required ContentManager Content { private get; init; }

    public T Load<T>(string assetName) => Content.Load<T>(assetName);
}
