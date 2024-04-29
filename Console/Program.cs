using Console;
using System;
using System.Collections.Generic;

namespace System;
internal class Program
{
    private static void Main(string[] args)
    {
        string InputPath = "fish";
        string result;
        
        //Coder coder = new Coder();
        //string returnString = coder.Coding(InputPath + ".txt");

        //Console.WriteLine(returnString);

        Decoder decoder = new Decoder();
        result = decoder.Decoding(InputPath + ".dat");

        Console.WriteLine(result);
    }
}