namespace Console;

public class Decoder
{
    public Decoder() { }

    public void Decoding(string pathInput, string pathOutput)
    {
        Dictionary<char, int> keyValuePairs = new Dictionary<char, int>();
        using (BinaryWriter writer = new BinaryWriter(new FileStream(pathOutput, FileMode.Create, FileAccess.Write))) { }

        using (BinaryReader reader = new BinaryReader(File.Open(pathInput, FileMode.Open)))
        {
            int count = reader.ReadInt32(); //Мощность алфавита
            for (int i = 0; i < count; i++) //Считывание каждой буквы алфавита и её код
            {
                char key = reader.ReadChar();
                int value = reader.ReadInt32();
                keyValuePairs.Add(key, value); //Добавление пары символ-частота в словарь
            }
            
            Node tree = new Node(); //Построение дерева
            tree.CreateTree(keyValuePairs);

            count = reader.ReadInt32(); //Количество бит которое нужно считать

            using (BinaryWriter writer = new BinaryWriter(new FileStream(pathOutput, FileMode.Open, FileAccess.Write)))
            {
                string stringCode = ""; //Объявление считанных битов
                byte readBits; //Переменная для считывания одного байта файла
                byte w = 0b1000_0000; //Битовая маска

                for (int k = 0; k < count / 8 + 1; k++) //Перебор по количеству байт в файле
                {
                    readBits = reader.ReadByte();

                    if (k == count / 8)
                    {
                        int var = count % 8;
                    }

                    for (int i = 0; i < ((k == count / 8) ? count % 8 : 8); i++)
                    {
                        byte tmpBit = (byte)(readBits & w);
                        if (tmpBit == 0b0000_0000)
                        {
                            stringCode += "0";
                        }
                        else
                        {
                            stringCode += "1";
                        }
                        w >>= 1;
                    }

                    w = 0b1000_0000;

                    int startindex = 0;
                    string tmpCharCode = "";

                    for (int i = 0; i < stringCode.Length; i++)
                    {
                        tmpCharCode += stringCode[i];
                        char? c = tree.SearcCharInTree(tmpCharCode);
                        if (c != null)
                        {
                            writer.Write((char)c);
                            startindex = i + 1;
                            tmpCharCode = "";
                        }
                    }
                    stringCode = stringCode.Substring(startindex);
                }
            }
        }
    }
}
