using System.Text;

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

	public override void Execute(Environment environment) {
		object value = Condition.Evaluate(environment);
		if (Utils.IsTruthy(value)) {
			ThenBranch.Execute(environment);
		}
		else {
			ElseBranch?.Execute(environment);
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

public class BlockStatement : Statement {
	public List<Statement> Statements { get; }

	public BlockStatement(List<Statement> statements) {
		Statements = statements;
	}

	public override string ToString() {
		StringBuilder builder = new();
		builder.Append("(block ");
		foreach (Statement statement in Statements) {
			builder.Append(statement.ToString());
			builder.Append(" ");
		}
		builder.Append(")");
		return builder.ToString();
	}

	public override void Execute(Environment environment) {
		Environment blockEnvironment = new(environment);
		foreach (Statement statement in Statements) {
			statement.Execute(blockEnvironment);
		}
	}
}