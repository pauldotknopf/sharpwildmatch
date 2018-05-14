using System;
using Xunit;

namespace SharpWildmatch.Tests
{
    public class CharPointerTests
    {
        [Fact]
        public void Char_pointer_tests()
        {
            CharPointer pointer = CharPointer.Null;
            Assert.Equal(pointer, CharPointer.Null);
            Assert.Null(pointer.Source);
            Assert.Null(pointer.Value);
            Assert.Throws<Exception>(() => { pointer.Increment(); });

            pointer = null;
            Assert.Equal(pointer, CharPointer.Null);
            Assert.Null(pointer.Value);

            pointer = "";
            Assert.NotEqual(pointer, CharPointer.Null);
        }

        [Fact]
        public void Char_can_point_to_null_terminator()
        {
            CharPointer test = "t";
            Assert.Equal(test.Value, 't');
            test = test.Increment();
            Assert.Equal(test.Value, '\0');
        }

        [Fact]
        public void Char_pointer_check_for_valid_char()
        {
            CharPointer pointer = "";
            Assert.Equal(pointer.HasValidChar, true);
            Assert.Equal(pointer.Value, '\0');
            pointer = pointer.Increment();
            Assert.Equal(pointer.HasValidChar, false);
            Assert.Throws<Exception>(() => pointer.Value);
            pointer = "";
            pointer = pointer.Increment(-1);
            Assert.Equal(pointer.HasValidChar, false);
        }
    }
}