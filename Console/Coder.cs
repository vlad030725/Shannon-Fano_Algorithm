namespace Console;

public class Coder
{
    public Coder() { }

    public void Coding(string InputPath)
    {
        BinaryReader ReaderInputFile = new BinaryReader(new FileStream(InputPath, FileMode.Open, FileAccess.Read));
        char symbol;
        Dictionary<char, int> keyValuePairs = new Dictionary<char, int>(); // заполнение словоря буквами из которых состоят входные данные и высчитываем частоту их появления
        while (ReaderInputFile.PeekChar() != -1) //Пока недостигнут конец файла
        {
            symbol = ReaderInputFile.ReadChar(); //Считывание символа

            if (keyValuePairs.ContainsKey(symbol))
            {
                keyValuePairs[symbol]++;
            }
            else
            {
                keyValuePairs.Add(symbol, 1);
            }
        }

        ReaderInputFile.BaseStream.Position = 0;

        keyValuePairs = new Dictionary<char, int>(keyValuePairs.OrderByDescending(i => i.Value)); // Сортируем символы по невозрастанию частот


        Node tree = new Node();

        CreateTree(keyValuePairs, tree);

        Dictionary<char, string> keyCodes = new Dictionary<char, string>(); // Создание словаря с кодами букв
        CreateKeyCodes(tree, keyCodes);
        keyCodes = new Dictionary<char, string>(keyCodes.OrderBy(i => i.Key));

        string path = InputPath.Replace(InputPath.Substring(InputPath.LastIndexOf('.')), ".vld");
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) { }

        for (int i = 0; i < keyCodes.Count; i++)
        {
            System.Console.WriteLine($"{keyCodes.ElementAt(i).Key} {keyCodes.ElementAt(i).Value}");
        }

        string codeString = "";

        for (int i = 0; i < keyCodes.Count; i++) // Передача алфавита кодов в начале строки
        {
            codeString += keyCodes.ElementAt(i).Key;
            codeString += keyCodes.ElementAt(i).Value;
        }

        long PositionLengthValue = 0;

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Open)))
        {
            writer.Write(keyCodes.Count);
            for (int i = 0; i < keyCodes.Count; i++)
            {
                writer.Write(keyCodes.ElementAt(i).Key);
                writer.Write(keyCodes.ElementAt(i).Value);
            }
            PositionLengthValue = writer.BaseStream.Position;
        }

        byte w = 0b1000_0000;
        byte writenBits = 0b0000_0000;

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Open)))
        {
            writer.Seek(4, SeekOrigin.End);

            int BitsToReadForDecoder = 0;

            while (ReaderInputFile.PeekChar() != -1)
            {
                symbol = ReaderInputFile.ReadChar();
                string CodeNowChar = "";
                SearcInTree(symbol, tree, ref CodeNowChar);
                for (int i = 0; i < CodeNowChar.Length; i++)
                {
                    if (CodeNowChar[i] == '1')
                    {
                        writenBits = (byte)(writenBits | w);
                    }
                    w >>= 1;
                    BitsToReadForDecoder++;

                    if (w == 0b0000_0000 || ReaderInputFile.PeekChar() == -1)
                    {
                        writer.Write(writenBits);
                        writenBits = 0b0000_0000;
                        w = 0b1000_0000;
                    }
                }
            }
            writer.BaseStream.Position = PositionLengthValue;
            writer.Write(BitsToReadForDecoder);
        }
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
