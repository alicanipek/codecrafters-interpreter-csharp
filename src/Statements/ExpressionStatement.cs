public class ExpressionStatement : Statement {
	public Expr Expression { get; }

	public ExpressionStatement(Expr expression) {
		Expression = expression;
	}

	public override string ToString() {
		return Expression.ToString();
	}

	public override void Execute(Environment environment) {
		Expression.Evaluate(environment);
	}
}
