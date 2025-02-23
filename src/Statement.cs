public abstract class Statement {
	public abstract void Execute(Environment environment);
}

public class PrintStatement : Statement {
	public Expr Expression { get; }

	public PrintStatement(Expr expression) {
		Expression = expression;
	}

	public override string ToString() {
		return $"(print {Expression})";
	}

	public override void Execute(Environment environment) {
		object value = Expression.Evaluate(environment);
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

	public override void Execute(Environment environment) {
		Expression.Evaluate(environment);
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

public class VarStatement : Statement {
	public Token Name { get; }
	public Expr Initializer { get; }

	public VarStatement(Token name, Expr initializer) {
		Name = name;
		Initializer = initializer;
	}

	public override string ToString() {
		return $"(var {Name.Lexeme} = {Initializer})";
	}

	public override void Execute(Environment environment) {
		object value = null;
		if (Initializer != null) {
			value = Initializer.Evaluate(environment);
		}
		environment.Define(Name.Lexeme, value);
	}
}