namespace NSKurtsTools;

// public static partial class KurtsTools{
    public static class ArrayExtensions{
        /// <summary>
        /// Extension to the Array class
        /// </summary>
        /// <returns>A sub array of the main array</returns>
        /// <example><code>
        /// 
        /// </code></example>
        public static T[] SubArray<T>(this T[] array, int offset, int length){
            if (offset >= array.Length) return Array.Empty<T>();
            
            if (offset + length > array.Length) length = array.Length - offset;
            
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
// }