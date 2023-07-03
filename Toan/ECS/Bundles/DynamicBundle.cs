using System.Collections.Generic;
using Toan.ECS.Components;

namespace Toan.ECS.Bundles;

public class DynamicBundle : IBundle
{
    public HashSet<IComponent> Components { get; init; } = new();

    public HashSet<IComponent> FlattenBundle()
        => Components;

    public void AddBundle(Entity entity)
    {
        foreach (var component in Components)
        {
            entity.With(component);
        }
    }

    public static DynamicBundleBuilder Build()
        => new();

    public readonly struct DynamicBundleBuilder
    {
        public HashSet<IComponent> Components { get; init; } = new();

        public DynamicBundleBuilder() { }

        public DynamicBundleBuilder With(IComponent component)
        {
            Components.Add(component);
            return this;
        }

        public DynamicBundleBuilder WithBundle(IBundle bundle)
        {
            Components.UnionWith(bundle.FlattenBundle());
            return this;
        }

        public DynamicBundle Bundle()
            => new() { Components = Components };
    }
}
