public class Parser(List<Token> tokens) {
	private int _current;

	private bool IsAtEnd() {
		return Peek().TokenType == "EOF";
	}

	private Token Peek() {
		return tokens[_current];
	}

	private Token Advance() {
		if (!IsAtEnd()) {
			_current++;
		}

		return tokens[_current - 1];
	}

	public void Parse() {
		while (!IsAtEnd()) {
			var token = Advance();
			if (token.TokenType == "NUMBER") {
				var literal = double.Parse(token.Lexeme);
				Console.WriteLine(literal % 1 == 0 ? $"{literal:0.0}" : literal.ToString());
			}
			else {

				Console.WriteLine(Advance().Lexeme);
			}
		}
	}
}