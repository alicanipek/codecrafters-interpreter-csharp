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
				Console.WriteLine(double.Parse(token.Lexeme).ToString("F1"));
			}
			else {

				Console.WriteLine(Advance().Lexeme);
			}
		}
	}
}