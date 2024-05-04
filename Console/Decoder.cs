namespace Console;

public class Decoder
{
    public Decoder() { }

    public void Decoding(string pathInput, string pathOutput, ref TimeSpan totalTimeSpan)
    {
        long begin = DateTime.Now.Ticks; //Время начала выполнения
        Dictionary<char, int> keyValuePairs = new Dictionary<char, int>();
        using (BinaryWriter writer = new BinaryWriter(new FileStream(pathOutput, FileMode.Create, FileAccess.Write))) { } //Создание декодированного файла

        using (BinaryReader reader = new BinaryReader(File.Open(pathInput, FileMode.Open))) //Открытие сжатого файла
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
                    readBits = reader.ReadByte(); //Чтение байта информации из закодированного файла

                    for (int i = 0; i < ((k == count / 8) ? count % 8 : 8); i++) //Обход битов байта: 8 бит, кроме последнего байта
                    {
                        byte tmpBit = (byte)(readBits & w); //Определение значение бита
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

                    for (int i = 0; i < stringCode.Length; i++) //Расшифровка имеющегося кода прочитанных битов
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
        long end = DateTime.Now.Ticks; //Время конца выполнения
        totalTimeSpan = new TimeSpan(end - begin); //Итоговое время выполнения
    }
}
