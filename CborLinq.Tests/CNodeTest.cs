// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace System.Formats.Cbor.Linq;

public sealed class CNodeTest
{
    private static readonly object?[][] ValueSource =
    [
        [null, null],
        [true, CNodeType.Boolean],
        [false, CNodeType.Boolean],
        [123, CNodeType.Integer],
        [123L, CNodeType.Integer],
        [(short)123, CNodeType.Integer],
        [(ushort)123, CNodeType.Integer],
        [(byte)123, CNodeType.Integer],
        [(sbyte)123, CNodeType.Integer],
        [123UL, CNodeType.Integer],
        [123.456, CNodeType.Float],
        [123.456f, CNodeType.Float],
        ["ABC", CNodeType.String],
        [DateTimeOffset.Now, CNodeType.String],
        [DateTime.Now, CNodeType.String],
        [Guid.NewGuid(), CNodeType.String],
        [DateTime.Now - DateTime.UnixEpoch, CNodeType.String],
        [new Uri("https://example.com"), CNodeType.String],
        [Guid.NewGuid().ToByteArray(), CNodeType.Bytes],
        [123.456m, CNodeType.String],
    ];

    [TestCaseSource(nameof(ValueSource))]
    public void CreateFromObject(object? value, CNodeType? type)
    {
        var cn = CNode.Create(value);

        Assert.That(value, Is.EqualTo(cn?.ToObject()));
        Assert.That(type, Is.EqualTo(cn?.TokenType));
    }
    
    /////////////////////////////////////////////////////////////////////////

