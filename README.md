# CborLinq

Simple CBOR (Concise Binary Object Representation) serializer/deserializer, `System.Formats.Cbor` wrapper library.

[![Project Status: WIP – Initial development is in progress, but there has not yet been a stable, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)

## NuGet

| Package  | NuGet                                                                                                                |
|:---------|:---------------------------------------------------------------------------------------------------------------------|
| CborLinq | [![NuGet CborLinq](https://img.shields.io/nuget/v/CborLinq.svg?style=flat)](https://www.nuget.org/packages/CborLinq) |


----

## What is this?

CborLinq is a simple CBOR (Concise Binary Object Representation) serializer/deserializer.

This wraps `System.Formats.Cbor` and has an interface inspired by `NewtonSoft.Json.Linq`.


----

## Short sample code

Install the `CborLinq` NuGet package and:

```csharp
using System.Formats.Cbor.Linq;

// Deserialize CBOR binary file.
using var fs = new FileStream("sample.cbor", ...);

CNode? cn = await CNode.ReadFromAsync(fs);

// Root node is the object?
if (cn is CObject co)
{
    // Get property values.
    CNode? prop1 = co["prop1"];

    // Get the string value.
    Console.WriteLine($"prop1: {(prop1?.ToObject<string>() ?? "(null)")}");

    // Get the integer value.
    Console.WriteLine($"prop2: {co["prop2"]?.ToObject<int>()}");

    if (co["prop3"] is CArray arr)
    {
        for (var i = 0; i < arr.Count; i++)
        {
            Console.WriteLine($"item[{i}]: {arr[i]});
        }
    }
}
```


----

## Target environments

Widen .NET platforms supported are as follows:

* .NET 9 to 5 (`net9.0` and etc)
* .NET Core 3.1
* .NET Standard 2.1, 2.0
* .NET Framework 4.8.1, 4.8, 4.6.2, 4.6.1

(Tested only newest TFM.)


----

## How to use

All model types placed inside `System.Formats.Cbor.Linq` namespace.
The model types are:

|Model type|Description|`Newtonsoft.Json.Linq` like|
|:----|:----|:----|
|`CNode`|Abstraction type of the node object|`JToken`|
|`CValue`|Value node object|`JValue`|
|`CArray`|Array node object|`JArray`|
|`CObject`|Object node object|`JObject`|

CborLinq does not use null objects for `null` value.

When deserializing from CBOR data, null nodes become `null` value explicitly.
Therefore, you can use `CNullValue` (Null-object pattern object) instead of.
When serializing `CNullValue` value, they are output as null nodes.

### Deserialize and serialize

```csharp
using System.Formats.Cbor.Linq;

// Deserialize CBOR binary file.
using var fs1 = new FileStream("sample.cbor", ...);

// (If got null, replace with CNullValue to avoid NRE)
CNode cn = await CNode.ReadFromAsync(fs1) ?? CNullValue.Value;

// Serialize CBOR binary.
using var fs2 = new FileStream("dump.cbor", ...);

await cn.WriteToAsync(fs2);
await fs2.FlushAsync();
```

### Value node

```csharp
// Root node is a value?
if (cn is CValue cv)
{
    // Untyped value.
    object ov = cv.Value.ToObject();

    // Untyped value with conversion (when required).
    long dv = (long)cv.Value.ToObject(typeof(long));

    // Typed value with conversion (when required).
    double tv = cv.Value.ToObject<double>();
}
```

Supported value types are:

|Group|Types|
|:----|:----|
|Primitives|`bool`,`int`,`long`,`byte`,`sbyte`,`short`,`ushort`,`uint`,`ulong`,`float`,`double`,`decimal`,`string`|
|Byte array|Can convert bidirectional to the base64 formed string|
|Complex|`System.DateTimeOffset`,`System.DateTime`,`System.TimeSpan`,`System.Uri`|
|Guid|`System.Guid` - Can convert bidirectional to `byte[]`|

Each format conversion is in invariant culture.

### Array node

```csharp
// Root node is the array?
if (cn is CArray ca)
{
    // Get the length
    Console.WriteLine(ca.Count);

    // Get indexed value.
    CNode? item2 = ca[2];
    Console.WriteLine($"item2: {(item2?.ToObject<string>() ?? "(null)")}");

    // Iterate values.
    foreach (CNode? cct in ca)
    {
        Console.WriteLine(cct);
    }
}
```

Of course, when listing the elements, you can retrieve the nested nodes in a regular way by method recursion.

### Object node

```csharp
// Root node is the object?
if (cn is CObject co)
{
    // Get the property entries.
    Console.WriteLine(co.Count);

    // Get property values.
    CNode? prop1 = co["prop1"];
    Console.WriteLine($"prop1: {(prop1?.ToObject<string>() ?? "(null)")}");

    // Iterate properties.
    foreach (KeyValuePair<string, CNode?> entry in co)
    {
        Console.WriteLine($"Name={entry.Key}, {entry.Value}");
    }
}
```

### Null node

As already mentioned, a null node is `null` value itself.

```csharp
// Root node is null
// Note: CborLinq does not generate CNullValue on deserialization.
//       Therefore, it can only be used to simplify our code.
if (cn == null || cn is CNullValue)
{
    Console.WriteLine("Node is null.")
}
```

### Construct a value node

```csharp
// The creator.
CValue cvi = CNode.Create(123);
CValue cvd = CNode.Create("ABC");

// Implicitly conversion.
CValue ci = 123;
```

### Construct array node

```csharp
// Easy way with collection expression.
CArray ca = CNode.Create(
    [
        CNode.Create(123),
        CNode.Create(456.789),
        null,
        CNode.Create("ABC"),
        (CNode)Guid.NewGuid(),
    ]);
```

### Construct object node

Creating object nodes is slightly different from `Newtonsoft.Json.Linq`.
CborLinq can be created more directly using a tuple type:

```csharp
// Easy way with tuple type.
CObject co1 = CNode.Create(
    ("prop1", CNode.Create(123)),
    ("prop2", CNode.Create(456.789)),
    ("prop3", CNode.Create("ABC")));
```

Or, more steady style:

```csharp
// From dictionary.
Dictionary<string, CNode?> dict = new()
{
    ["prop1"] = CNode.Create(123),
    ["prop2"] = CNode.Create(456.789),
    ["prop3"] = CNode.Create("ABC"),
};
CObject co2 = new(dict);
```


----

## TODO

* Serialize and deserialize any class/struct/records.
* Implements own CBOR parser/writer and removed reference for `System.Formats.Cbor.`
  * It will make asynchronous operation perfectly.
* Attachable `Newtonsoft.Json.Linq.JToken` reader/writer (We can use CBOR with `JToken` directly).
* NPM package on JavaScript/TypeScript platforms.

## License

Apache-v2.

## History

* 0.1.0:
  * Initial release.

