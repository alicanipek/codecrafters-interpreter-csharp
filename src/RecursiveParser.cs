using System.Globalization;

public class RecursiveParser {
	private readonly List<Token> tokens;
	private int current = 0;
	public bool hadError = false;

	public RecursiveParser(List<Token> tokens) {
		this.tokens = tokens;
	}

	public Expr ParseExpression() {
		try {
			return Expression();
		}
		catch (ParseError) {
			Synchronize(); // Recover from the error
			return null; // Return null in case of error
		}
	} 
	public List<Statement> Parse() {
		try {
			List<Statement> statements = new();
			while (!IsAtEnd()) {
				statements.Add(Declaration());
			}
			return statements;
		}
		catch (ParseError) {
			Synchronize(); // Recover from the error
			return null; // Return null in case of error
		}
	}

	private Statement Declaration(){
		try {
			if (Match(TokenType.VAR)) return VarDeclaration();
			return Statement();
		}
		catch (ParseError) {
			Synchronize(); // Recover from the error
			return null; // Return null in case of error
		}
	}

	private Statement VarDeclaration(){
		Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

		Expr initializer = null;
		if (Match(TokenType.EQUAL)) {
			initializer = Expression();
		}

		Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
		return new VarStatement(name, initializer);
	}

	private Statement Statement() {
		if (Match(TokenType.PRINT)) return PrintStatement();
		if(Match(TokenType.LEFT_BRACE)) return new BlockStatement(Block());
		return ExpressionStatement();
	}

	private List<Statement> Block() {
		List<Statement> statements = new();
		while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd()) {
			statements.Add(Declaration());
		}

		Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
		return statements;
	}

	private Statement PrintStatement() {
		Expr value = Expression();
		Consume(TokenType.SEMICOLON, "Expect ';' after value.");
		return new PrintStatement(value);
	}

	private Statement ExpressionStatement() {
		Expr expr = Expression();
		Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
		return new ExpressionStatement(expr);
	}

	private Expr Expression() {
		return Assignment();
	}

	private Expr Assignment(){
		Expr expr = Equality();
		if(Match(TokenType.EQUAL)){
			Token equals = Previous();
			Expr value = Assignment();

			if(expr is VarExpr varExpr){
				Token name = varExpr.Name;
				return new AssignExpr(name, value);
			}

			Error(equals, "Invalid assignment target.");
		}

		return expr;
	}

	private Expr Equality() {
		Expr expr = Comparison();

		while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL)) {
			Token op = Previous();
			Expr right = Comparison();
			expr = new BinaryExpr(expr, op, right);
		}

		return expr;
	}

	private Expr Comparison() {
		Expr expr = Term();

		while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL)) {
			Token op = Previous();
			Expr right = Term();
			expr = new BinaryExpr(expr, op, right);
		}

		return expr;
	}

	private Expr Term() {
		Expr expr = Factor();

		while (Match(TokenType.MINUS, TokenType.PLUS)) {
			Token op = Previous();
			Expr right = Factor();
			expr = new BinaryExpr(expr, op, right);
		}

		return expr;
	}

	private Expr Factor() {
		Expr expr = Unary();

		while (Match(TokenType.SLASH, TokenType.STAR)) {
			Token op = Previous();
			Expr right = Unary();
			expr = new BinaryExpr(expr, op, right);
		}

		return expr;
	}

	private Expr Unary() {
		if (Match(TokenType.BANG, TokenType.MINUS)) {
			Token op = Previous();
			Expr right = Unary();
			return new UnaryExpr(op, right);
		}

		return Primary();
	}

	private Expr Primary() {
		if (Match(TokenType.FALSE)) return new LiteralExpr(false);
		if (Match(TokenType.TRUE)) return new LiteralExpr(true);
		if (Match(TokenType.NIL)) return new LiteralExpr(null);
		if (Match(TokenType.NUMBER)) {
			var value = Convert.ToDouble(Previous().Literal, CultureInfo.InvariantCulture);
			
			return new LiteralExpr(value);
		}
		if (Match(TokenType.STRING)) {
			return new LiteralExpr(Previous().Literal);
		}

		if(Match(TokenType.IDENTIFIER)){
			return new VarExpr(Previous());
		}

		if (Match(TokenType.LEFT_PAREN)) {
			Expr expr = Expression();
			Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
			return new GroupingExpr(expr);
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
		if (token.TokenType == TokenType.EOF) {
			ReportError(token.Line, " at end", message);
		}
		else {
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