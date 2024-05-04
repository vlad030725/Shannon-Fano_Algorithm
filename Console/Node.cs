using System.Linq;

namespace Console
{
    public class Node
    {
        public Node() { }
        public Node? LeftNode { get; set; } // левый потомок текущего узла
        public Node? RightNode { get; set; } // правый потомок текущего узла
        public Node? ParentNode { get; set; } // родительский элемент текущего узла
        public char? Item { get; set; } // хранящееся значение в узле дерева

        public Node CreateTree(Dictionary<char, int> keyValuePairs) //Функция создания дерева по словарю символ-частота
        {
            int leftSum = keyValuePairs.ElementAt(0).Value;
            int rightSum = keyValuePairs.Sum(x => x.Value) - leftSum;
            int raznica = Math.Abs(leftSum - rightSum);
            int imin = 0;

            for (int i = 1; i < keyValuePairs.Count - 1; i++) // Разделение символов на две группы приверно равные по сумме частоты встречи
            {
                leftSum += keyValuePairs.ElementAt(i).Value; //Перемещение элемента правой суммы в левую
                rightSum -= keyValuePairs.ElementAt(i).Value;

                if (Math.Abs(leftSum - rightSum) <= raznica) //Перерасчёт разницы
                {
                    raznica = Math.Abs(leftSum - rightSum);
                    imin = i;
                }
                else
                {
                    break;
                }
            }

            Dictionary<char, int> leftKeyValuePairs = new Dictionary<char, int>(); // Инициализация первой группы символов
            for (int i = 0; i <= imin; i++)
            {
                leftKeyValuePairs.Add(keyValuePairs.ElementAt(i).Key, keyValuePairs.ElementAt(i).Value);
            }

            Dictionary<char, int> rightKeyValuePairs = new Dictionary<char, int>(); // Инициализация второй группы символов
            for (int i = imin + 1; i < keyValuePairs.Count; i++)
            {
                rightKeyValuePairs.Add(keyValuePairs.ElementAt(i).Key, keyValuePairs.ElementAt(i).Value);
            }

            if (leftKeyValuePairs.Count > 1) // С новыми группами символов проводится аналогичная операция, если их в группе больше чем 1
            {
                LeftNode = new Node() { ParentNode = this };
                LeftNode.CreateTree(leftKeyValuePairs);
            }
            else
            {
                LeftNode = new Node() { ParentNode = this, Item = leftKeyValuePairs.ElementAt(0).Key };
            }
            if (rightKeyValuePairs.Count > 1)
            {
                RightNode = new Node() { ParentNode = this };
                RightNode.CreateTree(rightKeyValuePairs);
            }
            else
            {
                RightNode = new Node() { ParentNode = this, Item = rightKeyValuePairs.ElementAt(0).Key };
            }

            return this;
        }

        public void SearcCodeInTree(char keyCode, ref string resultCode, string code = "") //Поиск кода по символу
        {
            if (Item == null && resultCode == "")
            {
                LeftNode.SearcCodeInTree(keyCode, ref resultCode, code + "0");
                RightNode.SearcCodeInTree(keyCode, ref resultCode, code + "1");
            }
            else
            {
                if (keyCode == Item)
                {
                    resultCode = code;
                }
            }
        }

        public char? SearcCharInTree(string code) //Поиск символа по коду
        {
            if (code == "")
            {
                return Item;
            }
            else
            {
                if (code[0] == '0')
                {
                    return LeftNode.SearcCharInTree(code[1..]);
                }
                else
                {
                    return RightNode.SearcCharInTree(code[1..]);
                }
            }
        }
    }
}
