using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InputSimulator.Internal
{
    static class EnumExtensions
    {
        /// <summary>
        /// Gets a single merged value containing all of the flags in the <paramref name="enumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of enum being operated on.</typeparam>
        /// <param name="enumerable">(implicit) Enumerable containing enum values of type <typeparamref name="T"/>.</param>
        /// <returns>A single enum value containing all of the values in the <paramref name="enumerable"/>.</returns>
        public static T GetSingleValue<T>(this IEnumerable<T> enumerable) where T : struct, Enum
        {
            var result = Convert.ToInt64(default(T));

            foreach (var value in enumerable)
            {
                result |= Convert.ToInt64(value);
            }

            return (T)Enum.ToObject(typeof(T), result);
        }
        /// <summary>
        /// Checks if the enum value has 0 or 1 bits set in total.
        /// </summary>
        /// <typeparam name="T">The type of enum being operated on.</typeparam>
        /// <param name="e">(implicit) Enum value to operate on.</param>
        /// <returns><see langword="true"/> when the enum value is a power of 2; <see langword="false"/>.</returns>
        public static bool IsSingleValue<T>(this T e) where T : struct, Enum
        {
            var e_v = Convert.ToInt64(e);

            return e_v == 0 || (e_v & e_v - 1) == 0;
        }
        /// <summary>
        /// Gets the flags that are set in the enum value as individual values.
        /// </summary>
        /// <typeparam name="T">The type of enum being operated on.</typeparam>
        /// <param name="e">(implicit) Enum value to operate on.</param>
        /// <returns>An array of the flags that were set in the enum value; when no flags were set, the array contains 1 zero value.</returns>
        public static T[] GetSingleValues<T>(this T e) where T : struct, Enum
        {
            var e_v = Convert.ToInt64(e);
            if (e_v == 0)
                return new[] { e };

            List<T> l = new();

            for (int i = 0, bit = 0x1, bitCount = 8 * Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
                i < bitCount;
                ++i, bit = 0x1 << i)
            {
                if ((e_v & bit) != 0)
                    l.Add((T)Enum.ToObject(typeof(T), bit));
            }

            return l.ToArray();
        }
    }
}
