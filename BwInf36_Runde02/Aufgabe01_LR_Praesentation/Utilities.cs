namespace Aufgabe01_LR_Praesentation
{
    /// <summary>
    /// Helper methods for Aufgabe01_LR
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Fills an Array with a given value.
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="array">The Array</param>
        /// <param name="value">The value</param>
        public static void FillArray<T>(this T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}