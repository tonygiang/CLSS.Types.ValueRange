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
}
