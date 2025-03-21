// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Cbor.Linq;

[DebuggerDisplay("{PrettyPrint}")]
public sealed class CValue : CNode
{
    public static readonly string DateTimeOffsetFormatString = "yyyy-MM-ddTHH:mm:ss.FFFFFFFK";

    private readonly object value;

    internal CValue(object value) =>
        this.value = value;

    public override CNodeType TokenType =>
        this.value switch
        {
            bool => CNodeType.Boolean,
            int => CNodeType.Integer,
            long => CNodeType.Integer,
            byte => CNodeType.Integer,
            short => CNodeType.Integer,
            sbyte => CNodeType.Integer,
            ushort => CNodeType.Integer,
            uint => CNodeType.Integer,
            ulong => CNodeType.Integer,
            double => CNodeType.Float,
            float => CNodeType.Float,
            string => CNodeType.String,
            decimal => CNodeType.String,
            DateTimeOffset => CNodeType.String,
            DateTime => CNodeType.String,
            TimeSpan => CNodeType.String,
            Guid => CNodeType.String,
            Uri => CNodeType.String,
            byte[] => CNodeType.Bytes,
            _ => CNodeType.Integer
        };

    public override ValueTask WriteToAsync(
        CborWriter cw,
        CancellationToken ct = default)
    {
        switch (this.value)
        {
            case bool bv:
                cw.WriteBoolean(bv);
                break;
            case int i32v:
                cw.WriteInt32(i32v);
                break;
            case long i64v:
                cw.WriteInt64(i64v);
                break;
            case double dblv:
                cw.WriteDouble(dblv);
                break;
            case string sv:
                cw.WriteTextString(sv);
                break;
            case byte byv:
                cw.WriteInt32(byv);
                break;
            case sbyte sbyv:
                cw.WriteInt32(sbyv);
                break;
            case short i16v:
                cw.WriteInt32(i16v);
                break;
            case ushort ui16v:
                cw.WriteInt32(ui16v);
                break;
            case uint ui32v:
                cw.WriteUInt32(ui32v);
                break;
            case ulong ui64v:
                cw.WriteUInt64(ui64v);
                break;
            case float fv:
                cw.WriteSingle(fv);
                break;
            case decimal decv:
                cw.WriteTextString(decv.ToString(CultureInfo.InvariantCulture));
                break;
            case DateTimeOffset dto:
                cw.WriteTextString(dto.ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture));
                break;
            case DateTime dt:
                cw.WriteTextString(dt.ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture));
                break;
            case TimeSpan ts:
                cw.WriteTextString(ts.ToString());
                break;
            case Guid g:
                cw.WriteTextString(g.ToString());
                break;
            case Uri u:
                cw.WriteTextString(u.ToString());
                break;
            case byte[] bysv:
                cw.WriteByteString(bysv);
                break;
        }
        return default;
    }

    private static DateTimeOffset ToDateTimeOffset(DateTime dt) =>
        dt.Kind != DateTimeKind.Unspecified ?
            new DateTimeOffset(dt) :
            new DateTimeOffset(dt, TimeZoneInfo.Local.GetUtcOffset(dt));

    private static object InternalToObject(object value, Type type) =>
        value switch
        {
            string v =>
                (type == typeof(byte[])) ? Convert.FromBase64String(v) :
                (type == typeof(DateTimeOffset)) ? DateTimeOffset.Parse(v, CultureInfo.InvariantCulture) :
                (type == typeof(DateTime)) ? DateTimeOffset.Parse(v, CultureInfo.InvariantCulture).LocalDateTime :
                (type == typeof(TimeSpan)) ? TimeSpan.Parse(v, CultureInfo.InvariantCulture) :
                (type == typeof(Guid)) ? Guid.Parse(v) :
                (type == typeof(Uri)) ? new Uri(v, UriKind.RelativeOrAbsolute) :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            byte[] v =>
                (type == typeof(string)) ? Convert.ToBase64String(v) :
                (type == typeof(Guid)) ? new Guid(v) :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            DateTimeOffset v =>
                (type == typeof(string)) ? v.ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture) :
                (type == typeof(DateTime)) ? v.LocalDateTime :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            DateTime v =>
                (type == typeof(string)) ? ToDateTimeOffset(v).ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture) :
                (type == typeof(DateTimeOffset)) ? ToDateTimeOffset(v) :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            TimeSpan v =>
                (type == typeof(string)) ? v.ToString() :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            Guid v =>
                (type == typeof(string)) ? v.ToString() :
                (type == typeof(byte[])) ? v.ToByteArray() :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            Uri v =>
                (type == typeof(string)) ? v.ToString() :
                Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
            var v => Convert.ChangeType(v, type, CultureInfo.InvariantCulture),
        };

    public override object ToObject() =>
        this.value;

    public override object ToObject(Type type) =>
        this.value.GetType() == type ?
            this.value :
            InternalToObject(this.value, type);

    public override T ToObject<T>() =>
        this.value is T tv ?
            tv :
            (T)InternalToObject(this.value, typeof(T));

    private static string ToByteArrayString(byte[] value) =>
        value.Length > 8 ?
            $"{BitConverter.ToString(value, 0, 8).Replace("-", string.Empty).ToLowerInvariant()}..." :
            BitConverter.ToString(value).Replace("-", string.Empty).ToLowerInvariant();
    
    private string PrettyPrint =>
        this.value switch
        {
            bool v => $"CValue: {v}",
            int v => $"CValue: {v} [int]",
            long v => $"CValue: {v} [long]",
            double v => $"CValue: {v} [double]",
            byte v => $"CValue: {v} [byte]",
            short v => $"CValue: {v} [short]",
            sbyte v => $"CValue: {v} [sbyte]",
            ushort v => $"CValue: {v} [ushort]",
            uint v => $"CValue: {v} [uint]",
            ulong v => $"CValue: {v} [ulong]",
            float v => $"CValue: {v} [float]",
            decimal v => $"CValue: {v} [decimal]",
            string v => $"CValue: \"{v}\"",
            DateTimeOffset v => $"CValue: {v.ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture)} [DateTimeOffset]",
            DateTime v => $"CValue: {ToDateTimeOffset(v).ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture)} [DateTime]",
            TimeSpan v => $"CValue: {v.ToString()} [TimeSpan]",
            Guid v => $"CValue: {v} [Guid]",
            Uri v => $"CValue: {v} [Uri]",
            byte[] v => $"CValue: {ToByteArrayString(v)} [byte[{v.Length}]]",
            _ => $"CValue: {InternalToObject(this.value, typeof(string))} [{this.value.GetType().FullName}]",
        };

    public override string ToString() =>
        this.PrettyPrint;
    
    /////////////////////////////////////////////////////////////////////////

    public static implicit operator CValue(bool value) =>
        new CValue(value);

    public static implicit operator CValue(int value) =>
        new CValue(value);

    public static implicit operator CValue(long value) =>
        new CValue(value);

    public static implicit operator CValue(byte value) =>
        new CValue(value);

    public static implicit operator CValue(short value) =>
        new CValue(value);

    public static implicit operator CValue(sbyte value) =>
        new CValue(value);

    public static implicit operator CValue(ushort value) =>
        new CValue(value);

    public static implicit operator CValue(uint value) =>
        new CValue(value);

    public static implicit operator CValue(ulong value) =>
        new CValue(value);

    public static implicit operator CValue(double value) =>
        new CValue(value);

    public static implicit operator CValue(float value) =>
        new CValue(value);

    public static implicit operator CValue(decimal value) =>
        new CValue(value);

    public static implicit operator CValue(DateTime value) =>
        new CValue(value);

    public static implicit operator CValue(DateTimeOffset value) =>
        new CValue(value);

    public static implicit operator CValue(TimeSpan value) =>
        new CValue(value);

    public static implicit operator CValue(Guid value) =>
        new CValue(value);

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CValue?(Uri? value) =>
        value != null ? new CValue(value) : null;

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CValue?(string? value) =>
        value != null ? new CValue(value) : null;

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CValue?(byte[]? value) =>
        value != null ? new CValue(value) : null;

    /////////////////////////////////////////////////////////////////////////

    public static implicit operator CValue?(bool? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(int? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(long? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(byte? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(short? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(sbyte? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(ushort? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(uint? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(ulong? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(double? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(float? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(decimal? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(DateTime? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(DateTimeOffset? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(TimeSpan? value) =>
        value.HasValue ? new CValue(value) : null;

    public static implicit operator CValue?(Guid? value) =>
        value.HasValue ? new CValue(value) : null;
}
