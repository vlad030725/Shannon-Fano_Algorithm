using Console;
using (StreamWriter writer = new StreamWriter(new FileStream("stats.csv", FileMode.Create, FileAccess.Write)))
{
    for (int i = 4; i <= 256; i <<= 1)
    {
        for (int j = 100; j <= 1000; j += 100)
        {
            string path = $"TestFiles\\{i}pwr_{j}kb.txt";
            TimeSpan timeSpan = TimeSpan.Zero;
            double compressionRatio = 0;
            Coder coder = new Coder();
            coder.Coding(path, ref timeSpan, ref compressionRatio);
            writer.WriteLine($"{compressionRatio};{timeSpan};{i};{j}");
        }
    }
}
