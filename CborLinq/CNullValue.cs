// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Cbor.Linq;

[EditorBrowsable(EditorBrowsableState.Advanced)]
[DebuggerDisplay("CNullValue")]
public sealed class CNullValue : CNode
{
    private CNullValue()
    {
    }

    public override CNodeType TokenType =>
        CNodeType.Null;

    public override object ToObject() =>
        null!;

    public override object ToObject(Type type) =>
        (!type.IsValueType || Nullable.GetUnderlyingType(type) != null) ?
            null! : throw new InvalidCastException();

    public override T ToObject<T>() =>
        (T)this.ToObject(typeof(T));

    public override ValueTask WriteToAsync(
        CborWriter cw,
        CancellationToken ct = default)
    {
        cw.WriteNull();
        return default;
    }

    public override string ToString() =>
        "CNullValue";

    public static readonly CNullValue Value = new();
}
