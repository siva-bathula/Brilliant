using GenericDefs.Patterns;
using GenericDefs.Util;
using GenericDefs.Classes.NumberTypes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericDefs.Classes.Collections
{
    /// <summary>
    /// An implementation of a Binary Tree data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="T:NGenerics.DataStructures.Trees.BinaryTree`1" />.</typeparam>
    [Serializable]
    public class BinaryTreeNode<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ITree<T>
    {
        private BinaryTreeNode<T> leftSubtree;

        private BinaryTreeNode<T> rightSubtree;

        private T data;

        /// <inheritdoc />
        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

        /// <inheritdoc />
        public bool IsFull
        {
            get
            {
                return this.leftSubtree != null && this.rightSubtree != null;
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                int num = 0;
                if (this.leftSubtree != null)
                {
                    num++;
                }
                if (this.rightSubtree != null)
                {
                    num++;
                }
                return num;
            }
        }

        ITree<T> ITree<T>.Parent
        {
            get
            {
                return this.Parent;
            }
        }

        /// <summary>
        /// Gets the parent of the current node..
        /// </summary>
        /// <value>The parent of the current node.</value>
        public BinaryTreeNode<T> Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the left subtree.
        /// </summary>
        /// <value>The left subtree.</value>
        public virtual BinaryTreeNode<T> Left
        {
            get
            {
                return this.leftSubtree;
            }
            set
            {
                if (this.leftSubtree != null)
                {
                    this.RemoveLeft();
                }
                if (value != null)
                {
                    if (value.Parent != null)
                    {
                        value.Parent.Remove(value);
                    }
                    value.Parent = this;
                }
                this.leftSubtree = value;
            }
        }

        /// <summary>
        /// Gets or sets the right subtree.
        /// </summary>
        /// <value>The right subtree.</value>
        public virtual BinaryTreeNode<T> Right
        {
            get
            {
                return this.rightSubtree;
            }
            set
            {
                if (this.rightSubtree != null)
                {
                    this.RemoveRight();
                }
                if (value != null)
                {
                    if (value.Parent != null)
                    {
                        value.Parent.Remove(value);
                    }
                    value.Parent = this;
                }
                this.rightSubtree = value;
            }
        }

        /// <inheritdoc />
        public virtual T Data
        {
            get
            {
                return this.data;
            }
            set
            {
                Guard.ArgumentNotNull(value, "data");
                this.data = value;
            }
        }

        /// <inheritdoc />
        public int Degree
        {
            get
            {
                return this.Count;
            }
        }

        /// <inheritdoc />
        public virtual int Height
        {
            get
            {
                if (this.Degree == 0)
                {
                    return 0;
                }
                return 1 + this.FindMaximumChildHeight();
            }
        }

        /// <inheritdoc />
        public virtual bool IsLeafNode
        {
            get
            {
                return this.Degree == 0;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:NGenerics.DataStructures.Trees.BinaryTree`1" /> at the specified index.
        /// </summary>
        public BinaryTreeNode<T> this[int index]
        {
            get
            {
                return this.GetChild(index);
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <param name="data">The data contained in this node.</param>
        public BinaryTreeNode(T data) : this(data, null, null)
        {
        }

        /// <param name="data">The data.</param>
        /// <param name="left">The data of the left subtree.</param>
        /// <param name="right">The data of the right subtree.</param>
        public BinaryTreeNode(T data, T left, T right) : this(data, new BinaryTreeNode<T>(left), new BinaryTreeNode<T>(right))
        {
        }

        /// <param name="data">The data contained in this node.</param>
        /// <param name="left">The left subtree.</param>
        /// <param name="right">The right subtree.</param>
        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right) : this(data, left, right, true)
        {
        }

        /// <param name="data">The data contained in this node.</param>
        /// <param name="left">The left subtree.</param>
        /// <param name="right">The right subtree.</param>
        /// <param name="validateData"><see langword="true" /> to validate <paramref name="data" />; otherwise <see langword="false" />.</param>
        internal BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right, bool validateData)
        {
            if (validateData)
            {
                Guard.ArgumentNotNull(data, "data");
            }
            this.leftSubtree = left;
            if (left != null)
            {
                left.Parent = this;
            }
            this.rightSubtree = right;
            if (right != null)
            {
                right.Parent = this;
            }
            this.data = data;
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            foreach (T current in this)
            {
                if (item.Equals(current))
                {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            Guard.ArgumentNotNull(array, "array");
            foreach (T current in this)
            {
                if (arrayIndex >= array.Length)
                {
                    throw new ArgumentException("Not enough space in the target array.", "array");
                }
                array[arrayIndex++] = current;
            }
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            this.AddItem(new BinaryTreeNode<T>(item));
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            if (this.leftSubtree != null && this.leftSubtree.data.Equals(item))
            {
                this.RemoveLeft();
                return true;
            }
            if (this.rightSubtree != null && this.rightSubtree.data.Equals(item))
            {
                this.RemoveRight();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the specified child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns>A value indicating whether the child was found (and removed) from this tree.</returns>
        public bool Remove(BinaryTreeNode<T> child)
        {
            if (this.leftSubtree != null && this.leftSubtree == child)
            {
                this.RemoveLeft();
                return true;
            }
            if (this.rightSubtree != null && this.rightSubtree == child)
            {
                this.RemoveRight();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                BinaryTreeNode<T> binaryTree = stack.Pop();
                yield return binaryTree.Data;
                if (binaryTree.leftSubtree != null)
                {
                    stack.Push(binaryTree.leftSubtree);
                }
                if (binaryTree.rightSubtree != null)
                {
                    stack.Push(binaryTree.rightSubtree);
                }
            }
            yield break;
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            if (this.leftSubtree != null)
            {
                this.leftSubtree.Parent = null;
                this.leftSubtree = null;
            }
            if (this.rightSubtree != null)
            {
                this.rightSubtree.Parent = null;
                this.rightSubtree = null;
            }
        }

        void ITree<T>.Add(ITree<T> child)
        {
            this.AddItem((BinaryTreeNode<T>)child);
        }

        ITree<T> ITree<T>.GetChild(int index)
        {
            return this.GetChild(index);
        }

        bool ITree<T>.Remove(ITree<T> child)
        {
            return this.Remove((BinaryTreeNode<T>)child);
        }

        ITree<T> ITree<T>.FindNode(Predicate<T> condition)
        {
            return this.FindNode(condition);
        }

        /// <summary>
        /// Finds the node with the specified condition.  If a node is not found matching
        /// the specified condition, null is returned.
        /// </summary>
        /// <param name="condition">The condition to test.</param>
        /// <returns>The first node that matches the condition supplied.  If a node is not found, null is returned.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="condition" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        public BinaryTreeNode<T> FindNode(Predicate<T> condition)
        {
            Guard.ArgumentNotNull(condition, "condition");
            if (condition(this.Data))
            {
                return this;
            }
            if (this.leftSubtree != null)
            {
                BinaryTreeNode<T> binaryTree = this.leftSubtree.FindNode(condition);
                if (binaryTree != null)
                {
                    return binaryTree;
                }
            }
            if (this.rightSubtree != null)
            {
                BinaryTreeNode<T> binaryTree2 = this.rightSubtree.FindNode(condition);
                if (binaryTree2 != null)
                {
                    return binaryTree2;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the child at the specified index.
        /// </summary>
        /// <param name="index">The index of the child in question.</param>
        /// <returns>The child at the specified index.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> does not equal 0 or 1.</exception>
        public BinaryTreeNode<T> GetChild(int index)
        {
            switch (index)
            {
                case 0:
                    return this.leftSubtree;
                case 1:
                    return this.rightSubtree;
                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Performs a depth first traversal on this tree with the specified visitor.
        /// </summary>
        /// <param name="orderedVisitor">The ordered visitor.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="orderedVisitor" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        public virtual void DepthFirstTraversal(OrderedVisitor<T> orderedVisitor)
        {
            Guard.ArgumentNotNull(orderedVisitor, "orderedVisitor");
            if (orderedVisitor.HasCompleted)
            {
                return;
            }
            orderedVisitor.VisitPreOrder(this.Data);
            if (this.leftSubtree != null)
            {
                this.leftSubtree.DepthFirstTraversal(orderedVisitor);
            }
            orderedVisitor.VisitInOrder(this.data);
            if (this.rightSubtree != null)
            {
                this.rightSubtree.DepthFirstTraversal(orderedVisitor);
            }
            orderedVisitor.VisitPostOrder(this.Data);
        }

        /// <summary>
        /// Performs a breadth first traversal on this tree with the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitor" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        public virtual void BreadthFirstTraversal(IVisitor<T> visitor)
        {
            Guard.ArgumentNotNull(visitor, "visitor");
            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                if (visitor.HasCompleted)
                {
                    return;
                }
                BinaryTreeNode<T> binaryTree = queue.Dequeue();
                visitor.Visit(binaryTree.Data);
                for (int i = 0; i < binaryTree.Degree; i++)
                {
                    BinaryTreeNode<T> child = binaryTree.GetChild(i);
                    if (child != null)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the left child.
        /// </summary>
        public virtual void RemoveLeft()
        {
            if (this.leftSubtree != null)
            {
                this.leftSubtree.Parent = null;
                this.leftSubtree = null;
            }
        }

        /// <summary>
        /// Removes the left child.
        /// </summary>
        public virtual void RemoveRight()
        {
            if (this.rightSubtree != null)
            {
                this.rightSubtree.Parent = null;
                this.rightSubtree = null;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="subtree">The subtree.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:NGenerics.DataStructures.Trees.BinaryTree`1" /> is full.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="subtree" /> is null (Nothing in Visual Basic).</exception>
        public void Add(BinaryTreeNode<T> subtree)
        {
            Guard.ArgumentNotNull(subtree, "subtree");
            this.AddItem(subtree);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="subtree">The subtree.</param>
        /// <remarks>
        /// 	<b>Notes to Inheritors: </b>
        /// Derived classes can override this method to change the behavior of the <see cref="M:NGenerics.DataStructures.Trees.BinaryTree`1.Clear" /> method.
        /// </remarks>
        protected virtual void AddItem(BinaryTreeNode<T> subtree)
        {
            if (this.leftSubtree == null)
            {
                if (subtree.Parent != null)
                {
                    subtree.Parent.Remove(subtree);
                }
                this.leftSubtree = subtree;
                subtree.Parent = this;
                return;
            }
            if (this.rightSubtree == null)
            {
                if (subtree.Parent != null)
                {
                    subtree.Parent.Remove(subtree);
                }
                this.rightSubtree = subtree;
                subtree.Parent = this;
                return;
            }
            throw new InvalidOperationException("This binary tree is full.");
        }
        

        /// <summary>
        /// Finds the maximum height between the child nodes.
        /// </summary>
        /// <returns>The maximum height of the tree between all paths from this node and all leaf nodes.</returns>
        protected virtual int FindMaximumChildHeight()
        {
            int num = 0;
            int num2 = 0;
            if (this.leftSubtree != null)
            {
                num = this.leftSubtree.Height;
            }
            if (this.rightSubtree != null)
            {
                num2 = this.rightSubtree.Height;
            }
            if (num <= num2)
            {
                return num2;
            }
            return num;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.data.ToString();
        }
    }

    public class BinaryTree<T>
    {
        bool IsSearchTree { get; set; }
        public BinaryTree(bool isSearchTree=false) { IsSearchTree = isSearchTree; }

        public BinaryTree(T rootNodeData, bool isSearchTree = false) {
            InsertNodeInternal(rootNodeData);
            IsSearchTree = isSearchTree;
        }

        public BinaryTree(IList<T> listData, bool isSearchTree = false) {
            InsertNodesInternal(listData);
            IsSearchTree = isSearchTree;
        }

        BinaryTreeNode<T> root;
        public void InsertNode(T data)
        {
            InsertNodeInternal(data);
        }

        public void InsertNodes(IList<T> listData)
        {
            InsertNodesInternal(listData);
        }

        internal void InsertNodesInternal(IList<T> listData)
        {
            foreach (T data in listData) { InsertNodeInternal(data); }
        }

        internal void IncrementNodeCount() {
            _nodeCount++;
            if (!IsSearchTree && _nodeCount == _nextHeightIncrement) {
                _height++;
                _nextHeightIncrement += (int)Math.Pow(2, _height);
            }
        }

        private int _nextHeightIncrement = 1;
        private int _height;
        internal int Height {
            get {
                if (_nodeCount == 0) return 0;
                return _height;
            }
        }
        
        public int GetHeight() {
            if (IsSearchTree)
            {
                return GetSearchTreeHeight(root, 0);
            }
            return _height == 0 ? 0 : _height - 1;
        }

        int GetSearchTreeHeight(BinaryTreeNode<T> node, int height, Number<int> MaxHeight = null)
        {
            if (MaxHeight == null) MaxHeight = new Number<int>(0);
            if(node.Left != null) { GetSearchTreeHeight(node.Left, height + 1, MaxHeight); }
            else { MaxHeight.UpdateMax(height); }
            if (node.Right != null) { GetSearchTreeHeight(node.Right, height + 1, MaxHeight); }
            else { MaxHeight.UpdateMax(height); }

            return MaxHeight.Value;
        }

        private int _nodeCount = 0;
        internal int NodeCount { get { return _nodeCount; } }

        internal BinaryTreeNode<T> GetNodeForInsertion(BinaryTreeNode<T> cNode, int heightLeft, T dataNew)
        {
            BinaryTreeNode<T> retNode = null;
            if (!IsSearchTree)
            {
                if (heightLeft == 1)
                {
                    if (cNode.Left == null) { cNode.Left = new BinaryTreeNode<T>(dataNew); retNode = cNode.Left; retNode.Parent = cNode; }
                    else if (cNode.Right == null) { cNode.Right = new BinaryTreeNode<T>(dataNew); retNode = cNode.Right; retNode.Parent = cNode; }
                } else {
                    retNode = GetNodeForInsertion(cNode.Left, heightLeft - 1, dataNew);
                    if (retNode == null) retNode = GetNodeForInsertion(cNode.Right, heightLeft - 1, dataNew);
                }
            } else {
                bool isLessThan = IsLessThan(cNode.Data, dataNew);
                if (heightLeft == 1)
                {
                    if (isLessThan) {
                        if (cNode.Left == null) {
                            cNode.Left = new BinaryTreeNode<T>(dataNew);
                            retNode = cNode.Left;
                            retNode.Parent = cNode;
                        } else {
                            BinaryTreeNode<T> left = cNode.Left;
                            bool isLessThanLeaf = IsLessThan(left.Data, dataNew);
                            if (isLessThanLeaf) {
                                left.Left = new BinaryTreeNode<T>(dataNew);
                                retNode = left.Left;
                                retNode.Parent = left;
                            } else {
                                left.Right = new BinaryTreeNode<T>(dataNew);
                                retNode = left.Right;
                                retNode.Parent = left;
                            }
                        }
                    } else if (!isLessThan) {
                        if (cNode.Right == null) {
                            cNode.Right = new BinaryTreeNode<T>(dataNew);
                            retNode = cNode.Right;
                            retNode.Parent = cNode;
                        } else {
                            BinaryTreeNode<T> right = cNode.Right;
                            bool isLessThanLeaf = IsLessThan(right.Data, dataNew);
                            if (isLessThanLeaf)
                            {
                                right.Left = new BinaryTreeNode<T>(dataNew);
                                retNode = right.Left;
                                retNode.Parent = right;
                            }
                            else
                            {
                                right.Right = new BinaryTreeNode<T>(dataNew);
                                retNode = right.Right;
                                retNode.Parent = right;
                            }
                        }
                    }
                } else {
                    if (isLessThan) {
                        if (cNode.Left == null)
                        {
                            cNode.Left = new BinaryTreeNode<T>(dataNew);
                            retNode = cNode.Left;
                            retNode.Parent = cNode;
                        } else retNode = GetNodeForInsertion(cNode.Left, heightLeft - 1, dataNew);
                    } else {
                        if (cNode.Right == null)
                        {
                            cNode.Right = new BinaryTreeNode<T>(dataNew);
                            retNode = cNode.Right;
                            retNode.Parent = cNode;
                        } else retNode = GetNodeForInsertion(cNode.Right, heightLeft - 1, dataNew);
                    }
                }
            }

            return retNode;
        }

        bool IsLessThan(T nodeData, T newData)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return (long)Convert.ChangeType(newData, TypeCode.Int64) < (long)Convert.ChangeType(nodeData, TypeCode.Int64);
            }
            else { throw new Exception("Supports only int and long for now."); }
        }

        internal void InsertNodeInternal(T data)
        {
            BinaryTreeNode<T> node = null;
            if (root == null) {
                root = new BinaryTreeNode<T>(data);
            } else {
                int height = Height;
                if (_nodeCount == _nextHeightIncrement) height += 1;
                while (true)
                {
                    node = GetNodeForInsertion(root, height, data);
                    if (node == null) height++;
                    else break;
                }
            }

            IncrementNodeCount();
        }

        /// <summary>
        /// Calculates leaf with least difference with node.
        /// </summary>
        /// <param name="smallestLeafDepth"></param>
        /// <returns></returns>
        public bool? FindLeastLeafDepth(Number<long> smallestLeafDepth)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long)) { FindLeastLeafDepth(root, 0, smallestLeafDepth); }
            return null;
        }

        bool? FindLeastLeafDepth(BinaryTreeNode<T> node, long depth, Number<long> smallestLeafDepth)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                long d = Convert.ToInt64(node.Data);
                if (node.Left != null) { FindLeastLeafDepth(node.Left, depth + d, smallestLeafDepth); }
                if (node.Right != null) { FindLeastLeafDepth(node.Right, depth + d, smallestLeafDepth); }
                if (node.Left == null && node.Right == null) {
                    if (depth < smallestLeafDepth.Value) { smallestLeafDepth.Value = depth + d; }
                }

                return true;
            }

            return null;
        }

        public string GetTreeSignature()
        {
            List<string> TreeStructure = GetTreeStructure(root, 0);
            return string.Join("#!", TreeStructure);
        }

        List<string> GetTreeStructure(BinaryTreeNode<T> node, int height, List<string> structure = null)
        {
            if (structure == null) structure = new List<string>();
            
            if (node.Left != null) {
                structure.Add("*" + height + node.Data.ToString() + "LEFT*");
                GetTreeStructure(node.Left, height + 1, structure);
            }
            if (node.Right != null) {
                structure.Add("*" + height + node.Data.ToString() + "RIGHT*");
                GetTreeStructure(node.Right, height + 1, structure);
            }

            return structure;
        }
    }
}