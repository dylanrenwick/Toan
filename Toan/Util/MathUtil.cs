using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Toan;

/// <summary>
/// Provides several useful math functions and constants
/// <para />
/// Includes some constants from <see cref="Math"/> typecast to <see cref="float"/>
/// </summary>
public static class MathUtil
{
    /// <summary>
    /// <see cref="Math.PI"/> typecast to <see cref="float"/>
    /// </summary>
    public const float PI = (float)Math.PI;
    /// <summary>
    /// <see cref="PI"/> multiplied by 2.0
    /// </summary>
    public const float PI2 = PI * 2f;

    /// <summary>
    /// Converts an angle from radians to degrees
    /// </summary>
    /// <param name="rad">Angle expressed in radians</param>
    /// <returns><paramref name="rad"/> expressed in degrees</returns>
    public static float RadToDeg(float rad) => (rad / PI) * 180f;
    /// <summary>
    /// Converts an angle from degrees to radians
    /// </summary>
    /// <param name="deg">Angle expressed in degrees</param>
    /// <returns><paramref name="deg"/> expressed in radians</returns>
    public static float DegToRad(float deg) => (deg / 180f) * PI;

    /// <summary>
    /// Calculates a normalized <see cref="Vector2"/> with an angle of <paramref name="deg"/>
    /// </summary>
    /// <param name="deg">Angle expressed in degrees</param>
    public static Vector2 DegToVec(float deg) => RadToVec(DegToRad(deg));
    /// <summary>
    /// Calculates a normalized <see cref="Vector2"/> with an angle of <paramref name="rad"/>
    /// </summary>
    /// <param name="rad">Angle expressed in radians</param>
    public static Vector2 RadToVec(float rad) => new(
        x: (float)Math.Cos(rad),
        y: (float)Math.Sin(rad)
    );

    /// <summary>
    /// Wraps a value around a range
    /// </summary>
    /// <param name="val">Value to wrap</param>
    /// <param name="min">Inclusive minimum of the range</param>
    /// <param name="max">Exclusive maximum of the range</param>
    /// <returns><paramref name="val"/> wrapped around the range of <paramref name="min"/> and <paramref name="max"/></returns>
    /// <exception cref="ArgumentException"></exception>
    public static float WrapClamp(float val, float min, float max)
    {
        if (min == max) return min;
        if (min > max) throw new ArgumentException($"{nameof(min)} must not be greater than {nameof(max)}");

        float t = max - min;
        while (val < min) val += t;
        while (val >= max) val -= t;
        return val;
    }

    /// <summary>
    /// Calculates evenly distributed points around a circle centered on (0, 0)
    /// </summary>
    /// <param name="radius">Radius of the circle</param>
    /// <param name="sides">Number of points to calculate</param>
    /// <returns></returns>
	public static List<Vector2> GetCirclePoints(float radius, float sides)
	{
		List<Vector2> points = new();

		float step = PI2 / sides;

		for (float theta = 0f; theta < PI2; theta += step)
		{
			points.Add(RadToVec(theta) * radius);
		}

		return points;
	}
}

