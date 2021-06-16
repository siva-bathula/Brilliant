using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericDefs.Classes
{
    public class SimpleHashSet<T>
    {
        internal List<SimpleHashString<T>> Sets = new List<SimpleHashString<T>>();
        internal Queue<IList<T>> _queue = new Queue<IList<T>>();

        int _maxLength = 1000000;
        public SimpleHashSet(string delimiter = "#", string separator = "%", int maxLength = 1000000)
        {
            if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("Delimiter cannot be null or empty.");
            _delimiter = delimiter;
            _separator = separator;
            _maxLength = maxLength;
        }

        string _delimiter = "#";
        string _separator = "%";

        SimpleHashString<T> _cur = null;
        public bool Add(IList<T> value)
        {
            if (Sets.Any(x => x.Contains(value))) return false;

            if (_cur == null) { _cur = new SimpleHashString<T>(_delimiter, _separator, _maxLength); Sets.Add(_cur); }
            else if (_cur.MaximumHashAchieved) { _cur = new SimpleHashString<T>(_delimiter, _separator, _maxLength); Sets.Add(_cur); }

            return _cur.Add(value);
        }

        public void AddQueue(IList<T> value)
        {
            _queue.Enqueue(value);
            if (!isExecuting) ProcessQueue();
        }

        public int GetCount()
        {
            return Sets.Sum(x=> x.GetCount());
        }

        public void Clear()
        {
            Sets.ForEach(x => x.Dispose(true));
        }

        bool isExecuting = false;
        private void ProcessQueue()
        {
            if (isExecuting) return;

            isExecuting = true;
            while (_queue.Count > 0) {
                IList<T> val = _queue.Dequeue();
                Add(val);
            }

            isExecuting = false;
        }
    }

    public class SimpleHashString<T>
    {
        string Hash { get; set; }
        public SimpleHashString(string delimiter = "#", string separator = "%", int maxLength = 1000000) {
            if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("Dellimiter cannot be null or empty.");
            _delimiter = delimiter;
            _separator = separator;
            Hash = _delimiter;
            _maxLength = maxLength;
        }
        string _delimiter = "#";
        string _separator = "%";

        public bool Contains(IList<T> value) {
            return Hash.IndexOf(_delimiter + Flatten(value) + _delimiter) >= 0;
        }

        internal bool Contains(string flattenedValue) {
            return Hash.IndexOf(_delimiter + flattenedValue + _delimiter) >= 0;
        }

        public bool Add(IList<T> value) {
            string fVal = Flatten(value);
            if (!Contains(fVal)) {
                lock (_syncCounter) {
                    Hash += fVal + _delimiter;
                    Counter++;
                    if (Counter == _maxLength) MaximumHashAchieved = true;
                }
                return true;
            } else { return false; }
        }

        public bool MaximumHashAchieved { get; set; }

        int _maxLength = 1000000;
        object _syncCounter = new object();
        int Counter { get; set; }

        public int GetCount() { return Counter; }

        public string Flatten(IList<T> list) {
            return string.Join(_separator, list);
        }
        
        private bool disposedValue = false;
        internal virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    Hash = string.Empty;
                    Counter = 0;
                }
                disposedValue = true;
            }
        }
    }
    
    public class SimpleHashString
    {
        string Hash { get; set; }
        public SimpleHashString(string delimiter = "#", int maxLength = 1000000)
        {
            if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("Dellimiter cannot be null or empty.");
            _delimiter = delimiter;
            Hash = _delimiter;
            _maxLength = maxLength;
        }
        string _delimiter = "#";

        public bool Contains(string value)
        {
            return Hash.IndexOf(_delimiter + value + _delimiter) >= 0;
        }

        public bool Add(string value)
        {
            if (!Contains(value))
            {
                lock (_syncCounter)
                {
                    Hash += value + _delimiter;
                    Counter++;
                    if (Counter == _maxLength) MaximumHashAchieved = true;
                }
                return true;
            }
            else { return false; }
        }

        public bool MaximumHashAchieved { get; set; }

        int _maxLength = 1000000;
        object _syncCounter = new object();
        int Counter { get; set; }

        public int GetCount() { return Counter; }

        private bool disposedValue = false;
        internal virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Hash = string.Empty;
                    Counter = 0;
                }
                disposedValue = true;
            }
        }
    }

    public class SimpleHashSet
    {
        internal List<SimpleHashString> Sets = new List<SimpleHashString>();
        internal Queue<string> _queue = new Queue<string>();

        int _maxLength = 1000000;
        public SimpleHashSet(string delimiter = "#", int maxLength = 1000000)
        {
            if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("Delimiter cannot be null or empty.");
            _delimiter = delimiter;
            _maxLength = maxLength;
        }

        string _delimiter = "#";

        SimpleHashString _cur = null;
        public bool Add(string value)
        {
            if (Sets.Any(x => x.Contains(value))) return false;

            if(_cur == null) { _cur = new SimpleHashString(_delimiter, _maxLength); Sets.Add(_cur); }
            else if (_cur.MaximumHashAchieved) { _cur = new SimpleHashString(_delimiter, _maxLength); Sets.Add(_cur); }

            return _cur.Add(value);
        }

        public bool Contains(string value)
        {
            if (Sets.Any(x => x.Contains(value))) return true;
            return false;
        }

        public void AddQueue(string value)
        {
            _queue.Enqueue(value);
            if (!isExecuting) ProcessQueue();
        }

        public int GetCount()
        {
            return Sets.Sum(x => x.GetCount());
        }

        public void Clear()
        {
            Sets.ForEach(x => x.Dispose(true));
        }

        bool isExecuting = false;
        private void ProcessQueue()
        {
            if (isExecuting) return;

            isExecuting = true;
            while (_queue.Count > 0) { Add(_queue.Dequeue()); }

            isExecuting = false;
        }
    }
}