public class Parser(List<Token> tokens){
    private int _current;

    private bool IsAtEnd()
    {
        return Peek().TokenType == "EOF";
    }

    private Token Peek()
    {
        return tokens[_current];
    }

    private Token Advance()
    {
        if (!IsAtEnd())
        {
            _current++;
        }

        return tokens[_current - 1];
    }

    public void Parse()
    {
        while (!IsAtEnd())
        {
            Console.WriteLine(Advance().Lexeme);
        }
    }
}