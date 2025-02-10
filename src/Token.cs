class Token(string TokenType, string Lexeme, object? Literal, int Line)
{
	public override string ToString()
	{
		return TokenType + " " + Lexeme + " " + (Literal ?? "null");
	}
}