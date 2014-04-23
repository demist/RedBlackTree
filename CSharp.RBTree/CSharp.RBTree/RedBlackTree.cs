using System;
using System.Collections.Generic;

namespace CSharp.RBTree
{
    internal class RedBlackTree<T> where T : IComparable<T>, IEquatable<T>, new()
    {
        public TreeNode<T> Root { get; private set; }
        public int Size { get; private set; }

        public RedBlackTree()
        {
            Root = null;
            Size = 0;
        }

        private static TreeNode<T> Add(TreeNode<T> to, TreeNode<T> newNode)
        {
            if (newNode.Data.CompareTo(to.Data) < 0)
            {
                if (to.LeftChild != null) return Add(to.LeftChild, newNode);
                newNode.LeftChild = null;
                newNode.RightChild = null;
                to.LeftChild = newNode;
                newNode.Color = 1;
                newNode.Parent = to;
                return newNode;
            }
            if (to.RightChild != null) return Add(to.RightChild, newNode);
            newNode.LeftChild = null;
            newNode.RightChild = null;
            to.RightChild = newNode;
            newNode.Color = 1;
            newNode.Parent = to;
            return newNode;
        }

        private void LeftTurn(TreeNode<T> node)
        {
            if (node.RightChild == null) return;
            var child = node.RightChild;
            node.RightChild = child.LeftChild;
            if (child.LeftChild != null) child.LeftChild.Parent = node;
            child.Parent = node.Parent;
            if (node.Parent == null) Root = child;
            else
            {
                if (node == node.Parent.LeftChild)
                    node.Parent.LeftChild = child;
                else
                    node.Parent.RightChild = child;
            }
            child.LeftChild = node;
            node.Parent = child;
        }

        private void RightTurn(TreeNode<T> node)
        {
            if (node.LeftChild == null) return;
            var child = node.LeftChild;
            node.LeftChild = child.RightChild;
            if (child.RightChild != null) child.RightChild.Parent = node;
            child.Parent = node.Parent;
            if (node.Parent == null) Root = child;
            else
            {
                if (node == node.Parent.RightChild) node.Parent.RightChild = child;
                else node.Parent.LeftChild = child;
            }
            child.RightChild = node;
            node.Parent = child;
        }

        public void Insert(TreeNode<T> node)
        {
            if (Root == null)
            {
                Root = node;
                Root.Color = 0;
                Root.LeftChild = null;
                Root.RightChild = null;
                Root.Parent = null;
            }
            else
            {
                var addedNode = Add(Root, node);
                while (addedNode != Root && addedNode.Parent.Color == 1)
                {
                    if (addedNode.Parent == addedNode.Parent.Parent.LeftChild)
                    {
                        var y = addedNode.Parent.Parent.RightChild;
                        if (y != null && y.Color == 1)
                        {
                            addedNode.Parent.Color = 0;
                            y.Color = 0;
                            addedNode.Parent.Parent.Color = 1;
                            addedNode = addedNode.Parent.Parent;
                        }
                        else
                        {
                            if (addedNode == addedNode.Parent.RightChild)
                            {
                                addedNode = addedNode.Parent;
                                LeftTurn(addedNode);
                            }
                            addedNode.Parent.Color = 0;
                            addedNode.Parent.Parent.Color = 1;
                            RightTurn(addedNode.Parent.Parent);
                        }
                    }
                    else
                    {
                        var y = addedNode.Parent.Parent.LeftChild;
                        if (y != null && y.Color == 1)
                        {
                            addedNode.Parent.Color = 0;
                            y.Color = 0;
                            addedNode.Parent.Parent.Color = 1;
                            addedNode = addedNode.Parent.Parent;
                        }
                        else
                        {
                            if (addedNode == addedNode.Parent.Parent.LeftChild)
                            {
                                addedNode = addedNode.Parent;
                                RightTurn(addedNode);
                            }
                            addedNode.Parent.Color = 0;
                            addedNode.Parent.Parent.Color = 1;
                            LeftTurn(addedNode.Parent.Parent);
                        }
                    }
                }
            }
            Root.Color = 0;
        }

        private static TreeNode<T> Min(TreeNode<T> node)
        {
            while (node.LeftChild != null) node = node.LeftChild;
            return node;
        }

        private static TreeNode<T> Next(TreeNode<T> node)
        {
            if (node.RightChild != null) return Min(node.RightChild);
            var y = node.Parent;
            while (y != null && node == y.RightChild)
            {
                node = y;
                y = y.Parent;
            }
            return y;
        }

