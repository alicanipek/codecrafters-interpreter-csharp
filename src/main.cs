using System;
using System.IO;

if (args.Length < 2) {
    Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
    Environment.Exit(1);
}

string command = args[0];
string filename = args[1];

if (command != "tokenize") {
    Console.Error.WriteLine($"Unknown command: {command}");
    Environment.Exit(1);
}

string fileContents = File.ReadAllText(filename);
// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.Error.WriteLine("Logs from your program will appear here!");

// Uncomment this block to pass the first stage
if (!string.IsNullOrEmpty(fileContents)) {
    var errors = new List<string>();
    for (int i = 0; i < fileContents.Length; i++) {
        char c = fileContents[i];
        Token tk;
        int line = 1;
        switch (c) {
            case '\n':
                line++;
                break;
            case '(':
                tk = new("LEFT_PAREN", "(", null, line);
                System.Console.WriteLine(tk);
                break;
            case ')':
                tk = new("RIGHT_PAREN", ")", null, line);
                System.Console.WriteLine(tk);
                break;
            case '{':
                tk = new("LEFT_BRACE", "{", null, line);
                System.Console.WriteLine(tk);
                break;
            case '}':
                tk = new("RIGHT_BRACE", "}", null, line);
                System.Console.WriteLine(tk);
                break;
            case ',':
                tk = new("COMMA", ",", null, line);
                System.Console.WriteLine(tk);
                break;
            case '.':
                tk = new("DOT", ".", null, line);
                System.Console.WriteLine(tk);
                break;
            case '-':
                tk = new("MINUS", "-", null, line);
                System.Console.WriteLine(tk);
                break;
            case '+':
                tk = new("PLUS", "+", null, line);
                System.Console.WriteLine(tk);
                break;
            case ';':
                tk = new("SEMICOLON", ";", null, line);
                System.Console.WriteLine(tk);
                break;
            case '*':
                tk = new("STAR", "*", null, line);
                System.Console.WriteLine(tk);
                break;
            case '=':
                if(i+1 < fileContents.Count() &&  fileContents[i+1] == '='){
                    i++;
                    tk = new("EQUAL_EQUAL", "==", null, line);
                }else{
                    tk = new("EQUAL", "=", null, line);
                }
                System.Console.WriteLine(tk);
                break;
            case '!':
                if(i+1 < fileContents.Count() &&  fileContents[i+1] == '='){
                    i++;
                    tk = new("BANG_EQUAL", "!=", null, line);
                }else{
                    tk = new("BANG", "!", null, line);
                }
                System.Console.WriteLine(tk);
                break;
            default:
                errors.Add($"[line {line}] Error: Unexpected character: {c}");
                break;
        }
    }

    Console.WriteLine("EOF  null");
    if (errors.Count > 0) {
        foreach (var error in errors) {
            Console.Error.WriteLine(error);
        }
        Environment.Exit(65);
    }
}
else {
    Console.WriteLine("EOF  null");

}
