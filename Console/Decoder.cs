using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console;

internal class Decoder
{
    public Decoder() { }

    public string Decoding(string codeString)
    {
        Dictionary<string, char> keyCodes = new Dictionary<string, char>();

        using (BinaryReader reader = new BinaryReader(File.Open("test.dat", FileMode.Open)))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                char value = reader.ReadChar();
                string key = reader.ReadString();
                keyCodes.Add(key, value);
            }
        }

        string code = "";
        //char? symbol = null;
        //while(codeString[0] != '\n')
        //{
        //    if (codeString[0] == '0' || codeString[0] == '1')
        //    {
        //        code += codeString[0];
        //        codeString = codeString.Remove(0, 1);
        //    }
        //    else
        //    {
        //        if (symbol != null)
        //        {
        //            //keyCodes.Add(code, (char)symbol);
        //        }
        //        code = "";
        //        symbol = codeString[0];
        //        codeString = codeString.Remove(0, 1);
        //    }
        //}
        ////keyCodes.Add(code, (char)symbol);
        //codeString = codeString.Remove(0, 1);

        for (int i = 0; i < keyCodes.Count; i++)
        {
            System.Console.WriteLine($"{keyCodes.ElementAt(i).Value} {keyCodes.ElementAt(i).Key}");
        }

        code = "";
        string returnString = "";

        while (codeString.Length > 0) // Декодирование строки по алфавиту
        {
            code += codeString[0];
            codeString = codeString.Remove(0, 1);
            if (keyCodes.ContainsKey(code))
            {
                returnString += keyCodes[code];
                code = "";
            }
        }

        return returnString;
    }
}
