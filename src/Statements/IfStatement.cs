public class IfStatement : Statement {
	public Expr Condition { get; }
	public Statement ThenBranch { get; }
	public Statement ElseBranch { get; }

	public IfStatement(Expr condition, Statement thenBranch, Statement elseBranch) {
		Condition = condition;
		ThenBranch = thenBranch;
		ElseBranch = elseBranch;
	}

	public override string ToString() {
		return $"(if {Condition} {ThenBranch} {ElseBranch})";
	}

	public override void Execute(Environment environment) {
		object value = Condition.Evaluate(environment);
		if (Utils.IsTruthy(value)) {
			ThenBranch.Execute(environment);
		}
		else {
			ElseBranch?.Execute(environment);
		}
	}
}
