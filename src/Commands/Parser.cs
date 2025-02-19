using System.Text;

public class Parser(List<Token> tokens) {
	private int current;

	private bool Check(string type) {
		if (IsAtEnd()) {
			return false;
		}
		return Peek().TokenType == type;
	}

	private bool IsAtEnd() {
		return Peek().TokenType == "EOF";
	}

	private Token Peek() {
		return tokens[current];
	}

	private Token Previous() {
		return tokens[current - 1];
	}

	private Token Advance() {
		if (!IsAtEnd()) {
			current++;
		}

		return Previous();
	}
	private List<string> unaryOperators = new() { "BANG", "MINUS" };
	public void Parse() {
		Stack<string> operands = new();
		Stack<Token> operators = new();
		while (!IsAtEnd()) {
			var token = Advance();
			switch (token.TokenType) {
				case "NUMBER": {
						operands.Push(token.Literal.ToString());
						break;
					}
				case "BANG":
				case "MINUS": {
						ParseUnary(token);
						break;
					}
				case "LEFT_PAREN": {
						ParseGroup();
						break;
					}
				case "PLUS":
				case "STAR":
				case "SLASH":
				case "GREATER":
				case "LESS": {
						while (operators.Count > 0 && HasPrecedence(token, operators.Peek())) {
							ProcessOperator(operands, operators);
						}
						operators.Push(token);
						break;

					}
				default:
					Console.WriteLine(token.Lexeme.Trim('"'));
					break;
			}

		}
		while (operators.Count > 0) {
			ProcessOperator(operands, operators);
		}
		while (operands.Count > 0) {
			Console.WriteLine(operands.Pop());
		}
	}

	private void ParseUnary(Token op) {
		Console.Write("(" + op.Lexeme + " ");
		Token next = Peek();
		if (next.TokenType == "LEFT_PAREN") {
			Advance();
			ParseGroup();
		}
		else if (unaryOperators.Contains(next.TokenType)) {
			Advance();
			ParseUnary(next);
		}
		else {
			Token v = Advance();
			string value = v.TokenType == "NUMBER" || v.TokenType == "STRING" ? v.Literal.ToString() : v.Lexeme;
			Console.Write(value);
		}
		Console.Write(")");
	}

	private void ParseGroup() {
		System.Console.Write("(group ");
		while (!IsAtEnd() && !Check("RIGHT_PAREN")) {
			Token t = Peek();
			if (t.TokenType == "LEFT_PAREN") {
				Advance();
				ParseGroup();
			}
			else if (unaryOperators.Contains(t.TokenType)) {
				Advance();
				ParseUnary(t);
			}
			else {
				Advance();
				string value = t.TokenType == "NUMBER" || t.TokenType == "STRING" ? t.Literal.ToString() : t.Lexeme;
				Console.Write(value);
			}
		}
		if (Check("RIGHT_PAREN")) {
			Advance();
		}
		System.Console.Write(")");
	}

	private bool HasPrecedence(Token current, Token top) {
		if ((current.TokenType == "STAR" || current.TokenType == "SLASH") && (top.TokenType == "PLUS" || top.TokenType == "MINUS")) {
			return false;
		}
		return true;
	}

	private void ProcessOperator(Stack<string> operands, Stack<Token> operators) {
		string right = operands.Pop();
		string left = operands.Pop();
		string op = operators.Pop().Lexeme;
		operands.Push("(" + op + " " + left + " " + right + ")");
	}
}