public class FunctionStatement : Statement {
	public Token Name;
	public List<Token> Parameters;
	public List<Statement> Body;

	public FunctionStatement(Token name, List<Token> parameters, List<Statement> body) {
		this.Name = name;
		this.Parameters = parameters;
		this.Body = body;
	}

	public override void Execute(Environment environment) {
		Function function = new(this, environment);
		environment.Define(Name.Lexeme, function);
	}
}
