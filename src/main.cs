using System;
using System.IO;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
    Environment.Exit(1);
}

string command = args[0];
string filename = args[1];

if (command != "tokenize")
{
    Console.Error.WriteLine($"Unknown command: {command}");
    Environment.Exit(1);
}

string fileContents = File.ReadAllText(filename);
// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.Error.WriteLine("Logs from your program will appear here!");

// Uncomment this block to pass the first stage
if (!string.IsNullOrEmpty(fileContents))
{
    foreach (var c in fileContents)
    {
        switch (c)
        {
            case '(':
                System.Console.WriteLine("LEFT_PAREN ( null");
                break;

            case ')':
                System.Console.WriteLine("RIGHT_PAREN ) null");
                break;
        }
    }

    Console.WriteLine("EOF  null");
}
else
{
    Console.WriteLine("EOF  null");

}
