public abstract class Expr {
	public abstract object Evaluate();
}

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
				if (left is string || right is string)
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

	public override object Evaluate() {
		object right = Right.Evaluate();

		switch (Operator.TokenType) {
			case TokenType.MINUS:
				return -(double)right;
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

public class GroupingExpr : Expr {
	public Expr Expression { get; }

	public GroupingExpr(Expr expression) {
		Expression = expression;
	}
	public override string ToString() {
		return $"(group {Expression})";
	}
	public override object Evaluate() {
		return Expression.Evaluate();
	}
}

public class LiteralExpr : Expr {
	public object Value { get; }

	public LiteralExpr(object value) {
		Value = value;
	}

	public override object Evaluate() {
		if (Value != null && double.TryParse(Value.ToString(), out double outValue)) {

			return outValue;
		}
		return Value;
	}

	public override string ToString() {
		if(Value == null) {
			return "nil";
		}
		if(bool.TryParse(Value.ToString(), out bool outValue)) {
			return outValue.ToString().ToLower();
		}
		return $"{Value}";
	}
}

public class Evaluator {
	public object Evaluate(Expr expression) {
		try {
			return expression.Evaluate();
		}
		catch (Exception ex) {
			Console.WriteLine($"Runtime error: {ex.Message}");
			return null;
		}
	}
}