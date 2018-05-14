namespace SharpWildmatch
{
    internal static class Extensions
    {
        public static char? At(this string value, int index)
        {
            if (index < 0) return null;
            return index >= value.Length ? (char?)null : value[index];
        }
    }
}