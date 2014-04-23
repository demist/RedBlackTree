using System;

namespace CSharp.RBTree
{
    class Program
    {
        static void Main()
        {
            var rbtree =  new RedBlackTree<int>();
            var randomizer = new Random();
            Console.WriteLine("Inserting");
            for(var i = 0; i < 10; i++)
                rbtree.Insert(new TreeNode<int>(randomizer.Next()%100));
            Console.WriteLine("Searching");
            for (var i = 0; i < 1; i++)
            {
                Console.WriteLine(rbtree.Search(randomizer.Next()%100));
            }
            Console.WriteLine("Deleting");
            for (var i = 0; i < 1; i++)
            {
                var forDelete = rbtree.SearchNode(randomizer.Next()%100);
                if (forDelete == null) continue;
                rbtree.Delete(forDelete);
                Console.WriteLine("Deleted!");
            }
            Console.WriteLine("Ordered:");
            foreach (var x in rbtree)
                Console.Write(x+" ");
            Console.WriteLine();
            Console.WriteLine("DFS Ordered:");
            var dfs = rbtree.DfsEnum();
            Console.WriteLine("Data\tColor");
            while (dfs.MoveNext())
            {
                Console.WriteLine(dfs.Current.Data+"\t"+dfs.Current.Color);
            }
            Console.WriteLine("BFS Ordered:");
            var bfs = rbtree.BfsEnum();
            Console.WriteLine("Data\tColor");
            while (bfs.MoveNext())
            {
                Console.WriteLine(bfs.Current.Data + "\t" + bfs.Current.Color);
            }
        }
    }
}
