using System;
using System.Globalization;

namespace Sinantrop.Logger.Helpers
{
    public struct ByteSize : IComparable<ByteSize>, IEquatable<ByteSize>
    {
        public static readonly ByteSize MinValue = FromBits(0);
        public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

        public const long BitsInByte = 8;
        public const long BytesInKiloByte = 1024;
        public const long BytesInMegaByte = 1048576;
        public const long BytesInGigaByte = 1073741824;
        public const long BytesInTeraByte = 1099511627776;
        public const long BytesInPetaByte = 1125899906842624;

        public const string BitSymbol = "b";
        public const string ByteSymbol = "B";
        public const string KiloByteSymbol = "KB";
        public const string MegaByteSymbol = "MB";
        public const string GigaByteSymbol = "GB";
        public const string TeraByteSymbol = "TB";
        public const string PetaByteSymbol = "PB";

        public long Bits { get; private set; }
        public double Bytes { get; private set; }
        public double KiloBytes { get; private set; }
        public double MegaBytes { get; private set; }
        public double GigaBytes { get; private set; }
        public double TeraBytes { get; private set; }
        public double PetaBytes { get; private set; }

        public string LargestWholeNumberSymbol
        {
            get
            {
                // Absolute value is used to deal with negative values
                if (Math.Abs(PetaBytes) >= 1)
                    return PetaByteSymbol;

                if (Math.Abs(TeraBytes) >= 1)
                    return TeraByteSymbol;

                if (Math.Abs(GigaBytes) >= 1)
                    return GigaByteSymbol;

                if (Math.Abs(MegaBytes) >= 1)
                    return MegaByteSymbol;

                if (Math.Abs(KiloBytes) >= 1)
                    return KiloByteSymbol;

                if (Math.Abs(Bytes) >= 1)
                    return ByteSymbol;

                return BitSymbol;
            }
        }

        public double LargestWholeNumberValue
        {
            get
            {
                // Absolute value is used to deal with negative values
                if (Math.Abs(PetaBytes) >= 1)
                    return PetaBytes;

                if (Math.Abs(TeraBytes) >= 1)
                    return TeraBytes;

                if (Math.Abs(GigaBytes) >= 1)
                    return GigaBytes;

                if (Math.Abs(MegaBytes) >= 1)
                    return MegaBytes;

                if (Math.Abs(KiloBytes) >= 1)
                    return KiloBytes;

                if (Math.Abs(Bytes) >= 1)
                    return Bytes;

                return Bits;
            }
        }

        public ByteSize(double byteSize)
            : this()
        {
            // Get ceiling because bis are whole units
            Bits = (long)Math.Ceiling(byteSize * BitsInByte);

            Bytes = byteSize;
            KiloBytes = byteSize / BytesInKiloByte;
            MegaBytes = byteSize / BytesInMegaByte;
            GigaBytes = byteSize / BytesInGigaByte;
            TeraBytes = byteSize / BytesInTeraByte;
            PetaBytes = byteSize / BytesInPetaByte;
        }

        public static ByteSize FromBits(long value)
        {
            return new ByteSize(value / (double)BitsInByte);
        }

        public static ByteSize FromBytes(double value)
        {
            return new ByteSize(value);
        }

        public static ByteSize FromKiloBytes(double value)
        {
            return new ByteSize(value * BytesInKiloByte);
        }

        public static ByteSize FromMegaBytes(double value)
        {
            return new ByteSize(value * BytesInMegaByte);
        }

        public static ByteSize FromGigaBytes(double value)
        {
            return new ByteSize(value * BytesInGigaByte);
        }

        public static ByteSize FromTeraBytes(double value)
        {
            return new ByteSize(value * BytesInTeraByte);
        }

        public static ByteSize FromPetaBytes(double value)
        {
            return new ByteSize(value * BytesInPetaByte);
        }

        /// <summary>
        /// Converts the value of the current ByteSize object to a string.
        /// The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is
        /// the largest metric prefix such that the corresponding value is greater
        //  than or equal to one.
        /// </summary>
        public override string ToString()
        {
            return ToString("#.##", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (!format.Contains("#") && !format.Contains("0"))
                format = "#.## " + format;

            if (provider == null) provider = CultureInfo.CurrentCulture;

            Func<string, bool> has = s => format.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) != -1;
            Func<double, string> output = n => n.ToString(format, provider);

            if (has("PB"))
                return output(PetaBytes);
            if (has("TB"))
                return output(TeraBytes);
            if (has("GB"))
                return output(GigaBytes);
            if (has("MB"))
                return output(MegaBytes);
            if (has("KB"))
                return output(KiloBytes);

            // Byte and Bit symbol must be case-sensitive
            if (format.IndexOf(ByteSymbol, StringComparison.Ordinal) != -1)
                return output(Bytes);

            if (format.IndexOf(BitSymbol, StringComparison.Ordinal) != -1)
                return output(Bits);

            return $"{LargestWholeNumberValue.ToString(format, provider)} {LargestWholeNumberSymbol}";
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            ByteSize other;
            if (value is ByteSize)
                other = (ByteSize)value;
            else
                return false;

            return Equals(other);
        }

