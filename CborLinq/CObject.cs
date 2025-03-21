// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Cbor.Linq;

[DebuggerDisplay("{PrettyPrint}")]
public sealed class CObject : CContainer, IReadOnlyDictionary<string, CNode?>
{
    private readonly Dictionary<string, CNode?> map;

    internal CObject(Dictionary<string, CNode?> map) =>
        this.map = map;

    public override CNodeType TokenType =>
        CNodeType.Object;

    public IEnumerable<string> Keys =>
        this.map.Keys;

    public IEnumerable<CNode?> Values =>
        this.map.Values;

    public CNode? this[string key] =>
        this.map[key];

    public bool ContainsKey(string key) =>
        this.map.ContainsKey(key);

    public bool TryGetValue(string key, out CNode? value) =>
        this.map.TryGetValue(key, out value);

    public override async ValueTask WriteToAsync(
        CborWriter cw,
        CancellationToken ct = default)
    {
        cw.WriteStartMap(this.map.Count);
        foreach (var kv in this.map)
        {
            cw.WriteTextString(kv.Key);
            if (kv.Value != null)
            {
                await kv.Value.WriteToAsync(cw, ct);
            }
            else
            {
                cw.WriteNull();
            }
        }
        cw.WriteEndMap();
    }

    public override object ToObject() =>
        this.map;

    public override object ToObject(Type type)
    {
        if (type == typeof(IReadOnlyDictionary<string, CNode?>))
        {
            return this.map;
        }
        else if (type == typeof(IReadOnlyCollection<KeyValuePair<string, CNode?>>))
        {
            return this.map;
        }
        else if (type == typeof(IEnumerable<KeyValuePair<string, CNode?>>))
        {
            return this.map;
        }
        else
        {
            return new Dictionary<string, CNode?>(this.map);
        }
    }

    public override T ToObject<T>() =>
        (T)this.ToObject(typeof(T));
    
    IEnumerator<KeyValuePair<string, CNode?>> IEnumerable<KeyValuePair<string, CNode?>>.GetEnumerator() =>
        this.map.GetEnumerator();

    private protected override int InternalCount() =>
        this.map.Count;

    private protected override IEnumerator InternalGetEnumerator() =>
        this.map.GetEnumerator();

    private protected override void InternalCopyTo(Array array, int index) =>
        ((ICollection)this.map).CopyTo(array, index);

    private string PrettyPrint =>
        $"CObject: Members=[{string.Join(",", this.map.Keys)}]";

    public override string ToString() =>
        this.PrettyPrint;
}
