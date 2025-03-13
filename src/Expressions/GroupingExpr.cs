public class GroupingExpr : Expr {
	public Expr Expression { get; }

	public GroupingExpr(Expr expression) {
		Expression = expression;
	}
	public override string ToString() {
		return $"(group {Expression})";
	}
	public override object Evaluate(Evaluator evaluator) {
		return Expression.Evaluate(evaluator);
	}
}
