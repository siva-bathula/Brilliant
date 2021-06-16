using System;
using System.Collections.Generic;

namespace GenericDefs.Classes.Quirky
{
    /// <summary>
    /// For iteration over a single variable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Iterator<T>
    {
        public bool IsIncrement { get; private set; }
        public Range<T> Range { get; private set; }
        public Iterator(bool isIncrement, Range<T> range)
        {
            IsIncrement = isIncrement;
            Range = range;
        }
    }

    /// <summary>
    /// For iteration over a single variable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Iterenumerator<T>
    {
        /// <summary>
        /// Allowed set of values. Iternumerator will return objects in sorted order.
        /// </summary>
        public List<T> Set { get; private set; }
        public Iterenumerator(List<T> range)
        {
            range.Sort();
            Set = range;
        }

        public T GetLast()
        {
            return Set[Set.Count - 1];
        }

        T _current;
        int _currentIndex;
        public T MoveNext()
        {
            if (_current == null)
            {
                _currentIndex = 0;
            }
            else {
                _currentIndex++;
                if (_currentIndex > Set.Count - 1) _currentIndex = 0;
            }
            _current = Set[_currentIndex];

            return _current;
        }
    }

    /// <summary>
    /// Use this if next step needs historical enumeration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomIterenumerator<T> where T : struct
    {
        /// <summary>
        /// Allowed set of values. Iternumerator will return objects in sorted order.
        /// </summary>
        public CustomIterenumerator(Func<CustomIterenumerator<T>, T?> customEnum, Func<CustomIterenumerator<T>, bool> validator)
        {
            _customEnum = customEnum;
            _validator = validator;
        }

        /// <summary>
        /// Allowed set of values. Iternumerator will return objects in sorted order.
        /// </summary>
        public CustomIterenumerator(Func<CustomIterenumerator<T>, T?> customEnum, Func<CustomIterenumerator<T>, bool> validator,
            Func<CustomIterenumerator<T>, T, Tuple<IEnumerable<T>, bool>> generator)
        {
            _customEnum = customEnum;
            _validator = validator;
            _generator = generator;
            _hasGenerator = true;
        }

        public int Depth
        {
            get; set;
        }

        bool _hasGenerator = false;
        public bool HasCoefficientGenerator()
        {
            return _hasGenerator;
        }

        private Func<CustomIterenumerator<T>, T?> _customEnum;
        private Func<CustomIterenumerator<T>, bool> _validator;
        private Func<CustomIterenumerator<T>, T, Tuple<IEnumerable<T>, bool>> _generator;

        T? _current;
        int _currentIndex = -1;
        public T? MoveNext()
        {
            if (_current == null) _currentIndex = 0;
            else { _currentIndex++; }
            _current = _customEnum.Invoke(this);
            if (_current == null) _currentIndex = -1;
            return _current;
        }

        /// <summary>
        /// How the iteration was carried on until that point.
        /// </summary>
        public object History { get; set; }

        public int CurrentIndex { get { return _currentIndex; } }

        public T? Current() { return _current; }

        public bool IsValid()
        {
            if (_validator == null) return true;
            return _validator.Invoke(this);
        }

        public Tuple<IEnumerable<T>, bool> GenerateCoefficients(T newObject)
        {
            return _generator.Invoke(this, newObject);
        }

        public CustomIterenumerator<T> Clone()
        {
            var clone1 = _customEnum == null ? null : (Func<CustomIterenumerator<T>, T?>)_customEnum.Clone();
            var clone2 = _validator == null ? null : (Func<CustomIterenumerator<T>, bool>)_validator.Clone();
            var clone3 = _generator == null ? null : (Func<CustomIterenumerator<T>, T, Tuple<IEnumerable<T>, bool>>)_generator.Clone();
            CustomIterenumerator<T> clone = new CustomIterenumerator<T>(clone1, clone2, clone3);
            return clone;
        }
    }

    public class Range<T>
    {
        public bool IsMinInclusive;
        public T Min;
        public T Max;
        public bool IsMaxInclusive;
    }
}