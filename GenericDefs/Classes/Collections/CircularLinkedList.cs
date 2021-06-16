using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace GenericDefs.Classes.Collections
{
    /// <summary>Represents a doubly linked list.</summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    /// <filterpriority>1</filterpriority>
    [Serializable]
    public class CircularLinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
    {
        /// <summary>Enumerates the elements of a <see cref="T:GenericDefs.Classes.Collections.CircularLinkedList`1" />.</summary>
        [Serializable]
        public class Enumerator : IEnumerator<T>, IDisposable, IEnumerator, ISerializable, IDeserializationCallback
        {
            private CircularLinkedList<T> list;

            private LinkedListNode<T> node;

            private T current;

            private int index;

            private SerializationInfo siInfo;

            private const string LinkedListName = "LinkedList";

            private const string CurrentValueName = "Current";

            private const string IndexName = "Index";

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the <see cref="T:System.Collections.Generic.LinkedList`1" /> at the current position of the enumerator.</returns>
            public T Current
            {
                get
                {
                    return current;
                }
            }

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
            object IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == list.Count + 1)
                    {
                        throw new InvalidOperationException("Unable to access Current object.");
                    }
                    return current;
                }
            }

            internal Enumerator(CircularLinkedList<T> list)
            {
                this.list = list;
                node = list.head;
                current = default(T);
                index = 0;
                siInfo = null;
            }

            internal Enumerator(SerializationInfo info, StreamingContext context)
            {
                siInfo = info;
                list = null;
                node = null;
                current = default(T);
                index = 0;
            }

            /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                if (node == null)
                {
                    index = list.Count + 1;
                    return false;
                }
                index++;
                if(index > list.count - 1 || node == list.head)
                {
                    index = 0;
                }
                current = node.item;
                node = node.next;
                //if (node == list.head)
                //{
                //    node = null;
                //}
                return true;
            }

            public bool MoveNext(bool stopAtTail)
            {
                if (node == null)
                {
                    index = list.Count + 1;
                    return false;
                }
                index++;
                current = node.item;
                node = node.next;
                if (node == list.head)
                {
                    node = null;
                }
                return true;
            }

            /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection. This class cannot be inherited.</summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            void IEnumerator.Reset()
            {
                current = default(T);
                node = list.head;
                index = 0;
            }

            /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.LinkedList`1.Enumerator" />.</summary>
            public void Dispose()
            {
            }

            /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</summary>
            /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
            /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="info" /> is null.</exception>
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException("info");
                }
                info.AddValue("LinkedList", list);
                info.AddValue("Current", current);
                info.AddValue("Index", index);
            }

            /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
            /// <param name="sender">The source of the deserialization event.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.LinkedList`1" /> instance is invalid.</exception>
            void IDeserializationCallback.OnDeserialization(object sender)
            {
                if (list != null)
                {
                    return;
                }
                if (siInfo == null)
                {
                    throw new SerializationException("Serialization_InvalidOnDeser");
                }
                list = (CircularLinkedList<T>)siInfo.GetValue("LinkedList", typeof(CircularLinkedList<T>));
                current = (T)(siInfo.GetValue("Current", typeof(T)));
                index = siInfo.GetInt32("Index");
                if (list.siInfo != null)
                {
                    list.OnDeserialization(sender);
                }
                if (index == list.Count + 1)
                {
                    node = null;
                }
                else
                {
                    node = list.First;
                    if (node != null && index != 0)
                    {
                        for (int i = 0; i < index; i++)
                        {
                            node = node.next;
                        }
                        if (node == list.First)
                        {
                            node = null;
                        }
                    }
                }
                siInfo = null;
            }
        }

        internal LinkedListNode<T> head;

        internal int count;

        private object _syncRoot;

        private SerializationInfo siInfo;

        private const string CountName = "Count";

        private const string ValuesName = "Data";

        /// <summary>Gets the number of nodes actually contained in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The number of nodes actually contained in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>Gets the first node of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1" /> of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        public LinkedListNode<T> First
        {
            get
            {
                return head;
            }
            //set
            //{
            //    if (this.head != null)
            //    {
            //        value.prev = this.head.prev;
            //        value.next = this.head;
            //        this.head.prev = value;
            //        this.head = value;
            //    }
            //}
        }

        /// <summary>Gets the last node of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1" /> of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        public LinkedListNode<T> Last
        {
            get
            {
                if (head != null)
                {
                    return head.prev;
                }
                return null;
            }
            //set {
            //    if (this.head != null)
            //    {
            //        this.head.prev = value;
            //        value.next = this.head;
            //    }
            //}
        }
        
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns false.</returns>
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns the current instance.</returns>
        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that is empty.</summary>
        public CircularLinkedList()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that contains elements copied from the specified <see cref="T:System.Collections.IEnumerable" /> and has sufficient capacity to accommodate the number of elements copied. </summary>
        /// <param name="collection">The <see cref="T:System.Collections.IEnumerable" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="collection" /> is null.</exception>
        public CircularLinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            foreach (T current in collection)
            {
                AddLast(current);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that is serializable with the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        protected CircularLinkedList(SerializationInfo info, StreamingContext context)
        {
            siInfo = info;
        }
        
        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }

        /// <summary>Adds a new node containing the specified value after the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
        /// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> after which to insert a new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</param>
        /// <param name="value">The value to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            LinkedListNode<T> linkedListNode = new LinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node.next, linkedListNode);
            return linkedListNode;
        }

        /// <summary>Adds the specified new node after the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> after which to insert <paramref name="newNode" />.</param>
        /// <param name="newNode">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.-or-<paramref name="newNode" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.-or-<paramref name="newNode" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node.next, newNode);
            newNode.list = this;
        }

        /// <summary>Adds a new node containing the specified value before the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
        /// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> before which to insert a new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</param>
        /// <param name="value">The value to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            LinkedListNode<T> linkedListNode = new LinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node, linkedListNode);
            if (node == head)
            {
                head = linkedListNode;
            }
            return linkedListNode;
        }

        /// <summary>Adds the specified new node before the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> before which to insert <paramref name="newNode" />.</param>
        /// <param name="newNode">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.-or-<paramref name="newNode" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.-or-<paramref name="newNode" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node, newNode);
            newNode.list = this;
            if (node == head)
            {
                head = newNode;
            }
        }

        /// <summary>Adds a new node containing the specified value at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
        /// <param name="value">The value to add at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> linkedListNode = new LinkedListNode<T>(this, value);
            if (this.head == null)
            {
                this.InternalInsertNodeToEmptyList(linkedListNode);
            }
            else
            {
                this.InternalInsertNodeBefore(this.head, linkedListNode);
                this.head = linkedListNode;
            }
            return linkedListNode;
        }

        /// <summary>Adds the specified new node at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <param name="node">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public void AddFirst(LinkedListNode<T> node)
        {
            this.ValidateNewNode(node);
            if (this.head == null)
            {
                this.InternalInsertNodeToEmptyList(node);
            }
            else
            {
                this.InternalInsertNodeBefore(this.head, node);
                this.head = node;
            }
            node.list = this;
        }

        /// <summary>Adds a new node containing the specified value at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
        /// <param name="value">The value to add at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> linkedListNode = new LinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(linkedListNode);
            }
            else
            {
                InternalInsertNodeBefore(head, linkedListNode);
            }
            return linkedListNode;
        }

        /// <summary>Adds the specified new node at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <param name="node">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public void AddLast(LinkedListNode<T> node)
        {
            this.ValidateNewNode(node);
            if (this.head == null)
            {
                this.InternalInsertNodeToEmptyList(node);
            }
            else
            {
                this.InternalInsertNodeBefore(this.head, node);
            }
            node.list = this;
        }

        /// <summary>Removes all nodes from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        public void Clear()
        {
            LinkedListNode<T> next = this.head;
            while (next != null)
            {
                LinkedListNode<T> linkedListNode = next;
                next = next.Next;
                linkedListNode.Invalidate();
            }
            head = null;
            count = 0;
        }

        /// <summary>Determines whether a value is in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>true if <paramref name="value" /> is found in the <see cref="T:System.Collections.Generic.LinkedList`1" />; otherwise, false.</returns>
        /// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />. The value can be null for reference types.</param>
        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        /// <summary>Copies the entire <see cref="T:System.Collections.Generic.LinkedList`1" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.LinkedList`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.LinkedList`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index", "IndexOutOfRange");
            }
            if (array.Length - index < Count)
            {
                throw new ArgumentException("Arg_InsufficientSpace");
            }
            LinkedListNode<T> next = head;
            if (next != null)
            {
                do
                {
                    array[index++] = next.item;
                    next = next.next;
                }
                while (next != head);
            }
        }

        /// <summary>Finds the first node that contains the specified value.</summary>
        /// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1" /> that contains the specified value, if found; otherwise, null.</returns>
        /// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> next = head;
            EqualityComparer<T> @default = EqualityComparer<T>.Default;
            if (next != null)
            {
                if (value != null)
                {
                    while (!@default.Equals(next.item, value))
                    {
                        next = next.next;
                        if (next == head)
                        {
                            goto IL_5A;
                        }
                    }
                    return next;
                }
                while (next.item != null)
                {
                    next = next.next;
                    if (next == head)
                    {
                        goto IL_5A;
                    }
                }
                return next;
            }
        IL_5A:
            return null;
        }

        /// <summary>Finds the last node that contains the specified value.</summary>
        /// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1" /> that contains the specified value, if found; otherwise, null.</returns>
        /// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        public LinkedListNode<T> FindLast(T value)
        {
            if (head == null)
            {
                return null;
            }
            LinkedListNode<T> prev = head.prev;
            LinkedListNode<T> linkedListNode = prev;
            EqualityComparer<T> @default = EqualityComparer<T>.Default;
            if (linkedListNode != null)
            {
                if (value != null)
                {
                    while (!@default.Equals(linkedListNode.item, value))
                    {
                        linkedListNode = linkedListNode.prev;
                        if (linkedListNode == prev)
                        {
                            goto IL_61;
                        }
                    }
                    return linkedListNode;
                }
                while (linkedListNode.item != null)
                {
                    linkedListNode = linkedListNode.prev;
                    if (linkedListNode == prev)
                    {
                        goto IL_61;
                    }
                }
                return linkedListNode;
            }
        IL_61:
            return null;
        }

        private Enumerator _instanceEnumerator;
        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.LinkedList`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        public Enumerator GetEnumerator()
        {
            if(_instanceEnumerator == null) _instanceEnumerator = new Enumerator(this);
            return _instanceEnumerator;
        }
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Removes the first occurrence of the specified value from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>true if the element containing <paramref name="value" /> is successfully removed; otherwise, false.  This method also returns false if <paramref name="value" /> was not found in the original <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        /// <param name="value">The value to remove from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        public bool Remove(T value)
        {
            LinkedListNode<T> linkedListNode = Find(value);
            if (linkedListNode != null)
            {
                InternalRemoveNode(linkedListNode);
                return true;
            }
            return false;
        }

        /// <summary>Removes the specified node from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to remove from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="node" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
        public void Remove(LinkedListNode<T> node)
        {
            ValidateNode(node);
            InternalRemoveNode(node);
        }

        /// <summary>Removes the node at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.LinkedList`1" /> is empty.</exception>
        public void RemoveFirst()
        {
            if (head == null)
            {
                throw new InvalidOperationException("LinkedListEmpty");
            }
            InternalRemoveNode(head);
        }

        /// <summary>Removes the node at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.LinkedList`1" /> is empty.</exception>
        public void RemoveLast()
        {
            if (head == null)
            {
                throw new InvalidOperationException("LinkedListEmpty");
            }
            InternalRemoveNode(head.prev);
        }

        /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="info" /> is null.</exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("Count", count);
            if (count != 0)
            {
                T[] array = new T[Count];
                CopyTo(array, 0);
                info.AddValue("Data", array, typeof(T[]));
            }
        }

        /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.LinkedList`1" /> instance is invalid.</exception>
        public virtual void OnDeserialization(object sender)
        {
            if (siInfo == null)
            {
                return;
            }
            int @int = siInfo.GetInt32("Version");
            if (siInfo.GetInt32("Count") != 0)
            {
                T[] array = (T[])siInfo.GetValue("Data", typeof(T[]));
                if (array == null)
                {
                    throw new SerializationException("Serialization_MissingValues");
                }
                for (int i = 0; i < array.Length; i++)
                {
                    AddLast(array[i]);
                }
            }
            else
            {
                head = null;
            }
            siInfo = null;
        }

        private void InternalInsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            count++;
        }

        private void InternalInsertNodeToEmptyList(LinkedListNode<T> newNode)
        {
            newNode.next = newNode;
            newNode.prev = newNode;
            head = newNode;
            count++;
        }

        internal void InternalRemoveNode(LinkedListNode<T> node)
        {
            if (node.next == node)
            {
                head = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (head == node)
                {
                    head = node.next;
                    head.prev = node.prev;
                    node.prev.next = head;
                }
                else if (head.prev == node)
                {
                    head.prev = node.prev;
                    node.prev.next = head;
                }
            }
            node.Invalidate();
            count--;
        }

        internal void ValidateNewNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (node.list != null)
            {
                throw new InvalidOperationException("LinkedListNodeIsAttached");
            }
        }

        internal void ValidateNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (node.list != this)
            {
                throw new InvalidOperationException("ExternalLinkedListNode");
            }
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Rank != 1)
            {
                throw new ArgumentException("Arg_MultiRank");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Arg_NonZeroLowerBound");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "IndexOutOfRange");
            }
            if (array.Length - index < Count)
            {
                throw new ArgumentException("Arg_InsufficientSpace");
            }
            T[] array2 = array as T[];
            if (array2 != null)
            {
                CopyTo(array2, index);
                return;
            }
            Type elementType = array.GetType().GetElementType();
            Type typeFromHandle = typeof(T);
            if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
            {
                throw new ArgumentException("Invalid_Array_Type");
            }
            object[] array3 = array as object[];
            if (array3 == null)
            {
                throw new ArgumentException("Invalid_Array_Type");
            }
            LinkedListNode<T> next = head;
            try
            {
                if (next != null)
                {
                    do
                    {
                        array3[index++] = next.item;
                        next = next.next;
                    }
                    while (next != head);
                }
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid_Array_Type");
            }
        }

        /// <summary>Returns an enumerator that iterates through the linked list as a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the linked list as a collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class LinkedListNode<T>
    {
        internal CircularLinkedList<T> list;

        internal LinkedListNode<T> next;

        internal LinkedListNode<T> prev;

        internal T item;

        /// <summary>Gets the <see cref="T:System.Collections.Generic.LinkedList`1" /> that the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> belongs to.</summary>
        /// <returns>A reference to the <see cref="T:System.Collections.Generic.LinkedList`1" /> that the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> belongs to, or null if the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> is not linked.</returns>
        public CircularLinkedList<T> List
        {
            get
            {
                return list;
            }
        }

        /// <summary>Gets the next node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>A reference to the next node in the <see cref="T:System.Collections.Generic.LinkedList`1" />, or null if the current node is the last element (<see cref="P:System.Collections.Generic.LinkedList`1.Last" />) of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        public LinkedListNode<T> Next
        {
            get
            {
                if (next != null)
                {
                    return next;
                }
                return null;
            }
        }

        /// <summary>Gets the previous node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
        /// <returns>A reference to the previous node in the <see cref="T:System.Collections.Generic.LinkedList`1" />, or null if the current node is the first element (<see cref="P:System.Collections.Generic.LinkedList`1.First" />) of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
        public LinkedListNode<T> Previous
        {
            get
            {
                if (prev != null)
                {
                    return prev;
                }
                return null;
            }
        }

        /// <summary>Gets the value contained in the node.</summary>
        /// <returns>The value contained in the node.</returns>
        public T Value
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> class, containing the specified value.</summary>
        /// <param name="value">The value to contain in the <see cref="T:System.Collections.Generic.LinkedListNode`1" />.</param>
        public LinkedListNode(T value)
        {
            item = value;
        }

        internal LinkedListNode(CircularLinkedList<T> list, T value)
        {
            this.list = list;
            item = value;
        }

        internal void Invalidate()
        {
            list = null;
            next = null;
            prev = null;
        }
    }
}