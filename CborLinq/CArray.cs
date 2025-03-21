// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Cbor.Linq;

[DebuggerDisplay("{PrettyPrint}")]
public sealed class CArray : CContainer, IReadOnlyList<CNode?>
{
    private readonly IReadOnlyList<CNode?> list;

    public CArray(IReadOnlyList<CNode?> list) =>
        this.list = list;

    public override CNodeType TokenType =>
        CNodeType.Array;

    public CNode? this[int index] =>
        this.list[index];

    public override async ValueTask WriteToAsync(
        CborWriter cw,
        CancellationToken ct = default)
    {
        cw.WriteStartArray(this.list.Count);
        foreach (var child in this.list)
        {
            if (child != null)
            {
                await child.WriteToAsync(cw, ct);
            }
            else
            {
                cw.WriteNull();
            }
        }
        cw.WriteEndArray();
    }

    public override object ToObject() =>
        this.list;

    public override object ToObject(Type type)
    {
        if (type == typeof(IReadOnlyList<CNode?>))
        {
            return this.list;
        }
        else if (type == typeof(IReadOnlyCollection<CNode?>))
        {
            return this.list;
        }
        else if (type == typeof(IEnumerable<CNode?>))
        {
            return this.list;
        }
        else
        {
            return this.list.ToArray();
        }
    }

    public override T ToObject<T>() =>
        (T)this.ToObject(typeof(T));

    public IEnumerator<CNode?> GetEnumerator() =>
        this.list.GetEnumerator();

    private protected override int InternalCount() =>
        this.list.Count;

    private protected override IEnumerator InternalGetEnumerator() =>
        this.list.GetEnumerator();

    private protected override void InternalCopyTo(Array array, int index) =>
        ((ICollection)this.list).CopyTo(array, index);

    private string PrettyPrint =>
        $"CArray: Count={this.Count}";

    public override string ToString() =>
        this.PrettyPrint;
}
