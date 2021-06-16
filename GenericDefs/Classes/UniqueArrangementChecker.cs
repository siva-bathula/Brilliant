using System;
using System.Collections.Generic;

namespace GenericDefs.Classes
{
    public class UniqueArrangements<T>
    {
        string _delimiter = string.Empty;
        string _separator = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter">Delimiter between objects.</param>
        /// <param name="seprator">Separator used to flatten a set.</param>
        public UniqueArrangements(string delimiter = "%", string separator = "#")
        {
            _delimiter = delimiter;
            _separator = separator;
        }
        List<FlattenedObjects<T>> _flattenedList = new List<FlattenedObjects<T>>();
        public bool Contains(IList<T> set)
        {
            if (_flattenedList.Count == 0) return false;
            foreach (FlattenedObjects<T> fo in _flattenedList)
            {
                if (fo.Contains(set))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds if unique.
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool Add(IList<T> set)
        {
            if (!Contains(set))
            {
                FlattenedObjects<T> f;
                if (_flattenedList.Count == 0)
                {
                    f = new FlattenedObjects<T>(_delimiter, _separator);
                    _flattenedList.Add(f);
                }
                f = _flattenedList[_flattenedList.Count - 1];
                if (f.MaxLimitHit)
                {
                    f = new FlattenedObjects<T>(_delimiter, _separator);
                    _flattenedList.Add(f);
                }
                return f.AddObject(set);
            }
            return false;
        }

        /// <summary>
        /// Adds if unique.
        /// Use this for concatenating sets. Do not use this for large sets.
        /// (Slow, extract sets from object 2, check if set is found, else add.)
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public void Add(UniqueArrangements<T> uA)
        {
            if (uA == null) throw new ArgumentNullException("UA object 2 cannot be null.");
            if (uA.GetCount() == 0) return;

            List<List<T>> sets = uA.ExtractAsSets();
            foreach(List<T> set in sets) { 
                if (!Contains(set))
                {
                    FlattenedObjects<T> f;
                    if (_flattenedList.Count == 0)
                    {
                        f = new FlattenedObjects<T>(_delimiter, _separator);
                        _flattenedList.Add(f);
                    }
                    f = _flattenedList[_flattenedList.Count - 1];
                    if (f.MaxLimitHit)
                    {
                        f = new FlattenedObjects<T>(_delimiter, _separator);
                        _flattenedList.Add(f);
                    }
                    f.AddObject(set);
                }
            }
        }

        public long GetCount()
        {
            long n = 0;
            foreach (FlattenedObjects<T> f in _flattenedList)
            {
                n += f.Count;
            }
            return n;
        }

        public List<string> Extract()
        {
            List<string> retObj = new List<string>();
            foreach (FlattenedObjects<T> fo in _flattenedList)
            {
                retObj.AddRange(fo.ExtractAsString());
            }

            return retObj;
        }

        public List<List<T>> ExtractAsSets()
        {
            List<List<T>> retObj = new List<List<T>>();
            foreach (FlattenedObjects<T> fo in _flattenedList)
            {
                retObj.AddRange(fo.Extract());
            }

            return retObj;
        }

        public int GetFlatCount()
        {
            return _flattenedList.Count;
        }

        public List<List<T>> GetFlatByIndex(int index)
        {
            List<List<T>> retObj = new List<List<T>>();
            retObj.AddRange(_flattenedList[index].Extract());

            return retObj;
        }

        public List<string> GetFlatByIndexAsString(int index)
        {
            List<string> retObj = new List<string>();
            retObj.AddRange(_flattenedList[index].ExtractAsString());

            return retObj;
        }

        public List<T> ExtractSet(string set)
        {
            return _flattenedList[0].ExtractSet(set);
        }
    }

    /// <summary>
    /// Flattened version of collection of sets.
    /// If {0,1,2} is a set, then 0#1#2 is its flat version.
    /// Collect all such objects to quickly check for duplicate arrangements.
    /// </summary>
    public class FlattenedObjects<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="delimiter">Delimiter between objects.</param>
        /// <param name="seprator">Separator used to flatten a set.</param>
        public FlattenedObjects(string delimiter = "%", string separator = "#", int maxObjects = 100000)
        {
            _delimiter = delimiter;
            _separator = separator;
            this.maxObjects = maxObjects;
            _flatObj = delimiter;
        }
        string _delimiter = string.Empty;
        string _separator = string.Empty;
        string _flatObj = string.Empty;
        public int Count { get; set; }
        int maxObjects;
        internal bool MaxLimitHit = false;
        private object _syncObject = new object();

        private string Flatten(IList<T> set)
        {
            string retValue = string.Empty;
            foreach (T o in set)
            {
                retValue += o + _separator;
            }
            return retValue;
        }

        public bool AddObject(IList<T> set)
        {
            string _flatSet = Flatten(set);
            if (Count < maxObjects) {
                if (Count == 0 || !Contains(_flatSet)) return Add(_flatSet);
            }
            return false;
        }

        /// <summary>
        /// Extracts each set in this flat as a string.
        /// </summary>
        /// <returns></returns>
        public List<string> ExtractAsString()
        {
            List<string> retObject = new List<string>();
            string[] flattenedObjects = _flatObj.Split(_delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in flattenedObjects)
            {
                retObject.Add(string.Join("", s.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));
            }

            return retObject;
        }

        /// <summary>
        /// Extracts each set as a list.
        /// </summary>
        /// <returns></returns>
        public List<List<T>> Extract()
        {
            List<List<T>> retObject = new List<List<T>>();
            string[] flattenedObjects = _flatObj.Split(_delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in flattenedObjects)
            {
                string[] flatSet = s.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<T> set = new List<T>();
                foreach (string fs in flatSet)
                {
                    set.Add((T)Convert.ChangeType(fs, typeof(T)));
                }
                retObject.Add(set);
            }

            return retObject;
        }

        private bool Add(string _flatSet)
        {
            lock (_syncObject)
            {
                if (Count < maxObjects)
                {
                    _flatObj += _flatSet + _delimiter;
                    Count++;
                    if (Count == maxObjects) MaxLimitHit = true;
                    return true;
                }
                return false;
            }
        }

        private bool Contains(string _flatSet)
        {
            return _flatObj.Contains(_delimiter + _flatSet + _delimiter);
        }

        public bool Contains(IList<T> set)
        {
            string _flatSet = Flatten(set);
            return Contains(_flatSet);
        }

        public List<T> ExtractSet(string set)
        {
            string[] flatSet = set.Split(_separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<T> retObj = new List<T>();
            foreach (string fs in flatSet)
            {
                retObj.Add((T)Convert.ChangeType(fs, typeof(T)));
            }
            return retObj;
        }
    }
}