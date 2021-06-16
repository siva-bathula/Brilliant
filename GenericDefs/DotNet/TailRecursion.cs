using System;

namespace GenericDefs.DotNet
{
    /// <summary>
    /// http://www.thomaslevesque.com/2011/09/02/tail-recursion-in-c/
    /// </summary>
    public static class TailRecursion
    {
        public static T Execute<T>(Func<RecursionResult<T>> func)
        {
            do
            {
                var recursionResult = func();
                if (recursionResult.IsFinalResult)
                    return recursionResult.Result;
                func = recursionResult.NextStep;
            } while (true);
        }

        public static RecursionResult<T> Return<T>(T result)
        {
            return new RecursionResult<T>(true, result, null);
        }

        public static RecursionResult<T> Next<T>(Func<RecursionResult<T>> nextStep)
        {
            return new RecursionResult<T>(false, default(T), nextStep);
        }

    }

    public class RecursionResult<T>
    {
        private readonly bool _isFinalResult;
        private readonly T _result;
        private readonly Func<RecursionResult<T>> _nextStep;
        public RecursionResult(bool isFinalResult, T result, Func<RecursionResult<T>> nextStep)
        {
            _isFinalResult = isFinalResult;
            _result = result;
            _nextStep = nextStep;
        }

        public bool IsFinalResult { get { return _isFinalResult; } }
        public T Result { get { return _result; } }
        public Func<RecursionResult<T>> NextStep { get { return _nextStep; } }
    }

    public class OverflowlessStack<T>
    {
        internal sealed class SinglyLinkedNode
        {
            //Larger the better, but we want to be low enough
            //to demonstrate the case where we overflow a node
            //and hence create another.
            private const int ArraySize = 2048;
            T[] _array;
            int _size;
            public SinglyLinkedNode Next;
            public SinglyLinkedNode()
            {
                _array = new T[ArraySize];
            }
            public bool IsEmpty { get { return _size == 0; } }
            public SinglyLinkedNode Push(T item)
            {
                if (_size == ArraySize - 1)
                {
                    SinglyLinkedNode n = new SinglyLinkedNode();
                    n.Next = this;
                    n.Push(item);
                    return n;
                }
                _array[_size++] = item;
                return this;
            }
            public T Pop()
            {
                return _array[--_size];
            }
        }
        private SinglyLinkedNode _head = new SinglyLinkedNode();

        public T Pop()
        {
            T ret = _head.Pop();
            if (_head.IsEmpty && _head.Next != null)
                _head = _head.Next;
            return ret;
        }
        public void Push(T item)
        {
            _head = _head.Push(item);
        }
        public bool IsEmpty
        {
            get { return _head.Next == null && _head.IsEmpty; }
        }
    }
}