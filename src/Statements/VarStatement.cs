public class VarStatement : Statement {
	public Token Name { get; }
	public Expr Initializer { get; }

	public VarStatement(Token name, Expr initializer) {
		Name = name;
		Initializer = initializer;
	}

	public override string ToString() {
		return $"(var {Name.Lexeme} = {Initializer})";
	}

	public override void Execute(Environment environment) {
		object value = null;
		if (Initializer != null) {
			value = Initializer.Evaluate(environment);
		}
		environment.Define(Name.Lexeme, value);
	}
}
