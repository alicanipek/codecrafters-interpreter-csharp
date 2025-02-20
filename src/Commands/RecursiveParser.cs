public class RecursiveParser {
	private readonly List<Token> tokens;
	private int current = 0;
	public bool hadError = false;

	public RecursiveParser(List<Token> tokens) {
		this.tokens = tokens;
	}

	public object Parse() {
		try {
			return Expression();
		}
		catch (ParseError) {
			Synchronize(); // Recover from the error
			return null; // Return null in case of error
		}
	}

	private object Expression() {
		return Equality();
	}

	private object Equality() {
		object expr = Comparison();

		while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL)) {
			Token op = Previous();
			object right = Comparison();
			expr = new Binary(expr, op, right);
		}

		return expr;
	}

	private object Comparison() {
		object expr = Term();

		while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL)) {
			Token op = Previous();
			object right = Term();
			expr = new Binary(expr, op, right);
		}

		return expr;
	}

	private object Term() {
		object expr = Factor();

		while (Match(TokenType.MINUS, TokenType.PLUS)) {
			Token op = Previous();
			object right = Factor();
			expr = new Binary(expr, op, right);
		}

		return expr;
	}

	private object Factor() {
		object expr = Unary();

		while (Match(TokenType.SLASH, TokenType.STAR)) {
			Token op = Previous();
			object right = Unary();
			expr = new Binary(expr, op, right);
		}

		return expr;
	}

	private object Unary() {
		if (Match(TokenType.BANG, TokenType.MINUS)) {
			Token op = Previous();
			object right = Unary();
			return new Unary(op, right);
		}

		return Primary();
	}

	private object Primary() {
		if (Match(TokenType.NUMBER, TokenType.STRING, TokenType.TRUE, TokenType.FALSE, TokenType.NIL)) {
			var v = Previous();
			string value = v.TokenType == TokenType.NUMBER || v.TokenType == TokenType.STRING ? v.Literal.ToString() : v.Lexeme;
			return value;
		}

		if (Match(TokenType.LEFT_PAREN)) {
			object expr = Expression();
			Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
			return new Grouping(expr);
		}

		throw Error(Peek(), "Expect expression.");
	}

	private bool Match(params TokenType[] types) {
		foreach (TokenType type in types) {
			if (Check(type)) {
				Advance();
				return true;
			}
		}

		return false;
	}

	private bool Check(TokenType type) {
		if (IsAtEnd()) return false;
		return Peek().TokenType == type;
	}

	private Token Advance() {
		if (!IsAtEnd()) current++;
		return Previous();
	}

	private bool IsAtEnd() {
		return Peek().TokenType == TokenType.EOF;
	}

	private Token Peek() {
		return tokens[current];
	}

	private Token Previous() {
		return tokens[current - 1];
	}

	private Token Consume(TokenType type, string message) {
		if (Check(type)) return Advance();
		throw Error(Peek(), message);
	}

	private ParseError Error(Token token, string message) {
		if(token.TokenType == TokenType.EOF) {
			ReportError(token.Line, " at end", message);
		} else {
			ReportError(token.Line, $" at '{token.Lexeme}'", message);
		}
		return new ParseError(message);
	}

	private void ReportError(int line, string at, string message) {

		Console.Error.WriteLine($"[line {line}] Error{at}: {message}");
		hadError = true;

	}

	private void Synchronize() {
		if (!IsAtEnd()) Advance();

		while (!IsAtEnd()) {
			if (Previous().TokenType == TokenType.SEMICOLON) return;

			switch (Peek().TokenType) {
				case TokenType.CLASS:
				case TokenType.FUN:
				case TokenType.VAR:
				case TokenType.FOR:
				case TokenType.IF:
				case TokenType.WHILE:
				case TokenType.PRINT:
				case TokenType.RETURN:
					return;
			}

			Advance();
		}
	}
}

public class ParseError : Exception {
	public ParseError(string message) : base(message) { }
}

public class Binary {
	public object Left { get; }
	public Token Operator { get; }
	public object Right { get; }

	public Binary(object left, Token op, object right) {
		Left = left;
		Operator = op;
		Right = right;
	}
	public override string ToString() {
		return $"({Operator.Lexeme} {Left} {Right})";
	}
}

public class Unary {
	public Token Operator { get; }
	public object Right { get; }

	public Unary(Token op, object right) {
		Operator = op;
		Right = right;
	}

	public override string ToString() {
		return $"({Operator.Lexeme} {Right})";
	}
}

public class Grouping {
	public object Expression { get; }

	public Grouping(object expression) {
		Expression = expression;
	}


	public override string ToString() {
		return $"(group {Expression})";
	}
}
