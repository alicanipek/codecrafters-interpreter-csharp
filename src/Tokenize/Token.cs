public class Token(string TokenType, string Lexeme, object? Literal, int Line)
{
    public string TokenType { get; } = TokenType;
    public string Lexeme { get; } = Lexeme;
    public object? Literal { get; } = Literal;
    public int Line { get; } = Line;

    public override string ToString()
	{
		string literalStr = Literal switch
		{
			double d => d % 1 == 0 ? $"{d:0.0}" : d.ToString(), // Show .0 for whole numbers, keep decimals untouched
			_ => Literal?.ToString() ?? "null" // Fallback for other types
		};
		return TokenType + " " + Lexeme + " " + literalStr;
	}
}