        public bool Equals(ByteSize value)
        {
            return Bits == value.Bits;
        }

        public override int GetHashCode()
        {
            return Bits.GetHashCode();
        }

        public int CompareTo(ByteSize other)
        {
            return Bits.CompareTo(other.Bits);
        }

        public ByteSize Add(ByteSize bs)
        {
            return new ByteSize(Bytes + bs.Bytes);
        }

        public ByteSize AddBits(long value)
        {
            return this + FromBits(value);
        }

        public ByteSize AddBytes(double value)
        {
            return this + FromBytes(value);
        }

        public ByteSize AddKiloBytes(double value)
        {
            return this + FromKiloBytes(value);
        }

        public ByteSize AddMegaBytes(double value)
        {
            return this + FromMegaBytes(value);
        }

        public ByteSize AddGigaBytes(double value)
        {
            return this + FromGigaBytes(value);
        }

        public ByteSize AddTeraBytes(double value)
        {
            return this + FromTeraBytes(value);
        }

        public ByteSize AddPetaBytes(double value)
        {
            return this + FromPetaBytes(value);
        }

        public ByteSize Subtract(ByteSize bs)
        {
            return new ByteSize(Bytes - bs.Bytes);
        }

        public static ByteSize operator +(ByteSize b1, ByteSize b2)
        {
            return new ByteSize(b1.Bytes + b2.Bytes);
        }

        public static ByteSize operator ++(ByteSize b)
        {
            return new ByteSize(b.Bytes + 1);
        }

        public static ByteSize operator -(ByteSize b)
        {
            return new ByteSize(-b.Bytes);
        }

        public static ByteSize operator --(ByteSize b)
        {
            return new ByteSize(b.Bytes - 1);
        }

        public static bool operator ==(ByteSize b1, ByteSize b2)
        {
            return b1.Bits == b2.Bits;
        }

        public static bool operator !=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits != b2.Bits;
        }

        public static bool operator <(ByteSize b1, ByteSize b2)
        {
            return b1.Bits < b2.Bits;
        }

        public static bool operator <=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits <= b2.Bits;
        }

        public static bool operator >(ByteSize b1, ByteSize b2)
        {
            return b1.Bits > b2.Bits;
        }

        public static bool operator >=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits >= b2.Bits;
        }

        public static bool TryParse(string s, out ByteSize result)
        {
            // Arg checking
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "String is null or whitespace");

            // Setup the result
            result = new ByteSize();

            // Get the index of the first non-digit character
            s = s.TrimStart(); // Protect against leading spaces

            int num;
            var found = false;

            var decimalSeparator = Convert.ToChar(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            var groupSeparator = Convert.ToChar(NumberFormatInfo.CurrentInfo.NumberGroupSeparator);

            // Pick first non-digit number
            for (num = 0; num < s.Length; num++)
                if (!(char.IsDigit(s[num]) || s[num] == decimalSeparator || s[num] == groupSeparator))
                {
                    found = true;
                    break;
                }

            if (found == false)
                return false;

            int lastNumber = num;

            // Cut the input string in half
            string numberPart = s.Substring(0, lastNumber).Trim();
            string sizePart = s.Substring(lastNumber, s.Length - lastNumber).Trim();

            // Get the numeric part
            double number;
            if (!double.TryParse(numberPart, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out number))
                return false;

            // Get the magnitude part
            switch (sizePart)
            {
                case "b":
                    if (Math.Abs(number % 1) > 0) // Can't have partial bits
                        return false;

                    result = FromBits((long)number);
                    break;

                case "B":
                    result = FromBytes(number);
                    break;

                case "KB":
                case "kB":
                case "kb":
                    result = FromKiloBytes(number);
                    break;

                case "MB":
                case "mB":
                case "mb":
                    result = FromMegaBytes(number);
                    break;

                case "GB":
                case "gB":
                case "gb":
                    result = FromGigaBytes(number);
                    break;

                case "TB":
                case "tB":
                case "tb":
                    result = FromTeraBytes(number);
                    break;

                case "PB":
                case "pB":
                case "pb":
                    result = FromPetaBytes(number);
                    break;
            }

            return true;
        }

        public static ByteSize Parse(string s)
        {
            ByteSize result;

            if (TryParse(s, out result))
                return result;

            throw new FormatException("Value is not in the correct format");
        }
    }
}