    [TestCase(true)]
    [TestCase(false)]
    public void CreateFromBoolean(bool value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123)]
    public void CreateFromInt32(int value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123L)]
    public void CreateFromInt64(long value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase((short)123)]
    public void CreateFromInt16(short value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase((byte)123)]
    public void CreateFromByte(byte value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase((sbyte)123)]
    public void CreateFromSByte(sbyte value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase((ushort)123)]
    public void CreateFromUInt16(ushort value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase((uint)123)]
    public void CreateFromUInt32(uint value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase((ulong)123)]
    public void CreateFromUInt64(ulong value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123.456)]
    public void CreateFromDouble(double value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123.456f)]
    public void CreateFromSingle(float value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly decimal[] DecimalSource =
        [123.456m];

    [TestCaseSource(nameof(DecimalSource))]
    public void CreateFromDecimal(decimal value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    [TestCase("ABC")]
    public void CreateFromString(string value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly DateTimeOffset[] DateTimeOffsetSource =
        [DateTimeOffset.Now];
    
    [TestCaseSource(nameof(DateTimeOffsetSource))]
    public void CreateFromDateTimeOffset(DateTimeOffset value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly DateTime[] DateTimeSource =
        [DateTime.Now];
    
    [TestCaseSource(nameof(DateTimeSource))]
    public void CreateFromDateTime(DateTime value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly TimeSpan[] TimeSpanSource =
        [DateTime.Now - DateTime.UnixEpoch];
    
    [TestCaseSource(nameof(TimeSpanSource))]
    public void CreateFromTimeSpan(TimeSpan value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly Guid[] GuidSource =
        [Guid.NewGuid()];
    
    [TestCaseSource(nameof(GuidSource))]
    public void CreateFromGuid(Guid value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly Uri[] UriSource =
        [new Uri("https://example.com")];
    
    [TestCaseSource(nameof(UriSource))]
    public void CreateFromUri(Uri value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }

    private static readonly byte[][] BytesSource =
        [Guid.NewGuid().ToByteArray()];
    
    [TestCaseSource(nameof(BytesSource))]
    public void CreateFromBytes(byte[] value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn.ToObject(), Is.EqualTo(value));
    }
        
    /////////////////////////////////////////////////////////////////////////

    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public void CreateFromNullableBoolean(bool? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123)]
    [TestCase(null)]
    public void CreateFromNullableInt32(int? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123L)]
    [TestCase(null)]
    public void CreateFromNullableInt64(long? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase((short)123)]
    [TestCase(null)]
    public void CreateFromNullableInt16(short? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase((byte)123)]
    [TestCase(null)]
    public void CreateFromNullableByte(byte? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase((sbyte)123)]
    [TestCase(null)]
    public void CreateFromNullableSByte(sbyte? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase((ushort)123)]
    [TestCase(null)]
    public void CreateFromNullableUInt16(ushort? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase((uint)123)]
    [TestCase(null)]
    public void CreateFromNullableUInt32(uint? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase((ulong)123)]
    [TestCase(null)]
    public void CreateFromNullableUInt64(ulong? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123.456)]
    [TestCase(null)]
    public void CreateFromNullableDouble(double? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase(123.456f)]
    [TestCase(null)]
    public void CreateFromNullableSingle(float? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly decimal?[] NullableDecimalSource =
        [123.456m, null];

    [TestCaseSource(nameof(NullableDecimalSource))]
    public void CreateFromNullableDecimal(decimal? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    [TestCase("ABC")]
    [TestCase(null)]
    public void CreateFromNullableString(string? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly DateTimeOffset?[] NullableDateTimeOffsetSource =
        [DateTimeOffset.Now, null];
    
    [TestCaseSource(nameof(NullableDateTimeOffsetSource))]
    public void CreateFromNullableDateTimeOffset(DateTimeOffset? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly DateTime?[] NullableDateTimeSource =
        [DateTime.Now, null];
    
    [TestCaseSource(nameof(NullableDateTimeSource))]
    public void CreateFromNullableDateTime(DateTime? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly TimeSpan?[] NullableTimeSpanSource =
        [DateTime.Now - DateTime.UnixEpoch, null];
    
    [TestCaseSource(nameof(NullableTimeSpanSource))]
    public void CreateFromNullableTimeSpan(TimeSpan? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly Guid?[] NullableGuidSource =
        [Guid.NewGuid(), null];
    
    [TestCaseSource(nameof(NullableGuidSource))]
    public void CreateFromNullableGuid(Guid? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly Uri?[] NullableUriSource =
        [new Uri("https://example.com"), null];
    
    [TestCaseSource(nameof(NullableUriSource))]
    public void CreateFromNullableUri(Uri? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    private static readonly byte[]?[] NullableBytesSource =
        [Guid.NewGuid().ToByteArray(), null];
    
    [TestCaseSource(nameof(NullableBytesSource))]
    public void CreateFromNullableBytes(byte[]? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject(), Is.EqualTo(value));
    }

    /////////////////////////////////////////////////////////////////////////

    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public void ToBoolean(bool? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<bool>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123)]
    [TestCase(null)]
    public void ToInt32(int? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<int>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123L)]
    [TestCase(null)]
    public void ToInt64(long? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<int>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((short)123)]
    [TestCase(null)]
    public void ToInt16(short? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<short>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((byte)123)]
    [TestCase(null)]
    public void ToByte(byte? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<byte>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((uint)123)]
    [TestCase(null)]
    public void ToUInt32(uint? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<uint>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123UL)]
    [TestCase(null)]
    public void ToUInt64(ulong? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<ulong>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ushort)123)]
    [TestCase(null)]
    public void ToUInt16(ushort? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<ushort>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((sbyte)123)]
    [TestCase(null)]
    public void ToSByte(sbyte? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<sbyte>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456)]
    [TestCase(null)]
    public void ToDouble(double? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<double>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456f)]
    [TestCase(null)]
    public void ToDouble(float? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<float>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCaseSource(nameof(DecimalSource))]
    public void ToDecimal(decimal? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<decimal>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCaseSource(nameof(DateTimeOffsetSource))]
    public void ToDateTimeOffset(DateTimeOffset? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<DateTimeOffset>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    [TestCaseSource(nameof(DateTimeSource))]
    public void ToDateTime(DateTime? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<DateTime>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    [TestCaseSource(nameof(TimeSpanSource))]
    public void ToTimeSpan(TimeSpan? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<TimeSpan>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    [TestCaseSource(nameof(GuidSource))]
    public void ToGuid(Guid? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<Guid>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    [TestCaseSource(nameof(UriSource))]
    public void ToUri(Uri? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<Uri>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    [TestCaseSource(nameof(BytesSource))]
    public void ToBytes(byte[]? value)
    {
        var cn = CNode.Create(value);

        Assert.That(cn?.ToObject<byte[]>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value != null ? Convert.ToBase64String(value) : null));
    }
    
    /////////////////////////////////////////////////////////////////////////

    [TestCase(true)]
    [TestCase(false)]
    public void BooleanReceiver(bool value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<bool>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123)]
    public void Int32Receiver(int value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<int>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123L)]
    public void Int64Receiver(long value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<long>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((byte)123)]
    public void ByteReceiver(byte value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<byte>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((short)123)]
    public void Int16Receiver(short value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<short>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((sbyte)123)]
    public void SByteReceiver(sbyte value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<sbyte>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ushort)123)]
    public void UInt16Receiver(ushort value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<ushort>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((uint)123)]
    public void UInt32Receiver(uint value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<uint>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ulong)123)]
    public void UInt64Receiver(ulong value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<ulong>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456)]
    public void DoubleReceiver(double value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<double>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456f)]
    public void SingleReceiver(float value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<float>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly decimal[] DecimalReceiverSource =
        [123.456m];
    
    [TestCaseSource(nameof(DecimalReceiverSource))]
    public void DecimalReceiver(decimal value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<decimal>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly DateTime[] DateTimeReceiverSource =
        [DateTime.Now];
    
    [TestCaseSource(nameof(DateTimeReceiverSource))]
    public void DateTimeReceiver(DateTime value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<DateTime>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly DateTimeOffset[] DateTimeOffsetReceiverSource =
        [DateTimeOffset.Now];
    
    [TestCaseSource(nameof(DateTimeOffsetReceiverSource))]
    public void DateTimeOffsetReceiver(DateTimeOffset value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<DateTimeOffset>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly TimeSpan[] TimeSpanReceiverSource =
        [DateTime.Now - DateTime.UnixEpoch];
    
    [TestCaseSource(nameof(TimeSpanReceiverSource))]
    public void TimeSpanReceiver(TimeSpan value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<TimeSpan>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString()));
    }

    private static readonly Guid[] GuidReceiverSource =
        [Guid.NewGuid()];
    
    [TestCaseSource(nameof(GuidReceiverSource))]
    public void GuidReceiver(Guid value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<Guid>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString()));
    }
    
    private static readonly Uri[] UriReceiverSource =
        [new Uri("http://example.com")];

    [TestCaseSource(nameof(UriReceiverSource))]
    public void UriReceiver(Uri value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<Uri>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value.ToString()));
    }

    [TestCase("ABC")]
    public void StringReceiver(string value)
    {
        CNode cn = value;

        Assert.That(cn.ToObject<string>(), Is.EqualTo(value));
        Assert.That(cn.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }
    
    private static readonly byte[][] BytesReceiverSource =
        [Guid.NewGuid().ToByteArray()];

    [TestCaseSource(nameof(BytesReceiverSource))]
    public void BytesReceiver(byte[] value)
    {
        CNode cn = value;

        Assert.That(cn?.ToObject<byte[]>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(Convert.ToBase64String(value)));
    }

    /////////////////////////////////////////////////////////////////////////

    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public void NullableBooleanReceiver(bool? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<bool?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123)]
    [TestCase(null)]
    public void NullableInt32Receiver(int? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<int?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123L)]
    [TestCase(null)]
    public void NullableInt64Receiver(long? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<long?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((byte)123)]
    [TestCase(null)]
    public void NullableByteReceiver(byte? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<byte?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((short)123)]
    [TestCase(null)]
    public void NullableInt16Receiver(short? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<short?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((sbyte)123)]
    [TestCase(null)]
    public void NullableSByteReceiver(sbyte? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<sbyte?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ushort)123)]
    [TestCase(null)]
    public void NullableUInt16Receiver(ushort? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<ushort?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((uint)123)]
    [TestCase(null)]
    public void NullableUInt32Receiver(uint? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<uint?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ulong)123)]
    [TestCase(null)]
    public void NullableUInt64Receiver(ulong? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<ulong?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456)]
    [TestCase(null)]
    public void NullableDoubleReceiver(double? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<double?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456f)]
    [TestCase(null)]
    public void NullableSingleReceiver(float? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<float?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly decimal?[] NullableDecimalReceiverSource =
        [123.456m, null];
    
    [TestCaseSource(nameof(NullableDecimalReceiverSource))]
    public void NullableDecimalReceiver(decimal? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<decimal?>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly DateTime?[] NullableDateTimeReceiverSource =
        [DateTime.Now, null];
    
    [TestCaseSource(nameof(NullableDateTimeReceiverSource))]
    public void NullableDateTimeReceiver(DateTime? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<DateTime>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly DateTimeOffset?[] NullableDateTimeOffsetReceiverSource =
        [DateTimeOffset.Now, null];
    
    [TestCaseSource(nameof(NullableDateTimeOffsetReceiverSource))]
    public void NullableDateTimeOffsetReceiver(DateTimeOffset? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<DateTimeOffset>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly TimeSpan?[] NullableTimeSpanReceiverSource =
        [DateTime.Now - DateTime.UnixEpoch, null];
    
    [TestCaseSource(nameof(NullableTimeSpanReceiverSource))]
    public void NullableTimeSpanReceiver(TimeSpan? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<TimeSpan>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    private static readonly Guid?[] NullableGuidReceiverSource =
        [Guid.NewGuid(), null];
    
    [TestCaseSource(nameof(NullableGuidReceiverSource))]
    public void NullableGuidReceiver(Guid? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<Guid>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }
    
    private static readonly Uri?[] NullableUriReceiverSource =
        [new Uri("http://example.com"), null];

    [TestCaseSource(nameof(NullableUriReceiverSource))]
    public void NullableUriReceiver(Uri? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<Uri>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    [TestCase("ABC")]
    [TestCase(null)]
    public void NullableStringReceiver(string? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }
        
    private static readonly byte[]?[] NullableBytesReceiverSource =
        [Guid.NewGuid().ToByteArray(), null];

    [TestCaseSource(nameof(NullableBytesReceiverSource))]
    public void NullableBytesReceiver(byte[]? value)
    {
        CNode? cn = value;

        Assert.That(cn?.ToObject<byte[]>(), Is.EqualTo(value));
        Assert.That(cn?.ToObject<string>(), Is.EqualTo(value != null ? Convert.ToBase64String(value) : null));
    }

    /////////////////////////////////////////////////////////////////////////
    
    [TestCaseSource(nameof(ValueSource))]
    public async Task WriteToRead(object? value, CNodeType? _)
    {
        var cn = CNode.Create(value) ?? CNullValue.Value;

        var ms = new MemoryStream();
        await cn.WriteToAsync(ms);

        ms.Position = 0;
        var actual = await CNode.ReadFromAsync(ms);
        
        Assert.That(actual?.ToObject(value?.GetType()!), Is.EqualTo(value));
    }
    
    [Test]
    public async Task WriteToReadMultipleInArray()
    {
        var expected = ValueSource.
            Select(arr => arr[0]).
            ToArray();
        var cns = expected.
            Select(CNode.Create).
            ToArray();

        var cn = CNode.Create(cns);

        var ms = new MemoryStream();
        await cn.WriteToAsync(ms);

        ms.Position = 0;
        var read = await CNode.ReadFromAsync(ms);
        
        var types = ValueSource.
            Select(arr => arr[0]?.GetType()).
            ToArray();

        var actual = ((CArray)read!).
            Zip(types, (r, t) => r?.ToObject(t!)).
            ToArray();

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task WriteToReadMultipleInObject()
    {
        var expected = ValueSource.
            Select((arr, i) => (i, v: arr[0])).
            ToDictionary(e => $"prop{e.i}", e => e.v);
        var cns = expected.
            ToDictionary(kv => kv.Key, kv => CNode.Create(kv.Value));

        var cn = CNode.Create(cns);

        var ms = new MemoryStream();
        await cn.WriteToAsync(ms);

        ms.Position = 0;
        var read = await CNode.ReadFromAsync(ms);
        
        var types = ValueSource.
            Select((arr, i) => (i, t: arr[0]?.GetType())).
            ToDictionary(e => $"prop{e.i}", e => e.t);

        var actual = ((CObject)read!).
            Select(kv => (kv.Key, Value: kv.Value?.ToObject(types[kv.Key]!))).
            ToDictionary(kv => kv.Key, kv => kv.Value);

        Assert.That(actual, Is.EqualTo(expected));
    }

    /////////////////////////////////////////////////////////////////////////
    
    //[Test]  // Compilation check only
    public async Task Overall1()
    {
        // Deserialize CBOR binary file.
        using var fs = new FileStream(
            "sample.cbor", FileMode.Open, FileAccess.Read, FileShare.Read, 65536, true);

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
                    Console.WriteLine($"item[{i}]: {arr[i]}");
                }
            }
        }
    }
    
    //[Test]  // Compilation check only
    public async Task Overall2()
    {
        // Deserialize CBOR binary file.
        using var fs1 = new FileStream(
            "sample.cbor", FileMode.Open, FileAccess.Read, FileShare.Read, 65536, true);

        // (If got null, replace with CNullValue to avoid NRE)
        CNode cn = await CNode.ReadFromAsync(fs1) ?? CNullValue.Value;

        // Serialize CBOR binary.
        using var fs2 = new FileStream(
            "dump.cbor", FileMode.Open, FileAccess.Read, FileShare.Read, 65536, true);

        await cn.WriteToAsync(fs2);
        await fs2.FlushAsync();
    }
    
    public void Overall3()
    {
        // The creator.
        CValue cvi = CNode.Create(123);
        CValue cvd = CNode.Create("ABC");

        // Implicitly conversion.
        CValue ci = 123;
        
        // Easy way with collection expression.
        CArray ca = CNode.Create(
        [
            CNode.Create(123),
            CNode.Create(456.789),
            null,
            CNode.Create("ABC"),
            (CNode)Guid.NewGuid(),
        ]);

        // Easy way with tuple type.
        CObject co1 = CNode.Create(
            ("prop1", CNode.Create(123)),
            ("prop2", CNode.Create(456.789)),
            ("prop3", CNode.Create("ABC")));

        // From dictionary.
        Dictionary<string, CNode?> dict = new()
        {
            ["prop1"] = CNode.Create(123),
            ["prop2"] = CNode.Create(456.789),
            ["prop3"] = CNode.Create("ABC"),
        };
        CObject co2 = new(dict);
    }
}
