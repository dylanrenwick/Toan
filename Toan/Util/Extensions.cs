using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;

namespace Toan;

public static class Extensions
{
    /// <summary>
    /// Returns the angle of the vector relative to +X (right on the X axis)
    /// </summary>
    /// <returns>Angle of self in degrees</returns>
    public static float AngleDeg(this Vector2 self) => MathUtil.RadToDeg(self.AngleRad());
    /// <summary>
    /// Returns the angle of the vector relative to +X (right on the X axis)
    /// </summary>
    /// <returns>Angle of self in radians</returns>
    public static float AngleRad(this Vector2 self) => (float)Math.Atan2(self.Y, self.X);

    /// <summary>
    /// Clamps both the X and Y axes to the inclusive range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    public static Vector2 Clamp(this Vector2 self, float min, float max)
        => self.Clamp(min, min, max, max);
    /// <summary>
    /// Clamps the X axis to the inclusive range of <paramref name="min"/>.X and <paramref name="max"/>.X
    /// <para />
    /// Clamps the Y axis to the inclusive range of <paramref name="min"/>.Y and <paramref name="max"/>.Y
    /// </summary>
    public static Vector2 Clamp(this Vector2 self, Vector2 min, Vector2 max)
        => self.Clamp(min.X, min.Y, max.X, max.Y);
    /// <summary>
    /// Clamps the X axis to the inclusive range of <paramref name="minX"/> and <paramref name="maxX"/>
    /// <para />
    /// Clamps the Y axis to the inclusive range of <paramref name="minY"/> and <paramref name="maxY"/Y
    /// </summary>
    public static Vector2 Clamp(this Vector2 self, float minX, float minY, float maxX, float maxY)
    => new(
        x: Math.Clamp(self.X, minX, maxX),
        y: Math.Clamp(self.Y, minY, maxY)
    );

    #region internal extensions
    /*
     * These extensions are used throughout the Toan codebase for convenience
     * They are marked internal to avoid polluting the LINQ extensions for end-users of Toan
     */

    /// <summary>
    /// Filters a sequence based on type
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the input sequence</typeparam>
    /// <param name="type">The type to filter the input sequence to</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that are instances of <typeparamref name="T"/></returns>
	internal static IEnumerable<T> WhereType<T>(this IEnumerable<T> self, Type type)
		where T : class
	{
		return self.Where(type.IsInstanceOfType);
	}
    /// <summary>
    /// Filters and typecasts a sequence based on type
    /// </summary>
    /// <typeparam name="TSource">The type of elements contained in the input sequence</typeparam>
    /// <typeparam name="TResult">The type to filter the input sequence to</typeparam>
    /// <param name="self"></param>
    /// <returns>An <see cref="IEnumerable{TResult}"/> that contains elements from the input sequence that are instances of <typeparamref name="TResult"/></returns>
    /// <exception cref="UnreachableException"></exception>
    internal static IEnumerable<TResult> WhereType<TSource, TResult>(this IEnumerable<TSource> self)
        where TSource : class
        where TResult : class, TSource
    {
        return self
            .Where(i => i is TResult)
            .Select(i => i as TResult ?? throw new UnreachableException());
    }

    /// <summary>
    /// Transforms a dictionary into a readonly sequence of (<typeparamref name="TKey"/>, <typeparamref name="TValue"/>) tuples
    /// </summary>
    /// <returns>An <see cref="IReadOnlySet{T}"/> of (<typeparamref name="TKey"/>, <typeparamref name="TValue"/>) tuples</returns>
	internal static IReadOnlySet<(TKey, TValue)> Tuplize<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self)
	{
		return self
			.Select(kvp => (kvp.Key, kvp.Value))
            .ToHashSet();
	}

    internal static bool ImplementsInterface(this Type self, Type toCheck)
    {
        if (!toCheck.IsInterface)
            return false;

        return self.GetInterface(toCheck.Name) != null;
    }
    #endregion
}
