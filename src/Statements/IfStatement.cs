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

	public override void Execute(Evaluator evaluator) {
		object value = Condition.Evaluate(evaluator);
		if (Utils.IsTruthy(value)) {
			ThenBranch.Execute(evaluator);
		}
		else {
			ElseBranch?.Execute(evaluator);
		}
	}
}
