using System;
using System.Collections;
using System.Collections.Generic;

namespace Toan.ECS.Components;

public class ComponentPool<TComponent> : IComponentPool, IEnumerable<TComponent>
    where TComponent : struct
{
    private int _lastComponentIndex = -1;

    private TComponent[] _components = Array.Empty<TComponent>();
    private Guid[] _entityLink = Array.Empty<Guid>();
    private Dictionary<Guid, int> _entityMappings = new();

    public int Count => _lastComponentIndex + 1;

    public void Add(Guid entityId, in object component)
    {
        if (component.GetType() != typeof(TComponent))
            throw new ArgumentException($"{nameof(component)} is not of type {typeof(TComponent).FullName}");
        Add(entityId, (TComponent)component);
    }

    public void Add(Guid entityId, in TComponent component)
    {
        int nextIndex = _lastComponentIndex + 1;

        EnsureLength(ref _components, nextIndex + 1);
        EnsureLength(ref _entityLink, nextIndex + 1);

        _components[nextIndex] = component;
        _entityLink[nextIndex] = entityId;
        _entityMappings[entityId] = nextIndex;

        _lastComponentIndex++;
    }

    public bool HasEntity(Guid entityId)
    {
        return _entityMappings.ContainsKey(entityId);
    }

    public TComponent? Get(Guid entityId)
    {
        if (_entityMappings.TryGetValue(entityId, out int componentIndex))
        {
            return _components[componentIndex];
        }
        else
            return null;
    }

    public bool Remove(Guid entityId)
    {
        if (!_entityMappings.ContainsKey(entityId))
            return false;

        int componentIndex = _entityMappings[entityId];
        if (componentIndex == -1)
            return false;

        if (componentIndex != _lastComponentIndex)
        {
            var lastLink = _entityLink[_lastComponentIndex];
            _components[componentIndex] = _components[_lastComponentIndex];
            _entityMappings[lastLink] = componentIndex;
        }

        _lastComponentIndex--;
        return true;
    }

    public IEnumerator<TComponent> GetEnumerator() => ((IEnumerable<TComponent>)_components).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _components.GetEnumerator();

    private static void EnsureLength<T>(ref T[] arr, int minLength, int maxLength = int.MaxValue)
    {
        if (minLength >= arr.Length)
        {
            int newLength = arr.Length;
            if (newLength == 0)
                newLength++;

            do
            {
                newLength *= 2;
                if (newLength < 0)
                {
                    newLength = minLength + 1;
                }
            }
            while (newLength < minLength);
            newLength = Math.Min(newLength, maxLength);
            Array.Resize(ref arr, newLength);
        }
    }

}
