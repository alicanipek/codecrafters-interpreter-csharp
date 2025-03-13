public class FunctionStatement : Statement {
	public Token Name;
	public List<Token> Parameters;
	public List<Statement> Body;

	public FunctionStatement(Token name, List<Token> parameters, List<Statement> body) {
		Name = name;
		Parameters = parameters;
		Body = body;
	}

	public override void Execute(Evaluator evaluator) {
		Function function = new(this, evaluator._environment);
		evaluator._environment.Define(Name.Lexeme, function);
	}
}
