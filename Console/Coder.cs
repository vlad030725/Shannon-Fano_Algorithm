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
            symbol = ReaderInputFile.ReadChar(); //Считывание одного символа

            if (keyValuePairs.ContainsKey(symbol)) //Если символ уже встречался...
            {
                keyValuePairs[symbol]++; //Инкремент значения соответствующему ключу-символу
            }
            else
            {
                keyValuePairs.Add(symbol, 1); //Добавление новой пары в словарь
            }
        }

        ReaderInputFile.BaseStream.Position = 0; //Возвращение в начало файла

        keyValuePairs = new Dictionary<char, int>(keyValuePairs.OrderByDescending(i => i.Value)); // Сортируем символы по невозрастанию частот

        Node tree = new Node(); //Инициализируем дерево
        tree.CreateTree(keyValuePairs); //Собираем дерево по словарю

        string path = InputPath.Replace(InputPath.Substring(InputPath.LastIndexOf('.')), ".vld"); //Создание пути будущего сжатого файла
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) { } //Создание будущего сжатого файла

        long PositionLengthValue = 0; //Объявление переменной позиции числа бит в файле

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Open)))
        {
            writer.Write(keyValuePairs.Count); //Запись числа размера алфавита в файл
            for (int i = 0; i < keyValuePairs.Count; i++) //Перебор алфавита
            {
                writer.Write(keyValuePairs.ElementAt(i).Key); //Запись ключа-символа
                writer.Write(keyValuePairs.ElementAt(i).Value); //Запись частоты его появления
            }
            PositionLengthValue = writer.BaseStream.Position; //Сохранение позиции числа бит в файле
        }

        byte w = 0b1000_0000;
        byte writenBits = 0b0000_0000;

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Open))) 
        {
            writer.Seek(4, SeekOrigin.End); //Отступ 4 байт для будущей записи туда числа записанных битов

            int BitsToReadForDecoder = 0; //Инициализация счётчика записи битов

            while (ReaderInputFile.PeekChar() != -1) //Пока не конец читаемого файла...
            {
                symbol = ReaderInputFile.ReadChar(); //Чтение одного символа
                string CodeNowChar = ""; //Инициализация сторки с кодом символа
                tree.SearcCodeInTree(symbol, ref CodeNowChar); //Запись в строку CodeNowChar код символа symbol
                for (int i = 0; i < CodeNowChar.Length; i++) //Обход строки кода текущего символа
                {
                    if (CodeNowChar[i] == '1') //"Сборка" байта из битов
                    {
                        writenBits = (byte)(writenBits | w);
                    }
                    w >>= 1;
                    BitsToReadForDecoder++; //Подсчёт количиства записанных битов

                    if (w == 0b0000_0000) //Запись в файл "собранного" байта
                    {
                        writer.Write(writenBits);
                        writenBits = 0b0000_0000;
                        w = 0b1000_0000;
                    }
                }
            }
            writer.Write(writenBits); //Обработка "хвостика" (не полный байт)

            writer.BaseStream.Position = PositionLengthValue; //Возвращение в позицию после алфавита для записи количиства записанных битов
            writer.Write(BitsToReadForDecoder);
        }
    }
}
