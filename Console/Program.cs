using Console;
using System;
using System.Collections.Generic;

namespace System;
internal class Program
{
    private static void Main(string[] args)
    {
        string InputPath = "test.txt";
        string result;
        
        Coder coder = new Coder();
        string returnString = coder.Coding(InputPath);

        Console.WriteLine(returnString);

        Decoder decoder = new Decoder();
        result = decoder.Decoding(returnString);

        Console.WriteLine(result);
    }
}