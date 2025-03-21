// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Collections;

namespace System.Formats.Cbor.Linq;

public abstract class CContainer : CNode, ICollection
{
    public int Count =>
        this.InternalCount();

    private protected abstract int InternalCount();
    private protected abstract IEnumerator InternalGetEnumerator();
    private protected abstract void InternalCopyTo(Array array, int index);

    IEnumerator IEnumerable.GetEnumerator() =>
        this.InternalGetEnumerator();

    void ICollection.CopyTo(Array array, int index) =>
        this.InternalCopyTo(array, index);

    object ICollection.SyncRoot => this;
    bool ICollection.IsSynchronized => false;
}
