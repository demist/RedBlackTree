namespace CSharp.RBTree
{
    class TreeNode<T>
    {
        public T Data { get; set; }
        public TreeNode<T> LeftChild { get; set; }
        public TreeNode<T> RightChild { get; set; }
        public TreeNode<T> Parent { get; set; }
        public int Color { get; set; }

        public TreeNode(T data)
        {
            Data = data;
            Parent = null;
            LeftChild = null;
            RightChild = null;
        }
        public TreeNode(T newData, TreeNode<T> parent)
        {
            Data = newData;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        } 
    }
}
