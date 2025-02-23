public class UnaryExpr : Expr {
	public Token Operator { get; }
	public Expr Right { get; }

	public UnaryExpr(Token op, Expr right) {
		Operator = op;
		Right = right;
	}
	public override string ToString() {
		return $"({Operator.Lexeme} {Right})";
	}

	public override object Evaluate(Environment environment) {
		object right = Right.Evaluate(environment);

		switch (Operator.TokenType) {
			case TokenType.MINUS:
				try
				{
					return -(double)right;
				}
				catch (System.Exception)
				{
					throw new Exception("Operand must be a number.");
				}
			case TokenType.BANG:
				return !IsTruthy(right);
			default:
				throw new Exception($"Unknown operator: {Operator.TokenType}");
		}
	}

	private bool IsTruthy(object value) {
		if (value == null) return false;
		if (value is bool) return (bool)value;
		return true;
	}
}
