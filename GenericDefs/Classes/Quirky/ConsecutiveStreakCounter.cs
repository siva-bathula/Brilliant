namespace GenericDefs.Classes.Quirky
{
    /// <summary>
    /// Calculates the consecutive streak of positive or negative values or zero values 
    /// returned by evaluating a function or expression for a range of inputs.
    /// </summary>
    public class ConsecutiveStreakCounter
    {
        public long CurrentStreak { get; set; }
        public bool IsNegative { get; set; }
        public bool IsPositive { get; set; }
        public bool IsValueZero { get; set; }

        /// <summary>
        /// Possible roots if evaluating functions.
        /// </summary>
        public int ZeroesOccurred { get; private set; }

        public void Reset()
        {
            ResetToZero(false);
        }

        /// <summary>
        /// -1 indicates negative streak.
        /// +1 indicates positive streak.
        /// 0 indicates current value is zero.
        /// </summary>
        /// <param name="n"></param>
        public void Add(int n)
        {
            if (n < 0)
            {
                IsNegative = true;
                if (IsPositive || IsValueZero) {
                    _flips++;
                    StartStreak(false);
                }
                else if (IsNegative) {
                    Increment();
                }
            } else if (n > 0) {
                IsPositive = true;
                if (IsNegative || IsValueZero) {
                    _flips++;
                    StartStreak(true);
                }
                else if (IsPositive) {
                    Increment();
                }
            } else {
                if(IsNegative || IsPositive) { 
                    ResetToZero(true);
                } else {
                    Increment();
                }
                ZeroesOccurred++;
            }
        }

        private int _flips = 0;
        /// <summary>
        /// Flips from +ve to -ve.
        /// </summary>
        /// <returns></returns>
        public int GetFlips()
        {
            return _flips;
        }

        private object _syncLocker = new object();
        internal void StartStreak(bool isPositive)
        {
            lock (_syncLocker) {
                IsNegative = !isPositive;
                IsPositive = isPositive;
                IsValueZero = false;
                CurrentStreak = 1;
            }
        }
        
        internal void Increment()
        {
            lock (_syncLocker) {
                CurrentStreak++;
                if (IsPositive && CurrentStreak > _maxPositiveStreak) UpdateMaxPositiveStreak();
            }
        }

        private int _maxPositiveStreak = 0;
        internal void UpdateMaxPositiveStreak()
        {
            _maxPositiveStreak++;
        }

        public int GetMaximumPositiveStreak()
        {
            return _maxPositiveStreak;
        }

        internal void ResetToZero(bool isValueZero)
        {
            lock (_syncLocker)
            {
                IsNegative = false;
                IsPositive = false;
                IsValueZero = isValueZero;
                CurrentStreak = 0;
            }
        }
    }
}