namespace SharpWildmatch
{
    internal static class Extensions
    {
        public static char? At(this string value, int index)
        {
            return index >= value.Length ? (char?)null : value[index];
        }
    }
}