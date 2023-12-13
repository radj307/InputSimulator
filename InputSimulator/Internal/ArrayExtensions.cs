using System;

namespace InputSimulator.Internal
{
    internal static class ArrayExtensions
    {
        #region Concat
        public static T[] Concat<T>(this T[] first, T[] second)
        {
            var outArray = new T[first.Length + second.Length];
            int i_outArray = 0;
            for (int i = 0, i_max = first.Length; i < i_max; ++i)
            {
                outArray[i_outArray++] = first[i];
            }
            for (int i = 0, i_max = second.Length; i < i_max; ++i)
            {
                outArray[i_outArray++] = second[i];
            }
            return outArray;
        }
        #endregion Concat

        #region SetValuesTo
        public static T[] SetValuesTo<T>(this T[] array, T value, Range range)
        {
            var (off, length) = range.GetOffsetAndLength(array.Length);
            for (int i = off; i < length; ++i)
            {
                array[i] = value;
            }
            return array;
        }
        public static T[] SetValuesTo<T>(this T[] array, T value)
        {
            for (int i = 0, i_max = array.Length; i < i_max; ++i)
            {
                array[i] = value;
            }
            return array;
        }

        // values array (unsafe):

        public static T[] SetValuesTo<T>(this T[] array, T[] values, Range range)
        {
            var (off, length) = range.GetOffsetAndLength(array.Length);
            var valuesLength = values.Length;

            if (length % valuesLength != 0)
                throw new InvalidOperationException($"The length of the specified values ({values.Length}) is not divisible by the specified range length of {length}!");

            for (int i = off; i < length; )
            {
                for (int j = 0; j < valuesLength; ++j)
                {
                    array[i++] = values[j];
                }
            }
            return array;
        }
        public static T[] SetValuesTo<T>(this T[] array, T[] values)
        {
            var arrayLength = array.Length;
            var valuesLength = values.Length;

            if (arrayLength % valuesLength != 0)
                throw new InvalidOperationException($"The length of the specified values ({values.Length}) is not divisible by the array length of {arrayLength}!");

            for (int i = 0; i < arrayLength; )
            {
                for (int j = 0; j < valuesLength; ++j)
                {
                    array[i++] = values[j];
                }
            }
            return array;
        }
        #endregion SetValuesTo
    }
}
