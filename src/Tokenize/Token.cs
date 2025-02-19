using System.Globalization;

public class Token(string TokenType, string Lexeme, Object? Literal, int Line) {
	public string TokenType { get; } = TokenType;
	public string Lexeme { get; } = Lexeme;
	public Object? Literal { get; } = Literal;
	public int Line { get; } = Line;

	public override string ToString() {
		return TokenType + " " + Lexeme + " " + GetLiteralString();
	}

	private string GetLiteralString() {
		return Literal switch {
			null => "null",
			string s => s,
			double d => d.ToString(),
			_ => throw new Exception("Unknown literal type")
		};
	}
}