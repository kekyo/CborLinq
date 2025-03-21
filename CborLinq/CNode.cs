// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Cbor.Linq;

public abstract class CNode
{
    public abstract CNodeType TokenType { get; }

    public abstract object ToObject();

    public abstract object ToObject(Type type);

    public abstract T ToObject<T>();

    /////////////////////////////////////////////////////////////////////////

    public abstract ValueTask WriteToAsync(
        CborWriter cw,
        CancellationToken ct = default);

    public ValueTask WriteToAsync(
        Stream stream,
        CancellationToken ct = default)
    {
        var cw = new CborWriter();
        this.WriteToAsync(cw, ct);
        var data = cw.Encode();
        return new(stream.WriteAsync(data, 0, data.Length, ct));
    }

    /////////////////////////////////////////////////////////////////////////

    [return: NotNullIfNotNull(nameof(arr))]
    public static CArray? Create(IReadOnlyList<CNode?>? arr) =>
        arr is not null ? new(arr) : null;

    /////////////////////////////////////////////////////////////////////////

    [return: NotNullIfNotNull(nameof(map))]
    public static CObject? Create(IReadOnlyDictionary<string, CNode?>? map) =>
        map is not null ? new(map.ToDictionary(kv => kv.Key, kv => kv.Value)) : null;

    [return: NotNullIfNotNull(nameof(map))]
    public static CObject? Create(IReadOnlyDictionary<string, object?>? map) =>
        map is not null ? new(map.ToDictionary(kv => kv.Key, kv => Create(kv.Value))) : null;

    [return: NotNullIfNotNull(nameof(tokens))]
    public static CObject? Create(IEnumerable<KeyValuePair<string, CNode?>>? tokens) =>
        tokens is not null ? new(tokens.ToDictionary(kv => kv.Key, kv => kv.Value)) : null;

    [return: NotNullIfNotNull(nameof(tokens))]
    public static CObject? Create(IEnumerable<KeyValuePair<string, object?>>? tokens) =>
        tokens is not null ? new(tokens.ToDictionary(kv => kv.Key, kv => Create(kv.Value))) : null;

    [return: NotNullIfNotNull(nameof(tokens))]
    public static CObject? Create(IEnumerable<(string key, CNode? value)>? tokens) =>
        tokens is not null ? new(tokens.ToDictionary(kv => kv.key, kv => kv.value)) : null;

    [return: NotNullIfNotNull(nameof(tokens))]
    public static CObject? Create(IEnumerable<(string key, object? value)>? tokens) =>
        tokens is not null ? new(tokens.ToDictionary(kv => kv.key, kv => Create(kv.value))) : null;

    public static CObject Create(params KeyValuePair<string, CNode?>[] tokens) =>
        new(tokens.ToDictionary(kv => kv.Key, kv => kv.Value));

    public static CObject Create(params (string key, CNode? value)[] tokens) =>
        new(tokens.ToDictionary(kv => kv.key, kv => kv.value));
    
    /////////////////////////////////////////////////////////////////////////

    public static CValue Create(bool value) =>
        new(value);

    public static CValue Create(int value) =>
        new(value);

    public static CValue Create(long value) =>
        new(value);

    public static CValue Create(short value) =>
        new(value);

    public static CValue Create(byte value) =>
        new(value);

    public static CValue Create(sbyte value) =>
        new(value);

    public static CValue Create(ushort value) =>
        new(value);

    public static CValue Create(uint value) =>
        new(value);

    public static CValue Create(ulong value) =>
        new(value);

    public static CValue Create(double value) =>
        new(value);

    public static CValue Create(float value) =>
        new(value);

    public static CValue Create(decimal value) =>
        new(value);

    public static CValue Create(DateTimeOffset value) =>
        new(value);

    public static CValue Create(DateTime value) =>
        new(value);

    public static CValue Create(TimeSpan value) =>
        new(value);

    public static CValue Create(Guid value) =>
        new(value);

    [return: NotNullIfNotNull(nameof(value))]
    public static CValue? Create(Uri? value) =>
        value is { } v ? new(v) : null;

    [return: NotNullIfNotNull(nameof(value))]
    public static CValue? Create(string? value) =>
        value is { } v ? new(v) : null;

    [return: NotNullIfNotNull(nameof(value))]
    public static CValue? Create(byte[]? value) =>
        value is { } v ? new(v) : null;

    /////////////////////////////////////////////////////////////////////////

