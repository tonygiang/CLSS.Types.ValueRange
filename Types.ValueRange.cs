// A part of the C# Language Syntactic Sugar suite.

using System;

namespace CLSS
{
  /// <summary>
  /// A serializable and equatable pair of 2 values semantically representing a
  /// range of values.
  /// </summary>
  /// <typeparam name="T">The type of comparable values represented by
  /// <see cref="ValueRange{T}"/>.</typeparam>
  [Serializable]
  public struct ValueRange<T> : IEquatable<ValueRange<T>>
    where T : IComparable<T>
  {
    /// <summary>
    /// The lower bound of the <see cref="ValueRange{T}"/>.
    /// </summary>
    public T Min;
    /// <summary>
    /// The upper bound of the <see cref="ValueRange{T}"/>.
    /// </summary>
    public T Max;

    /// <summary>
    /// Initializes a new instance of <see cref="ValueRange{T}"/> with its
    /// Min and Max fields initialized.
    /// </summary>
    /// <param name="min">The initial value of the Min field.</param>
    /// <param name="max">The initial value of the Max field.</param>
    public ValueRange(T min, T max) { Min = min; Max = max; }

    public bool Equals(ValueRange<T> other)
    {
      return this.Min.CompareTo(other.Min) == 0
        && this.Max.CompareTo(other.Max) == 0;
    }
  }

  public static partial class ValueRangeExtension
  {
    /// <summary>
    /// Returns a <see cref="ValueRange{T}"/> that contains the specified values
    /// and the source <see cref="ValueRange{T}"/>'s Min and Max values.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="ValueRange{T}"/></typeparam>
    /// <param name="source">The source <see cref="ValueRange{T}"/></param>
    /// <param name="values">The value that will be encapsulated </param>
    /// <returns>A <see cref="ValueRange{T}"/> that contains the specified
    /// values and the source <see cref="ValueRange{T}"/>'s Min and Max values.
    /// </returns>
    public static ValueRange<T> Encapsulate<T>(this ValueRange<T> source,
      params T[] values) where T : IComparable<T>
    {
      foreach (var v in values)
      {
        if (v.CompareTo(source.Min) < 0) source.Min = v;
        if (v.CompareTo(source.Max) > 0) source.Max = v;
      }
      return source;
    }

    /// <summary>
    /// Returns a <see cref="ValueRange{T}"/> that contains the Min and Max
    /// values of the specified ranges and the source
    /// <see cref="ValueRange{T}"/>.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="ValueRange{T}"/></typeparam>
    /// <param name="source">
    /// <inheritdoc cref="Encapsulate{T}(ValueRange{T}, T[])"/></param>
    /// <param name="ranges">The ranges that will be encapsulated.</param>
    /// <returns>A <see cref="ValueRange{T}"/> that contains the Min and Max
    /// values of the specified ranges and the source
    /// <see cref="ValueRange{T}"/>.</returns>
    public static ValueRange<T> Encapsulate<T>(this ValueRange<T> source,
      params ValueRange<T>[] ranges) where T : IComparable<T>
    {
      foreach (var r in ranges) source = source.Encapsulate(r.Min, r.Max);
      return source;
    }
  }
}
