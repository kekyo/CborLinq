// CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
// Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

using System.Globalization;
using NUnit.Framework;

namespace System.Formats.Cbor.Linq;

public sealed class CValueTest
{
    [TestCase(true)]
    [TestCase(false)]
    public void BooleanReceiver(bool value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<bool>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123)]
    public void Int32Receiver(int value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<int>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123L)]
    public void Int64Receiver(long value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<long>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((byte)123)]
    public void ByteReceiver(byte value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<byte>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((short)123)]
    public void Int16Receiver(short value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<short>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((sbyte)123)]
    public void SByteReceiver(sbyte value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<sbyte>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ushort)123)]
    public void UInt16Receiver(ushort value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<ushort>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((uint)123)]
    public void UInt32Receiver(uint value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<uint>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ulong)123)]
    public void UInt64Receiver(ulong value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<ulong>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456)]
    public void DoubleReceiver(double value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<double>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456f)]
    public void SingleReceiver(float value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<float>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly decimal[] DecimalReceiverSource =
        [123.456m];
    
    [TestCaseSource(nameof(DecimalReceiverSource))]
    public void DecimalReceiver(decimal value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<decimal>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly DateTime[] DateTimeReceiverSource =
        [DateTime.Now];
    
    [TestCaseSource(nameof(DateTimeReceiverSource))]
    public void DateTimeReceiver(DateTime value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<DateTime>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly DateTimeOffset[] DateTimeOffsetReceiverSource =
        [DateTimeOffset.Now];
    
    [TestCaseSource(nameof(DateTimeOffsetReceiverSource))]
    public void DateTimeOffsetReceiver(DateTimeOffset value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<DateTimeOffset>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly TimeSpan[] TimeSpanReceiverSource =
        [DateTime.Now - DateTime.UnixEpoch];
    
    [TestCaseSource(nameof(TimeSpanReceiverSource))]
    public void TimeSpanReceiver(TimeSpan value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<TimeSpan>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString()));
    }

    private static readonly Guid[] GuidReceiverSource =
        [Guid.NewGuid()];
    
    [TestCaseSource(nameof(GuidReceiverSource))]
    public void GuidReceiver(Guid value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<Guid>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString()));
    }
    
    private static readonly Uri[] UriReceiverSource =
        [new Uri("http://example.com")];

    [TestCaseSource(nameof(UriReceiverSource))]
    public void UriReceiver(Uri value)
    {
        CValue cv = value;

        Assert.That(cv?.ToObject<Uri>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value.ToString()));
    }

    [TestCase("ABC")]
    public void StringReceiver(string value)
    {
        CValue cv = value;

        Assert.That(cv.ToObject<string>(), Is.EqualTo(value));
        Assert.That(cv.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }
        
    private static readonly byte[][] BytesReceiverSource =
        [Guid.NewGuid().ToByteArray()];

    [TestCaseSource(nameof(BytesReceiverSource))]
    public void BytesReceiver(byte[] value)
    {
        CValue cv = value;

        Assert.That(cv.ToObject<byte[]>(), Is.EqualTo(value));
        Assert.That(cv.ToObject<string>(), Is.EqualTo(Convert.ToBase64String(value)));
    }

    /////////////////////////////////////////////////////////////////////////

    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public void NullableBooleanReceiver(bool? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<bool?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123)]
    [TestCase(null)]
    public void NullableInt32Receiver(int? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<int?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123L)]
    [TestCase(null)]
    public void NullableInt64Receiver(long? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<long?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((byte)123)]
    [TestCase(null)]
    public void NullableByteReceiver(byte? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<byte?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((short)123)]
    [TestCase(null)]
    public void NullableInt16Receiver(short? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<short?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((sbyte)123)]
    [TestCase(null)]
    public void NullableSByteReceiver(sbyte? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<sbyte?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ushort)123)]
    [TestCase(null)]
    public void NullableUInt16Receiver(ushort? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<ushort?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((uint)123)]
    [TestCase(null)]
    public void NullableUInt32Receiver(uint? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<uint?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase((ulong)123)]
    [TestCase(null)]
    public void NullableUInt64Receiver(ulong? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<ulong?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456)]
    [TestCase(null)]
    public void NullableDoubleReceiver(double? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<double?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase(123.456f)]
    [TestCase(null)]
    public void NullableSingleReceiver(float? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<float?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly decimal?[] NullableDecimalReceiverSource =
        [123.456m, null];
    
    [TestCaseSource(nameof(NullableDecimalReceiverSource))]
    public void NullableDecimalReceiver(decimal? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<decimal?>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }

    private static readonly DateTime?[] NullableDateTimeReceiverSource =
        [DateTime.Now, null];
    
    [TestCaseSource(nameof(NullableDateTimeReceiverSource))]
    public void NullableDateTimeReceiver(DateTime? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<DateTime>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly DateTimeOffset?[] NullableDateTimeOffsetReceiverSource =
        [DateTimeOffset.Now, null];
    
    [TestCaseSource(nameof(NullableDateTimeOffsetReceiverSource))]
    public void NullableDateTimeOffsetReceiver(DateTimeOffset? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<DateTimeOffset>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CValue.DateTimeOffsetFormatString, CultureInfo.InvariantCulture)));
    }

    private static readonly TimeSpan?[] NullableTimeSpanReceiverSource =
        [DateTime.Now - DateTime.UnixEpoch, null];
    
    [TestCaseSource(nameof(NullableTimeSpanReceiverSource))]
    public void NullableTimeSpanReceiver(TimeSpan? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<TimeSpan>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    private static readonly Guid?[] NullableGuidReceiverSource =
        [Guid.NewGuid(), null];
    
    [TestCaseSource(nameof(NullableGuidReceiverSource))]
    public void NullableGuidReceiver(Guid? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<Guid>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }
    
    private static readonly Uri?[] NullableUriReceiverSource =
        [new Uri("http://example.com"), null];

    [TestCaseSource(nameof(NullableUriReceiverSource))]
    public void NullableUriReceiver(Uri? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<Uri>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString()));
    }

    [TestCase("ABC")]
    [TestCase(null)]
    public void NullableStringReceiver(string? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value?.ToString(CultureInfo.InvariantCulture)));
    }
        
    private static readonly byte[]?[] NullableBytesReceiverSource =
        [Guid.NewGuid().ToByteArray(), null];

    [TestCaseSource(nameof(NullableBytesReceiverSource))]
    public void NullableBytesReceiver(byte[]? value)
    {
        CValue? cv = value;

        Assert.That(cv?.ToObject<byte[]>(), Is.EqualTo(value));
        Assert.That(cv?.ToObject<string>(), Is.EqualTo(value != null ? Convert.ToBase64String(value) : null));
    }
}
