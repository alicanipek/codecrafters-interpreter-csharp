public class PrintStatement : Statement {
	public Expr Expression { get; }

	public PrintStatement(Expr expression) {
		Expression = expression;
	}

	public override string ToString() {
		return $"(print {Expression})";
	}

	public override void Execute(Evaluator evaluator) {
		object value = Expression.Evaluate(evaluator);
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
