Random random = new Random();
for (int i = 4; i <= 32768; i <<= 1)
{
    for (int j = 100; j <= 1000; j += 100)
    {
        using (StreamWriter writer = new StreamWriter(new FileStream($"TestFiles\\{i}pwr_{j}kb.txt", FileMode.Create, FileAccess.Write)))
        {
            long countChars = j * 1024 - 1;
            while (writer.BaseStream.Length < countChars)
            {
                writer.Write((char)random.Next(0, i));
            }
        }
    }
}
