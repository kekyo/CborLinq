// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

namespace System.Formats.Cbor.Linq;

public enum CNodeType
{
    /// <summary>
    /// No token type has been set.
    /// </summary>
    None = 0,

    /// <summary>
    /// A JSON object.
    /// </summary>
    Object = 1,

    /// <summary>
    /// A JSON array.
    /// </summary>
    Array = 2,

    /// <summary>
    /// An integer value.
    /// </summary>
    Integer = 3,

    /// <summary>
    /// A float value.
    /// </summary>
    Float = 4,

    /// <summary>
    /// A string value.
    /// </summary>
    String = 5,

    /// <summary>
    /// A boolean value.
    /// </summary>
    Boolean = 6,

    /// <summary>
    /// A null value.
    /// </summary>
    Null = 7,

    /// <summary>
    /// A collection of bytes value.
    /// </summary>
    Bytes = 8,
}
