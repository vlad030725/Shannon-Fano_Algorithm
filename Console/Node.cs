using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    public class Node
    {
        public Node()
        {

        }
        public Node? LeftNode { get; set; } // левый потомок текущего узла
        public Node? RightNode { get; set; } // правый потомок текущего узла
        public Node? ParentNode { get; set; } // родительский элемент текущего узла
        public char? Item { get; set; } // хранящееся значение в узле дерева
    }
}
