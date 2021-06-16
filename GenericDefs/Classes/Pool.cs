using System;
using System.Collections.Generic;
using System.Linq;
using GenericDefs.DotNet;

namespace GenericDefs.Classes
{
    public sealed class Pool<T>
    {
        private readonly Func<Pool<T>, Pool<T>> initializer;

        public bool RangedPool { get; set; }
        public T MaxValue { set; get; }
        public T MinValue { set; get; }

        private readonly List<T> bag;
        private Random _r;

        public Pool(Func<Pool<T>, Pool<T>> initializer)
        {
            if (initializer == null)
                throw new ArgumentNullException("initializer");

            this.bag = new List<T>();
            _r = new Random();
            this.initializer = initializer;
            this.initializer.Invoke(this);
        }

        public void Push(T item)
        {
            this.bag.Add(item);
        }

        public void AddRange(List<T> range)
        {
            this.bag.AddRange(range);
        }

        public IEnumerable<T> Pop(int k)
        {
            for (int t = 1; t <= k; t++)
            {
                yield return Pop();
            }
        }

        public T Pop()
        {
            T item = this.bag[this.GetRandomIndex()];
            this.bag.Remove(item);
            return item;
        }
        
        /// <summary>
        /// For removing an item which is not p.
        /// </summary>
        /// <param name="p">The removed item cannot be equal to p.</param>
        /// <returns></returns>
        public T Pop(T p) {
            T item;
            do
            {
                item = this.bag[this.GetRandomIndex()];
            } while (item.Equals(p));

            this.bag.Remove(item);
            return item;
        }

        private int GetRandomIndex() {
            if (this.bag.Count == 1) return 0;
            return (_r).Next(this.bag.Count);
        }

        public void ReInitialize() {
            this.bag.Clear();
            this.initializer.Invoke(this);
        }

        public bool IsEmpty() {
            return this.bag.Count == 0;
        }
    }

    public sealed class PoolRandomArrayGenerator<T>
    {
        private readonly Func<List<T>> initializer;

        private readonly List<T> bag;
        private Random _r;
        public PoolRandomArrayGenerator(Func<List<T>> initializer)
        {
            if (initializer == null) throw new ArgumentNullException("initializer");

            this.initializer = initializer;
            bag = new List<T>(this.initializer.Invoke());
            _r = new Random();
        }

        public List<T> GetNewList()
        {
            return new List<T>(GetNewArray());
        }

        private T Pop()
        {
            T item = bag[GetRandomIndex()];
            bag.Remove(item);
            return item;
        }

        public T[] GetNewArray()
        {
            if (IsEmpty()) ReInitialize();

            T[] array = new T[bag.Count];
            int index = 0;
            while (!IsEmpty())
            {
                array[index] = Pop();
                index++;
            }

            return array;
        }

        private int GetRandomIndex()
        {
            if (bag.Count == 1) return 0;
            return (_r).Next(bag.Count);
        }

        public void ReInitialize()
        {
            bag.Clear();
            bag.AddRange(initializer.Invoke());
        }

        public bool IsEmpty()
        {
            return bag == null || bag.Count == 0;
        }
    }
}