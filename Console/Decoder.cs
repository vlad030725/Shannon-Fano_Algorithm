namespace Console;

public class Decoder
{
    public Decoder() { }

    public void Decoding(string pathInput, string pathOutput)
    {
        byte readBits;
        byte w = 0b1000_0000;
        string tmpStringCode = "";

        Dictionary<string, char> KeyCodes = new Dictionary<string, char>();
        using (BinaryWriter writer = new BinaryWriter(new FileStream(pathOutput, FileMode.Create, FileAccess.Write))) { }

        using (BinaryReader reader = new BinaryReader(File.Open(pathInput, FileMode.Open)))
        {
            int count = reader.ReadInt32(); //Мощность алфавита
            for (int i = 0; i < count; i++) //Считывание каждой буквы алфавита и её код
            {
                char value = reader.ReadChar();
                string key = reader.ReadString();
                KeyCodes.Add(key, value);
            }

            count = reader.ReadInt32(); //Количество бит которое нужно считать

            using (BinaryWriter writer = new BinaryWriter(new FileStream(pathOutput, FileMode.Open, FileAccess.Write)))
            {
                for (int k = 0; k < count / 8 + 1; k++)
                {
                    readBits = reader.ReadByte();

                    for (int i = 0; i < ((k == count / 8) ? count % 8 : 8); i++)
                    {
                        byte tmpBit = (byte)(readBits & w);
                        if (tmpBit == 0b0000_0000)
                        {
                            tmpStringCode += "0";
                        }
                        else
                        {
                            tmpStringCode += "1";
                        }
                        w >>= 1;
                    }

                    w = 0b1000_0000;

                    int startindex = 0;
                    string tmpCharCode = "";

                    for (int i = 0; i < tmpStringCode.Length; i++)
                    {
                        tmpCharCode += tmpStringCode[i];
                        if (KeyCodes.ContainsKey(tmpCharCode))
                        {
                            writer.Write(KeyCodes[tmpCharCode]);
                            startindex = i + 1;
                            tmpCharCode = "";
                        }
                    }
                    tmpStringCode = tmpStringCode.Substring(startindex);
                }
            }
        }
    }
}
