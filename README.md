# CLSS.Types.ValueRange

### Problem

Structs can be used as dictionary keys. And a legitimate use case of structs as dictionary keys is to create branching code paths that can be changed at runtime as opposed to `switch-case` expression which necessarily requires hard-coding your code paths.

```csharp
// You can't change these branching conditions at runtime
switch (responseCode)
{
  case int n when (200 <= n && n < 300):
    Path1();
    break;
  case int n when (300 <= n && n < 400):
    Path2();
    break;
  case int n when (400 <= n && n < 500):
    Path3();
    break;
}

// You can change these branching conditions at runtime
public struct IntRange { public int Min; public int Max; }
Dictionary<IntRange, Action> ResponseCodePaths = new Dictionary<IntRange, Action>()
{
  [new IntRange { Min = 200, Max = 300 }] = Path1,
  [new IntRange { Min = 300, Max = 400 }] = Path2,
  [new IntRange { Min = 400, Max = 500 }] = Path3
};
ResponseCodePaths.First(p => p.Key.Min <= responseCode < p.Key.Max)();
// add custom response code range at runtime
ResponseCodePaths[new IntRange { Min = 600, Max = 620 }] = Path4;
```

The default equality comparison of structs - which would be used by dictionaries when comparing keys - is not very performant, as [.NET engineers themselves would readily tell you](https://devblogs.microsoft.com/premier-developer/performance-implications-of-default-struct-equality-in-c/). In order to mitigate this performance issue, the struct type must implement the [`IEquatable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1) interface.

The fairly new [`ValueTuple`](https://docs.microsoft.com/en-us/dotnet/api/system.valuetuple) type does this and also allows custom field names, but for the specific use case of value ranges, it is not type-safe. The types that `ValueTuple` accepts are not necessarily comparable types. `ValueTuple` is also not serializable - an attribute that's undoubtably valuable to Unity developers.

### Solution

`ValueRange` is a type-safe, serializable generic struct type tailored to semantically represent a range of comparable values. The type of its `Min` and `Max` fields must satisfy an [`IComparable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1) constraint. If the constraint is not met, you will get a compilation error, ensuring that you will never get a runtime error if you use an uncomparable type in the type parameter.

`ValueRange` implements `IEquatable<T>` and therefore it's ready to be used as dictionary keys.

```csharp
using System;

Dictionary<ValueRange<Version>, Action> VersionMigrationPaths = new Dictionary<ValueRange<Version>, Action>()
{
  [new ValueRange<Version>(Version.Parse("1.0"), Version.Parse("1.4"))] = Path1,
  [new ValueRange<Version>(Version.Parse("1.4"), Version.Parse("1.6"))] = Path2,
  [new ValueRange<Version>(Version.Parse("1.6"), Version.Parse("2.0"))] = Path3
};
```

Since version 1.1, this package also includes the `Encapsulate` extension method to grow a `ValueRange` to include more values. It can take in a `params` argument list of `IComparable<T>`s or other `ValueRange`s.

```csharp
using CLSS;

var numbers = new int[] { 6, -11, -2, 4, 9 };
var numbersRange = new ValueRange<int>(numbers[0], numbers[0]);
numbersRange = numbersRange.Encapsulate(numbers); // Min: -11, Max: 9
var anotherRange1 = new ValueRange<int>(0, 16);
var anotherRange2 = new ValueRange<int>(-12, 0);
numbersRange = numbersRange.Encapsulate(anotherRange1, anotherRange2); // Min: -12, Max: 16
```

##### This package is a part of the [C# Language Syntactic Sugar suite](https://github.com/tonygiang/CLSS).