public class WhileStatement : Statement {
	private Expr condition;
	private Statement body;

	public WhileStatement(Expr condition, Statement body) {
		this.condition = condition;
		this.body = body;
	}

	public override void Execute(Environment environment) {
		while (Utils.IsTruthy(condition.Evaluate(environment))) {
			body.Execute(environment);
		}
	}
}
