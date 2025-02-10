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
    int line = 1;
    for (int i = 0; i < fileContents.Length; i++) {
        char c = fileContents[i];
        Token tk;
        switch (c) {
            case '\n':
                line++;
                break;
            case '\r':
                if (i + 1 < fileContents.Count() && fileContents[i + 1] == '\n') {
                    i++;
                    line++;
                }
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
                if (i + 1 < fileContents.Count() && fileContents[i + 1] == '=') {
                    i++;
                    tk = new("EQUAL_EQUAL", "==", null, line);
                }
                else {
                    tk = new("EQUAL", "=", null, line);
                }
                System.Console.WriteLine(tk);
                break;
            case '!':
                if (i + 1 < fileContents.Count() && fileContents[i + 1] == '=') {
                    i++;
                    tk = new("BANG_EQUAL", "!=", null, line);
                }
                else {
                    tk = new("BANG", "!", null, line);
                }
                System.Console.WriteLine(tk);
                break;
            case '<':
                if (i + 1 < fileContents.Count() && fileContents[i + 1] == '=') {
                    i++;
                    tk = new("LESS_EQUAL", "<=", null, line);
                }
                else {
                    tk = new("LESS", "<", null, line);
                }
                System.Console.WriteLine(tk);
                break;
            case '>':
                if (i + 1 < fileContents.Count() && fileContents[i + 1] == '=') {
                    i++;
                    tk = new("GREATER_EQUAL", ">=", null, line);
                }
                else {
                    tk = new("GREATER", ">", null, line);
                }
                System.Console.WriteLine(tk);
                break;
            case '/':
                if (i + 1 < fileContents.Count() && fileContents[i + 1] == '/') {
                    i += 2; // Skip the initial "//"
                    while (i < fileContents.Length && fileContents[i] != '\n' && 
                        !(fileContents[i] == '\r' && i + 1 < fileContents.Length && fileContents[i + 1] == '\n')) {
                        i++;
                    }
                    i--;
                }
                else {
                    tk = new("SLASH", "/", null, line);
                    System.Console.WriteLine(tk);
                }
                break;
            case '\"':
                int start = i;
                i++;
                while (i < fileContents.Length && fileContents[i] != '"') {
                    if (fileContents[i] == '\n') {
                        line++;
                    }
                    i++;
                }
                if (i >= fileContents.Length) {
                    errors.Add($"[line {line}] Error: Unterminated string.");
                    break;
                }
                string value = fileContents.Substring(start + 1, i - start - 1);
                tk = new("STRING", $"\"{value}\"", value, line);
                System.Console.WriteLine(tk);
                break;
            case char digit when char.IsDigit(digit):
                start = i;
                while (i < fileContents.Length && (char.IsDigit(fileContents[i]) || fileContents[i] == '.')) {
                    i++;
                }
                value = fileContents.Substring(start, i - start);
                tk = new("NUMBER", value, double.Parse(value), line);
                System.Console.WriteLine(tk);
                i--;
                break;
            case '_':
            case char letter when char.IsLetter(letter):
                start = i;
                while (i < fileContents.Length && (char.IsLetterOrDigit(fileContents[i]) || fileContents[i] == '_')) {
                    i++;
                }
                value = fileContents.Substring(start, i - start);
                tk = new("IDENTIFIER", value, null, line);
                System.Console.WriteLine(tk);
                i--;
                break;
            case '\t':
            case ' ': // Ignore whitespace
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
