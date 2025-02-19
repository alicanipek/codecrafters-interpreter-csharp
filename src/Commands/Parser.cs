using System.Text;

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
			switch (token.TokenType) {
				case "NUMBER": {
						var literal = double.Parse(token.Lexeme);
						Console.WriteLine(literal % 1 == 0 ? $"{literal:0.0}" : literal.ToString());
						break;
					}
				case "STRING": {
						Console.WriteLine(token.Lexeme.Trim('"'));
						break;
					}
				case "LEFT_PAREN":
					StringBuilder sb = new();
					sb.Append("(group ");
					var leftCount = 1;

					while (leftCount > 0) {
						if (Peek().TokenType == "LEFT_PAREN") {
							sb.Append("(group ");
							leftCount++;
						}
						else if (Peek().TokenType == "RIGHT_PAREN") {
							leftCount--;
							sb.Append(')');
						}
						else {
							sb.Append(Peek().Lexeme.Trim('"'));
						}
						Advance();
					}
					Advance();
					Console.WriteLine(sb.ToString());
					break;
				default:
					Console.WriteLine(token.Lexeme);
					break;
			}

		}
	}
}