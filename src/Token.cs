class Token(string TokenType, string Lexeme, object? Literal)
{
	public override string ToString()
	{
		return TokenType + " " + Lexeme + " " + (Literal ?? "null");
	}
}