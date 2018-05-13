namespace SharpWildmatch
{
    internal class Sane
    {
        // ReSharper disable InconsistentNaming
        private const uint GIT_SPACE = 0x01;
        private const uint GIT_DIGIT = 0x02;
        private const uint GIT_ALPHA = 0x04;
        private const uint GIT_GLOB_SPECIAL = 0x08;
        private const uint GIT_REGEX_SPECIAL = 0x10;
        private const uint GIT_PATHSPEC_MAGIC = 0x20;
        private const uint GIT_CNTRL = 0x40;
        private const uint GIT_PUNCT = 0x80;
        // ReSharper restore InconsistentNaming
        
        private const uint S = GIT_SPACE;
        private const uint A = GIT_ALPHA;
        private const uint D = GIT_DIGIT;
        private const uint G = GIT_GLOB_SPECIAL;	/* *, ?, [, \\ */
        private const uint R = GIT_REGEX_SPECIAL;	/* $, (, ), +, ., ^, {, | */
        private const uint P = GIT_PATHSPEC_MAGIC; /* other non-alnum, except for ] and } */
        private const uint X = GIT_CNTRL;
        private const uint U = GIT_PUNCT;
        private const uint Z = GIT_CNTRL | GIT_SPACE;

        private static readonly uint[] SaneTypes = {
            X, X, X, X, X, X, X, X, X, Z, Z, X, X, Z, X, X,		/*   0.. 15 */
            X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X,		/*  16.. 31 */
            S, P, P, P, R, P, P, P, R, R, G, R, P, P, R, P,		/*  32.. 47 */
            D, D, D, D, D, D, D, D, D, D, P, P, P, P, P, G,		/*  48.. 63 */
            P, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,		/*  64.. 79 */
            A, A, A, A, A, A, A, A, A, A, A, G, G, U, R, P,		/*  80.. 95 */
            P, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,		/*  96..111 */
            A, A, A, A, A, A, A, A, A, A, A, R, R, U, P, X,		/* 112..127 */
        };

        private static bool SaneIstest(char value, uint mask)
        {
            return (SaneTypes[value] & mask) != 0;
        }

        public static bool IsGlobSpecial(char value)
        {
            return SaneIstest(value, GIT_GLOB_SPECIAL);
        }
    }
}