using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console;

public class Coder
{
    //private string ResultCode = "";
    public int ProgressValue {  get; private set; }

    public Coder() { }

    public void Coding(string InputPath)
    {
        BinaryReader ReaderInputFile = new BinaryReader(new FileStream(InputPath, FileMode.Open, FileAccess.Read));
        int fileLenght = (int)ReaderInputFile.BaseStream.Length;
        char symbol;
        //string Str = File.ReadAllText(InputPath);
        //int plusBar = Str.Length / 100;

        //Str += '\0';
        Dictionary<char, int> keyValuePairs = new Dictionary<char, int>(); // заполнение словоря буквами из которых состоят входные данные и высчитываем частоту их появления
        for (int i = 0; i < fileLenght; i++)
        {
            //try 
            //{
            //    symbol = ReaderInputFile.ReadChar();
            //}
            //catch
            //{
            //    break;
            //}
            symbol = ReaderInputFile.ReadChar();

            if (keyValuePairs.ContainsKey(symbol))
            {
                keyValuePairs[symbol]++;
            }
            else
            {
                keyValuePairs.Add(symbol, 1);
            }
        }
        keyValuePairs.Add('\0', 1);

        ReaderInputFile.BaseStream.Position = 0;

        keyValuePairs = new Dictionary<char, int>(keyValuePairs.OrderByDescending(i => i.Value)); // Сортируем символы по невозрастанию частот


        Node tree = new Node();

        CreateTree(keyValuePairs, tree);

        Dictionary<char, string> keyCodes = new Dictionary<char, string>();

        

        CreateKeyCodes(tree, keyCodes);
        keyCodes = new Dictionary<char, string>(keyCodes.OrderBy(i => i.Key));

        bool fEnd = false;

        string path = InputPath.Replace(InputPath.Substring(InputPath.LastIndexOf('.')), ".dat");
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) { }

        for (int i = 0; i < keyCodes.Count; i++)
        {
            System.Console.WriteLine($"{keyCodes.ElementAt(i).Key} {keyCodes.ElementAt(i).Value}");
        }

        //string returnString = ""; //Создание закодированной строки

        //for (int i = 0; i < fileLenght; i++)
        //{
        //    symbol = ReaderInputFile.ReadChar();
        //    returnString += keyCodes[symbol];
        //}

        string codeString = "";

        for (int i = 0; i < keyCodes.Count; i++) // Передача алфавита кодов в начале строки
        {
            codeString += keyCodes.ElementAt(i).Key;
            codeString += keyCodes.ElementAt(i).Value;
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Open)))
        {
            writer.Write(keyCodes.Count);
            for (int i = 0; i < keyCodes.Count; i++)
            {
                writer.Write(keyCodes.ElementAt(i).Key);
                writer.Write(keyCodes.ElementAt(i).Value);
            }
        }

        byte w = 0b1000_0000;
        byte writenBits = 0b0000_0000;
        int iterPlusBar = 0;

        while (ReaderInputFile.BaseStream.CanRead)
        {
            //if (iterPlusBar > plusBar)
            //{
            //    iterPlusBar = 0;
            //    ProgressValue++;
            //}
            //else
            //{
            //    iterPlusBar++;
            //}
            //string CodeNowChar = GetKeyCode(Str[0], keyCodes);
            symbol = ReaderInputFile.ReadChar();
            string CodeNowChar = "";
            SearcInTree(symbol, tree, ref CodeNowChar);
            for (int i = 0; i < CodeNowChar.Length; i++)
            {
                if (symbol == '\0' && i + 1 == CodeNowChar.Length)
                {
                    fEnd = true;
                }
                if (CodeNowChar[i] == '1')
                {
                    writenBits = (byte)(writenBits | w);
                }
                w >>= 1;
                if (w == 0b0000_0000 || fEnd)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Open)))
                    {
                        writer.Seek(0, SeekOrigin.End);
                        writer.Write(writenBits);
                    }
                    writenBits = 0b0000_0000;
                    w = 0b1000_0000;

                    if (fEnd)
                    {
                        break;
                    }
                }
            }

            if (fEnd)
            {
                break;
            }
        }

        //return returnString;
    }

    private Node CreateTree(Dictionary<char, int> keyValuePairs, Node parent) // Функция создания дерева
    {
        Node node = null;

        int leftSum = 0;
        int rightSum = 0;
        int raznica = int.MaxValue;
        int imin = 0;

        for (int i = 0; i < keyValuePairs.Count - 1; i++) // Разделение символов на две группы приверно равные по сумме частоты встречи
        {
            for (int j = 0; j <= i; j++)
            {
                leftSum += keyValuePairs.ElementAt(j).Value;
            }
            for (int j = i + 1; j < keyValuePairs.Count; j++)
            {
                rightSum += keyValuePairs.ElementAt(j).Value;
            }
            if (Math.Abs(leftSum - rightSum) <= raznica)
            {
                //System.Console.WriteLine($"{leftSum} {rightSum} {leftSum - rightSum} {i}");
                raznica = Math.Abs(leftSum - rightSum);
                imin = i;
            }
            leftSum = 0;
            rightSum = 0;
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
            parent.LeftNode = new Node() { ParentNode = parent };
            CreateTree(leftKeyValuePairs, parent.LeftNode);
        }
        else
        {
            parent.LeftNode = new Node() { ParentNode = parent, Item = leftKeyValuePairs.ElementAt(0).Key };
        }
        if (rightKeyValuePairs.Count > 1)
        {
            parent.RightNode = new Node() { ParentNode = parent };
            CreateTree(rightKeyValuePairs, parent.RightNode);
        }
        else
        {
            parent.RightNode = new Node() { ParentNode = parent, Item = rightKeyValuePairs.ElementAt(0).Key };
        }

        return node;
    }

    private void CreateKeyCodes(Node node, Dictionary<char, string> keyCodes, string code = "") // Функция создания словоря кодов символов по дереву
    {
        if (node.Item == null)
        {
            if (node.LeftNode != null)
            {
                CreateKeyCodes(node.LeftNode, keyCodes, code + "0");
            }
            if (node.RightNode != null)
            {
                CreateKeyCodes(node.RightNode, keyCodes, code + "1");
            }
        }
        else
        {
            keyCodes.Add((char)node.Item, code);
        }
    }

    //private string GetKeyCode(char keyCode, Dictionary<char, string> keyCodes)
    //{
    //    int a = 0; 
    //    int b = keyCodes.Count;
    //    int n = keyCodes.Count / 2;
    //    while (keyCode != keyCodes.ElementAt(n).Key)
    //    {
    //        if (keyCode < keyCodes.ElementAt(n).Key)
    //        {
    //            b = n;
    //        }
    //        else
    //        {
    //            a = n;
    //        }
    //        n = (a + b) / 2;
    //    }

    //    return keyCodes.ElementAt(n).Value;
    //}

    private void SearcInTree(char keyCode, Node tree, ref string resultCode, string code = "")
    {
        if (tree.Item == null && resultCode == "")
        {
            SearcInTree(keyCode, tree.LeftNode, ref resultCode, code + "0");
            SearcInTree(keyCode, tree.RightNode, ref resultCode, code + "1");
        }
        else
        {
            if (keyCode == tree.Item)
            {
                resultCode = code;
            }
        }
    }
}
