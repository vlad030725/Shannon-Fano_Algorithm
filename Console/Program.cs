using Console;
using System;
using System.Collections.Generic;

namespace System;
internal class Program
{
    private static void Main(string[] args)
    {
        string codeString = "АБРАКАДАБРА";
        
        Coder coder = new Coder();
        string returnString = coder.Coding(codeString);

        Console.WriteLine(returnString);

        Decoder decoder = new Decoder();
        codeString = decoder.Decoding(returnString);

        Console.WriteLine(codeString);
    }
}