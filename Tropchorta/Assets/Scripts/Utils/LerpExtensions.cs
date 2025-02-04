using UnityEngine;
using System;
using Unity.Mathematics;

public static class LerpExtensions
{
    /// <summary>
    /// Frame-independent exponential interpolation for float.
    /// <para>a - current value (this)</para>
    /// <para>b - target value</para>
    /// <para>dt - delta time</para>
    /// <para>h - half-life of the lerp</para>
    /// Source: https://x.com/FreyaHolmer/status/1757836988495847568
    /// </summary>
    public static float LerpFI(this float a, float b, float dt, float h)
    {
        return b + (a - b) * Mathf.Pow(2, -dt / h);
    }

    /// <summary>
    /// Frame-independent exponential interpolation for Vector2.
    /// <para>a - current value (this)</para>
    /// <para>b - target value</para>
    /// <para>dt - delta time</para>
    /// <para>h - half-life of the lerp</para>
    /// Source: https://x.com/FreyaHolmer/status/1757836988495847568
    /// </summary>
    public static Vector2 LerpFI(this Vector2 a, Vector2 b, float dt, float h)
    {
        return b + (a - b) * Mathf.Pow(2, -dt / h);
    }

    /// <summary>
    /// Frame-independent exponential interpolation for Vector3.
    /// <para>a - current value (this)</para>
    /// <para>b - target value</para>
    /// <para>dt - delta time</para>
    /// <para>h - half-life of the lerp</para>
    /// Source: https://x.com/FreyaHolmer/status/1757836988495847568
    /// </summary>
    public static Vector3 LerpFI(this Vector3 a, Vector3 b, float dt, float h)
    {
        return b + (a - b) * Mathf.Pow(2, -dt / h);
    }

    /// <summary>
    /// Frame-independent exponential interpolation for Vector4.
    /// <para>a - current value (this)</para>
    /// <para>b - target value</para>
    /// <para>dt - delta time</para>
    /// <para>h - half-life of the lerp</para>
    /// Source: https://x.com/FreyaHolmer/status/1757836988495847568
    /// </summary>
    public static Vector4 LerpFI(this Vector4 a, Vector4 b, float dt, float h)
    {
        return b + (a - b) * Mathf.Pow(2, -dt / h);
    }
}
