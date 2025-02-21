public class BinaryExpr : Expr {
	public Expr Left { get; }
	public Token Operator { get; }
	public Expr Right { get; }

	public BinaryExpr(Expr left, Token op, Expr right) {
		Left = left;
		Operator = op;
		Right = right;
	}
	public override string ToString() {
		return $"({Operator.Lexeme} {Left} {Right})";
	}

	public override object Evaluate() {
		object left = Left.Evaluate();
		object right = Right.Evaluate();

		switch (Operator.TokenType) {
			case TokenType.MINUS:
				return (double)left - (double)right;
			case TokenType.PLUS:
				if (left is double && right is double)
					return (double)left + (double)right;
				if (left is string && right is string)
					return left.ToString() + right.ToString();
				throw new Exception("Operands must be two numbers or two strings.");
			case TokenType.SLASH:
				return (double)left / (double)right;
			case TokenType.STAR:
				return (double)left * (double)right;
			case TokenType.GREATER:
				return (double)left > (double)right;
			case TokenType.GREATER_EQUAL:
				return (double)left >= (double)right;
			case TokenType.LESS:
				return (double)left < (double)right;
			case TokenType.LESS_EQUAL:
				return (double)left <= (double)right;
			case TokenType.BANG_EQUAL:
				return !IsEqual(left, right);
			case TokenType.EQUAL_EQUAL:
				return IsEqual(left, right);
			default:
				throw new Exception($"Unknown operator: {Operator.TokenType}");
		}
	}

	private bool IsEqual(object a, object b) {
		if (a == null && b == null) return true;
		if (a == null) return false;
		return a.Equals(b);
	}
}
