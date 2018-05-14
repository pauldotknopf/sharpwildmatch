using System;

namespace SharpWildmatch
{
    internal struct CharPointer
    {
        private readonly string _source;
        private readonly int _index;

        public static CharPointer Null;
        
        public CharPointer(string source, int index = 0)
        {
            _source = source ?? throw new Exception("Source can't be null, use CharPointer.Null");
            _index = index;
        }

        public string Source => _source;

        public char Value
        {
            get
            {
                if(!HasValidChar) throw new Exception("Invalid char");
                if (_index == _source.Length) return '\0';
                return _source[_index];
            }
        }

        public int Index => _index;

        public bool HasValidChar
        {
            get
            {
                if (_source == null) return false;
                if (_index < 0) return false;
                if (_index > _source.Length) return false;
                return true;
            }
        }
        
        public bool HasMore()
        {
            return _index < _source.Length;
        }

        public CharPointer Increment()
        {
            return Increment(1);
        }
        
        public CharPointer Increment(int value)
        {
            if(_source == null) throw new Exception("Can't increment null char pointer.");
            return new CharPointer(_source, _index+value);
        }
        
        public static bool operator !=(CharPointer x, CharPointer y)
        {
            return x.GetHashCode() != y.GetHashCode();
        }
        
        public static bool operator ==(CharPointer x, CharPointer y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _source.GetHashCode() + _index.GetHashCode();
        }
        
        public static implicit operator CharPointer(string value)
        {
            return value == null ? Null : new CharPointer(value);
        }
    }
}