public class LogicalExpr : Expr {
	public Expr Left { get; }
	public Token Operator { get; }
	public Expr Right { get; }

	public LogicalExpr(Expr left, Token op, Expr right) {
		Left = left;
		Operator = op;
		Right = right;
	}

    public override object Evaluate(Evaluator evaluator) {
        object left = Left.Evaluate(evaluator);
		if(Operator.TokenType == TokenType.OR) {
			if(Utils.IsTruthy(left)) return left;
		}
		else {
			if(!Utils.IsTruthy(left)) return left;
		}
		return Right.Evaluate(evaluator);
    }
}
