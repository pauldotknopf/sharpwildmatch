namespace SharpWildmatch
{
    internal static class Sane
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
        
        private static readonly int[] HexTable =
        {
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 00-07 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 08-0f */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 10-17 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 18-1f */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 20-27 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 28-2f */
            0,  1,  2,  3,  4,  5,  6,  7,		/* 30-37 */
            8,  9, -1, -1, -1, -1, -1, -1,		/* 38-3f */
            -1, 10, 11, 12, 13, 14, 15, -1,		/* 40-47 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 48-4f */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 50-57 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 58-5f */
            -1, 10, 11, 12, 13, 14, 15, -1,		/* 60-67 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 68-67 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 70-77 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 78-7f */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 80-87 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 88-8f */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 90-97 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* 98-9f */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* a0-a7 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* a8-af */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* b0-b7 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* b8-bf */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* c0-c7 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* c8-cf */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* d0-d7 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* d8-df */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* e0-e7 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* e8-ef */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* f0-f7 */
            -1, -1, -1, -1, -1, -1, -1, -1,		/* f8-ff */
        };

        private static bool SaneIstest(char value, uint mask)
        {
            return (SaneTypes[value] & mask) != 0;
        }

        public static bool IsGlobSpecial(char value)
        {
            return SaneIstest(value, GIT_GLOB_SPECIAL);
        }

        public static bool IsAscii(char value)
        {
            return (value & ~0x7f) == 0;
        }

        public static bool IsAlNum(char value)
        {
            return IsAscii(value) && SaneIstest(value, GIT_ALPHA | GIT_DIGIT);
        }

        public static bool IsAlpha(char value)
        {
            return IsAscii(value) && SaneIstest(value, GIT_ALPHA);
        }

        public static bool IsBlank(char value)
        {
            return value == ' ' || value == '\t';
        }

        public static bool IsCtrl(char value)
        {
            return IsAscii(value) && SaneIstest(value, GIT_CNTRL);
        }

        public static bool IsDigit(char value)
        {
            return IsAscii(value) && SaneIstest(value, GIT_DIGIT);
        }

        public static bool IsGraph(char value)
        {
            // TODO: is this right?
            return IsAscii(value);
        }

        public static bool IsPrint(char value)
        {
            return IsAscii(value) && (value >= 0x20 && value <= 0x7e);
        }

        public static bool IsPunc(char value)
        {
            return IsAscii(value) &&
                   SaneIstest(value, GIT_PUNCT | GIT_REGEX_SPECIAL | GIT_GLOB_SPECIAL | GIT_PATHSPEC_MAGIC);
        }

        public static bool IsSpace(char value)
        {
            return IsAscii(value) && SaneIstest(value, GIT_SPACE);
        }

        public static bool IsXDigit(char value)
        {
            return IsAscii(value) && HexTable[value] != -1;
        }
    }
}