    public static CValue? Create(bool? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(int? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(long? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(short? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(byte? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(sbyte? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(ushort? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(uint? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(ulong? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(double? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(float? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(decimal? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(DateTimeOffset? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(DateTime? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(Guid? value) =>
        value is { } v ? new(v) : null;

    public static CValue? Create(TimeSpan? value) =>
        value is { } v ? new(v) : null;
    
    /////////////////////////////////////////////////////////////////////////

    [return: NotNullIfNotNull(nameof(value))]
    public static CNode? Create(object? value) =>
        value switch
        {
            null => null,
            bool v => new CValue(v),
            int v => new CValue(v),
            long v => new CValue(v),
            double v => new CValue(v),
            string v => new CValue(v),
            byte[] v => new CValue(v),
            DateTimeOffset v => new CValue(v),
            Guid v => new CValue(v),
            byte v => new CValue(v),
            float v => new CValue(v),
            decimal v => new CValue(v),
            short v => new CValue(v),
            sbyte v => new CValue(v),
            ushort v => new CValue(v),
            uint v => new CValue(v),
            ulong v => new CValue(v),
            DateTime v => new CValue(v),
            TimeSpan v => new CValue(v),
            Uri v => new CValue(v),
            CNode v => v,
            _ => throw new InvalidCastException(),
        };

    /////////////////////////////////////////////////////////////////////////

    public static implicit operator CNode(bool value) =>
        new CValue(value);

    public static implicit operator CNode(int value) =>
        new CValue(value);

    public static implicit operator CNode(long value) =>
        new CValue(value);

    public static implicit operator CNode(byte value) =>
        new CValue(value);

    public static implicit operator CNode(short value) =>
        new CValue(value);

    public static implicit operator CNode(sbyte value) =>
        new CValue(value);

    public static implicit operator CNode(ushort value) =>
        new CValue(value);

    public static implicit operator CNode(uint value) =>
        new CValue(value);

    public static implicit operator CNode(ulong value) =>
        new CValue(value);

    public static implicit operator CNode(double value) =>
        new CValue(value);

    public static implicit operator CNode(float value) =>
        new CValue(value);

    public static implicit operator CNode(decimal value) =>
        new CValue(value);

    public static implicit operator CNode(DateTime value) =>
        new CValue(value);

    public static implicit operator CNode(DateTimeOffset value) =>
        new CValue(value);

    public static implicit operator CNode(TimeSpan value) =>
        new CValue(value);

    public static implicit operator CNode(Guid value) =>
        new CValue(value);

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CNode?(Uri? value) =>
        value != null ? new CValue(value) : null;

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CNode?(string? value) =>
        value != null ? new CValue(value) : null;

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CNode?(byte[]? value) =>
        value != null ? new CValue(value) : null;

    /////////////////////////////////////////////////////////////////////////

    public static implicit operator CNode?(bool? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(int? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(long? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(byte? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(short? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(sbyte? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(ushort? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(uint? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(ulong? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(double? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(float? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(decimal? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(DateTime? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(DateTimeOffset? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(TimeSpan? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CNode?(Guid? value) =>
        value.HasValue ? new CValue(value) : null;

    /////////////////////////////////////////////////////////////////////////

    private static async ValueTask<CNode?> InternalParseAsync(
        CborReader cr,
        CancellationToken ct)
    {
        switch (cr.PeekState())
        {
            case CborReaderState.Null:
                cr.ReadNull();
                return null;
            case CborReaderState.Boolean:
                return new CValue(cr.ReadBoolean());
            case CborReaderState.NegativeInteger:
                return new CValue(cr.ReadInt32());
            case CborReaderState.UnsignedInteger:
                return new CValue(cr.ReadUInt32());
            case CborReaderState.SinglePrecisionFloat:
                return new CValue(cr.ReadSingle());
            case CborReaderState.DoublePrecisionFloat:
                return new CValue(cr.ReadDouble());
            case CborReaderState.TextString:
                return new CValue(cr.ReadTextString());
            case CborReaderState.ByteString:
                return new CValue(cr.ReadByteString());
            case CborReaderState.StartArray:
                if (cr.ReadStartArray() is { } arrayLength)
                {
                    var arr = new CNode?[arrayLength];
                    for (var i = 0; i < arr.Length; i++)
                    {
                        arr[i] = await InternalParseAsync(cr, ct);
                    }
                    cr.ReadEndArray();
                    return new CArray(arr);
                }
                else
                {
                    var list = new List<CNode?>();
                    do
                    {
                        list.Add(await InternalParseAsync(cr, ct));
                    }
                    while (cr.PeekState() != CborReaderState.EndArray);
                    cr.ReadEndArray();
                    return new CArray(list.ToArray());
                }
            case CborReaderState.StartMap:
                if (cr.ReadStartMap() is { } mapLength)
                {
                    var map = new Dictionary<string, CNode?>(mapLength);
                    for (var i = 0; i < mapLength; i++)
                    {
                        var key = cr.ReadTextString();
                        var value = await InternalParseAsync(cr, ct);
                        map.Add(key, value);
                    }
                    cr.ReadEndMap();
                    return new CObject(map);
                }
                else
                {
                    var map = new Dictionary<string, CNode?>();
                    do
                    {
                        var key = cr.ReadTextString();
                        var value = await InternalParseAsync(cr, ct);
                        map.Add(key, value);
                    }
                    while (cr.PeekState() != CborReaderState.EndMap);
                    cr.ReadEndMap();
                    return new CObject(map);
                }
            case var state:
                throw new FormatException(
                    $"Found unknown state: State={state}, Depth={cr.CurrentDepth}");
        }
    }

    public static ValueTask<CNode?> ReadFromAsync(
        CborReader cr,
        CancellationToken ct = default) =>
        InternalParseAsync(cr, ct);

    public static async ValueTask<CNode?> ReadFromAsync(
        Stream stream,
        CborConformanceMode conformanceMode = CborConformanceMode.Lax,
        CancellationToken ct = default)
    {
        var ms = new MemoryStream();
        await stream.CopyToAsync(ms, 65536, ct);   // TODO: Dirty

        var cr = new CborReader(ms.ToArray(), conformanceMode);
        return await InternalParseAsync(cr, ct);
    }
}
