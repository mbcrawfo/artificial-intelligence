﻿using Microsoft.Xna.Framework;
using SFML.System;

namespace Game.Core.Extensions
{
  /// <summary>
  ///   Extensions to help with conversions between the vector classes.
  /// </summary>
  public static class VectorExtensions
  {
    /// <summary>
    ///   Converts <see cref="Vector2"/> to <see cref="Vector2f"/>.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    // ReSharper disable once InconsistentNaming
    public static Vector2f ToVector2f(this Vector2 v)
    {
      return new Vector2f(v.X, v.Y);
    }

    /// <summary>
    ///   Converts <see cref="Vector2f"/> to <see cref="Vector2"/>.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(this Vector2f v)
    {
      return new Vector2(v.X, v.Y);
    }

    /// <summary>
    ///   Returns a copy of the vector with the Y coordinate inverted to match 
    ///   SFML coordinate systems.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2f InvertY(this Vector2f v)
    {
      return new Vector2f(v.X, -v.Y);
    }

    /// <summary>
    ///   Returns a copy of the vector with the Y coordinate inverted to match 
    ///   SFML coordinate systems.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2 InvertY(this Vector2 v)
    {
      return new Vector2(v.X, -v.Y);
    }
  }
}
