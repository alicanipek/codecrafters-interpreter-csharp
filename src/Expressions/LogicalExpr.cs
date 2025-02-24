public class LogicalExpr : Expr {
	public Expr Left { get; }
	public Token Operator { get; }
	public Expr Right { get; }

	public LogicalExpr(Expr left, Token op, Expr right) {
		Left = left;
		Operator = op;
		Right = right;
	}

    public override object Evaluate(Environment environment) {
        object left = Left.Evaluate(environment);
		if(Operator.TokenType == TokenType.OR) {
			if(Utils.IsTruthy(left)) return left;
		}
		else {
			if(!Utils.IsTruthy(left)) return left;
		}
		return Right.Evaluate(environment);
    }
}