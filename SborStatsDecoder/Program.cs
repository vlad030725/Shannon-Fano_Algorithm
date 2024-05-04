using Console;
using (StreamWriter writer = new StreamWriter(new FileStream("statsDecoding.csv", FileMode.Create, FileAccess.Write)))
{
    for (int i = 4; i <= 128; i <<= 1)
    {
        for (int j = 100; j <= 1000; j += 100)
        {
            string path = $"TestFiles\\{i}pwr_{j}kb.vld";
            TimeSpan timeSpan = TimeSpan.Zero;
            Decoder decoder = new Decoder();
            decoder.Decoding(path, $"TestFiles\\{i}pwr_{j}kb_DECODE.txt", ref timeSpan);
            writer.WriteLine($"{timeSpan.TotalSeconds};{i};{j}");
        }
    }
}