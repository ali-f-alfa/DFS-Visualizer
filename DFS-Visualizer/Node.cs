using System.Collections.Generic;
using System.Linq;
namespace finall
{

    static class StackHelper
    {

        public static Node PeekOrDefault(this Stack<Node> s)
        {
            return s.Count == 0 ? null : s.Peek();
        }

        public static void PushReverse(this Stack<Node> s, List<Node> list)
        {
            foreach (var l in list.ToArray().Reverse())
            {
                s.Push(l);
            }
        }
    }

    public class Node
    {
        public int Id;
        public List<Node> Children;

        public Node(int id)
        {
            Id = id;
            Children = new List<Node>();
        }


        public void addEdge(Node a)
        {
            Children.Add(a);
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            if (node != null)
            {
                return node.Id == this.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}