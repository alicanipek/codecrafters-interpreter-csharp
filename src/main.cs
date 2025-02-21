using System;
using System.IO;
public class Program {
    private static void Main(string[] args) {
        if (args.Length < 2) {
            Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
            Environment.Exit(1);
        }

        string command = args[0];
        string filename = args[1];


        string fileContents = File.ReadAllText(filename);

        if (!string.IsNullOrEmpty(fileContents)) {
            switch (command) {
                case "tokenize": {
                        Tokenizer tokenizer = new(fileContents);
                        // You can use print statements as follows for debugging, they'll be visible when running tests.
                        Console.Error.WriteLine("Logs from your program will appear here!");
                        tokenizer.Tokenize();
                        if (tokenizer.errors.Count > 0) {
                            foreach (string error in tokenizer.errors) {
                                Console.Error.WriteLine(error);
                            }
                        }
                        foreach (Token token in tokenizer.tokens) {
                            Console.WriteLine(token);
                        }
                        Environment.Exit(tokenizer.errorCode);
                        break;
                    }
                case "parse": {
                        Tokenizer tokenizer = new(fileContents);
                        tokenizer.Tokenize();

                        RecursiveParser parser = new(tokenizer.tokens);
                        object result = parser.ParseExpression();
                        if (parser.hadError) {
                            Environment.Exit(65);
                        }
                        Console.WriteLine(result);

                        break;
                    }
                case "evaluate": {
                        Tokenizer tokenizer = new(fileContents);
                        tokenizer.Tokenize();

                        RecursiveParser parser = new(tokenizer.tokens);
                        Expr result = parser.ParseExpression();
                        if (parser.hadError) {
                            Environment.Exit(70);
                        }

                        Evaluator evaluator = new();
                        var value = evaluator.Evaluate(result);
                        if (value == null) {
                            System.Console.WriteLine("nil");
                        }
                        else if (typeof(bool) == value.GetType()) {
                            System.Console.WriteLine(value.ToString().ToLower());
                        }
                        else {
                            System.Console.WriteLine(value);
                        }
                        break;
                    }
                case "run": {
                        Tokenizer tokenizer = new(fileContents);
                        tokenizer.Tokenize();

                        RecursiveParser parser = new(tokenizer.tokens);
                        List<Statement> result = parser.Parse();
                        if (parser.hadError) {
                            Environment.Exit(65);
                        }

                        Evaluator evaluator = new();
                        evaluator.Run(result);

                        break;
                    }
                default:
                    Console.Error.WriteLine("Invalid command.");
                    Environment.Exit(1);
                    break;
            }

        }
        else {
            Console.WriteLine("EOF  null");
            Environment.Exit(0);
        }
    }
}