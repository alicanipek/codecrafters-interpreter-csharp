using System.Globalization;

public class Token(string TokenType, string Lexeme, object? Literal, int Line) {
	public string TokenType { get; } = TokenType;
	public string Lexeme { get; } = Lexeme;
	public object? Literal { get; } = Literal;
	public int Line { get; } = Line;

	public override string ToString() {
		return TokenType + " " + Lexeme + " " + GetLiteralString();
	}

	private string GetLiteralString() {
		return Literal switch {
			null => "null",
			string s => s,
			double d => d % 1 == 0 ? $"{d:0.0}" : d.ToString(),
			_ => throw new Exception("Unknown literal type")
		};
	}
}