        private void FixUp(TreeNode<T> node)
        {
            while (node != Root && node.Color == 0)
            {
                if (node == node.Parent.LeftChild)
                {
                    var w = node.Parent.RightChild;
                    if (w.Color == 1)
                    {
                        w.Color = 0;
                        node.Parent.Color = 1;
                        LeftTurn(node.Parent);
                        w = node.Parent.RightChild;
                    }
                    if (w.LeftChild.Color == 0 && w.RightChild.Color == 0)
                    {
                        w.Color = 1;
                        node = node.Parent;
                    }
                    else
                    {
                        if (w.RightChild.Color == 0)
                        {
                            w.LeftChild.Color = 0;
                            w.Color = 1;
                            RightTurn(w);
                            w = node.Parent.RightChild;
                        }
                        w.Color = node.Parent.Color;
                        node.Parent.Color = 0;
                        w.RightChild.Color = 0;
                        LeftTurn(node.Parent);
                        node = Root;
                    }
                }
                else
                {
                    var w = node.Parent.LeftChild;
                    if (w.Color == 1)
                    {
                        w.Color = 0;
                        node.Parent.Color = 1;
                        RightTurn(node.Parent);
                        w = node.Parent.LeftChild;
                    }
                    if (w.RightChild.Color == 0 && w.LeftChild.Color == 0)
                    {
                        w.Color = 1;
                        node = node.Parent;
                    }
                    else
                    {
                        if (w.LeftChild.Color == 0)
                        {
                            w.RightChild.Color = 0;
                            w.Color = 1;
                            LeftTurn(w);
                            w = node.Parent.LeftChild;
                        }
                        w.Color = node.Parent.Color;
                        node.Parent.Color = 0;
                        w.LeftChild.Color = 0;
                        RightTurn(node.Parent);
                        node = Root;
                    }
                }
            }
            node.Color = 0;
        }
        public void Delete(TreeNode<T> node)
        {
            TreeNode<T> y;
            if (node.LeftChild == null || node.RightChild == null)
                y = node;
            else
                y = Next(node);
            var x = y.LeftChild ?? y.RightChild;
            if (x == null)
            {
                node.Data = y.Data;
                if (y.Parent == null) return;
                if (y.Parent.LeftChild == y) y.Parent.LeftChild = null;
                else y.Parent.RightChild = null;
                return;
            }
            x.Parent = y.Parent;
            if (y.Parent == null) Root = x;
            else
            {
                if (y == y.Parent.LeftChild) y.Parent.LeftChild = x;
                else y.Parent.RightChild = x;
            }
            if (y != node)
            {
                node.Data = y.Data;
            }
            if (y.Color == 0) FixUp(x);
        }

        private static TreeNode<T> SearchInSubTree(TreeNode<T> topNode,T data)
        {
            if (data.Equals(topNode.Data))
                return topNode;
            if (data.CompareTo(topNode.Data) < 0 && topNode.LeftChild != null)
                return SearchInSubTree(topNode.LeftChild, data);
            if(data.CompareTo(topNode.Data) > 0 && topNode.RightChild != null)
                return SearchInSubTree(topNode.RightChild, data);
            return null;
        }

        public bool Search(T data)
        {
            return SearchInSubTree(Root, data) != null;
        }

        public TreeNode<T> SearchNode(T data)
        {
            return SearchInSubTree(Root, data);
        }

        public IEnumerator<T> GetEnumerator()
        {
            if(Root==null)
                yield break;
            var current = Min(Root);
            yield return current.Data;
            while (Next(current) != null)
            {
                current = Next(current);
                yield return current.Data;
            }
        }

        public IEnumerator<TreeNode<T>> DfsEnum()
        {
            var verts = new Stack<TreeNode<T>>();
            if(Root == null)
                yield break;
            verts.Push(Root);
            var previous = 0;
            while (verts.Count != 0)
            {
                var current = verts.Peek();
                if (current.LeftChild == null && current.RightChild == null)
                {
                    verts.Pop();
                    yield return current;
                    if (current.Parent != null)
                        previous = current == current.Parent.LeftChild ? 1 : 2;
                    else
                        yield break;
                    continue;
                }
                switch (previous)
                {
                    case 0:
                        if (current.LeftChild == null)
                        {
                            previous = 1;
                            continue;
                        }
                        verts.Push(current.LeftChild);
                        previous = 0;
                        break;
                    case 1:
                        if (current.RightChild == null)
                        {
                            verts.Pop();
                            yield return current;
                            if (current.Parent != null)
                            {
                                previous = current == current.Parent.LeftChild ? 1 : 2;
                                continue;
                            }
                            yield break;
                }
                        verts.Push(current.RightChild);
                        previous = 0;
                        break;
                    case 2:
                        verts.Pop();
                        yield return current;
                        if (current.Parent != null)
                            previous = current == current.Parent.LeftChild ? 1 : 2;
                        else
                            yield break;
                        break;
                }
            }
        }

        public IEnumerator<TreeNode<T>> BfsEnum()
        {
            var verts = new Queue<TreeNode<T>>();
            verts.Enqueue(Root);
            while (verts.Count != 0)
            {
                var current = verts.Dequeue();
                yield return current;
                if(current.LeftChild != null)
                    verts.Enqueue(current.LeftChild);
                if(current.RightChild != null)
                    verts.Enqueue(current.RightChild);
            }
        } 
    }
}