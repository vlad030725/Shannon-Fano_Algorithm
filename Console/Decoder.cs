using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console;

public class Decoder
{
    public Decoder() { }

    public string Decoding(string path)
    {
        byte readBits;
        byte w = 0b1000_0000;
        string tmpStringCode = "";
        string result = "";
        bool f = false;

        Dictionary<string, char> KeyCodes = new Dictionary<string, char>();

        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                char value = reader.ReadChar();
                string key = reader.ReadString();
                KeyCodes.Add(key, value);
            }

            while (!f)
            {
                readBits = reader.ReadByte();

                for (int i = 0; i < 8; i++)
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
                        if (KeyCodes[tmpCharCode] == '\0')
                        {
                            f = true;
                            break;
                        }
                        result += KeyCodes[tmpCharCode];
                        startindex = i + 1;
                        tmpCharCode = "";
                    }
                }
                tmpStringCode = tmpStringCode.Substring(startindex);
            }
        }

        return result;
    }
}
