using System;
using System.Globalization;

namespace Tharga.Toolkit
{
    public class Checksum
    {
        //The length check-sum is used for "Plusgiro".
        //The length check-sum cannot be used for "Person" eller "Org" numbers.
        //The length check-sum can be added for "Bankgiro" but is not used.
        public static string CreateLuhnString(string valueWithoutChecksum, bool useLengthChecksum = true)
        {
            if (useLengthChecksum)
                valueWithoutChecksum += GetLengthChecksum(valueWithoutChecksum);

            var result = valueWithoutChecksum + CalculateLuhnChecksum(valueWithoutChecksum);

            if (!VerifyLuhnChecksum(result))
                throw new InvalidOperationException(string.Format("The Luhn-string {0} is not valid after creation. Input was {1}.", result, valueWithoutChecksum));

            return result;
        }

        private static long CalculateLuhnChecksum(string valueWithoutChecksum)
        {
            long lngSum = 0;

            for (var lngPos = 0; lngPos < valueWithoutChecksum.Length; lngPos++)
            {
                var lngTemp = int.Parse(valueWithoutChecksum[lngPos].ToString(CultureInfo.InvariantCulture)) * (((valueWithoutChecksum.Length - lngPos) % 2) + 1);

                if (lngTemp > 9)
                    lngTemp = lngTemp - 9;

                lngSum = lngSum + lngTemp;
            }

            var checkNumber = (10 - (lngSum % 10)) % 10;

            return checkNumber;
        }

        private static string GetChecksumFromLuhnString(string luhnStringWithChecksum)
        {
            return string.IsNullOrEmpty(luhnStringWithChecksum) ? string.Empty : luhnStringWithChecksum.Substring(luhnStringWithChecksum.Length - 1);
        }

        private static int GetLengthChecksum(string valueWithoutChecksum)
        {
            var valueLengthWithChecksum = (valueWithoutChecksum.Length + 2).ToString(CultureInfo.InvariantCulture);
            var lengthCheckSum = valueLengthWithChecksum.Substring(valueLengthWithChecksum.Length - 1);
            return int.Parse(lengthCheckSum);
        }

        public static bool VerifyLengthChecksum(string luhnStringWithChecksum)
        {
            var valueLengthWithChecksum =
                (luhnStringWithChecksum.Length).ToString(CultureInfo.InvariantCulture);
            var lengthCheckSum =
                valueLengthWithChecksum.Substring(valueLengthWithChecksum.Length - 1);
            var actualValue =
                luhnStringWithChecksum.Substring(luhnStringWithChecksum.Length - 2, 1);
            return actualValue == lengthCheckSum;
        }

        public static bool VerifyLuhnChecksum(string luhnStringWithChecksum)
        {
            var checksum = GetChecksumFromLuhnString(luhnStringWithChecksum);
            var stringWithoutSum = luhnStringWithChecksum.Substring(0, luhnStringWithChecksum.Length - 1);
            var calculatedChecksum = CalculateLuhnChecksum(stringWithoutSum);
            return calculatedChecksum == long.Parse(checksum);
        }
    }
}