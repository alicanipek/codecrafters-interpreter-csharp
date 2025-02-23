public abstract class Statement {
	public abstract void Execute();
}

public class PrintStatement : Statement {
	public Expr Expression { get; }

	public PrintStatement(Expr expression) {
		Expression = expression;
	}

	public override string ToString() {
		return $"(print {Expression})";
	}

	public override void Execute() {
		object value = Expression.Evaluate();
		if (value == null) {
			Console.WriteLine("nil");
		}
		else if (typeof(bool) == value.GetType()) {
			Console.WriteLine(value.ToString().ToLower());
		}
		else {
			Console.WriteLine(value);
		}
	}
}

public class ExpressionStatement : Statement {
	public Expr Expression { get; }

	public ExpressionStatement(Expr expression) {
		Expression = expression;
	}

	public override string ToString() {
		return Expression.ToString();
	}

	public override void Execute() {
		Expression.Evaluate();
		// if (value == null) {
		// 	Console.WriteLine("nil");
		// }
		// else if (typeof(bool) == value.GetType()) {
		// 	Console.WriteLine(value.ToString().ToLower());
		// }
		// else {
		// 	Console.WriteLine(value);
		// }
	}
}