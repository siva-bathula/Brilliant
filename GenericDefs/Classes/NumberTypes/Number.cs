using System;
namespace GenericDefs.Classes.NumberTypes
{
    public class Number<T>
    {
        bool _isintorlong = false;
        bool _isInt = false;
        bool _isLong = false;
        public Number(T value) {
            if (typeof(T) == typeof(int))
            {
                _isintorlong = true;
                _isInt = true;
            }
            if( typeof(T) == typeof(long)) {
                _isintorlong = true;
                _isLong = true;
            }
            Value = value;
        }
        public T Value { get; set; }

        public void Increment()
        {
            if (_isintorlong)
            {
                if (_isInt) { Value = (T)(object)(((int)(object)Value) + 1); }
                else if (_isLong) { Value = (T)(object)(((long)(object)Value) + 1); }
            } else { throw new Exception("Supports only int and long for now."); }
        }

        public void Decrement()
        {
            if (_isintorlong)
            {
                if (_isInt) { Value = (T)(object)(((int)(object)Value) - 1); }
                else if (_isLong) { Value = (T)(object)(((long)(object)Value) - 1); }
            } else { throw new Exception("Supports only int and long for now."); }
        }

        public void UpdateMax(T newValue)
        {
            if (_isintorlong)
            {
                long t1 = (long)Convert.ChangeType(Value, TypeCode.Int64);
                long t2 = (long)Convert.ChangeType(newValue, TypeCode.Int64);

                if (t2 > t1) Value = newValue;
            } else { throw new Exception("Supports only int and long for now."); }
        }
    }
}