
public class Evaluator {
	Environment global;
	public Evaluator() {
		global = new Environment();
	}
	public Evaluator(Environment environment) {
		global = environment;
	}

	public object Evaluate(Expr expression) {
		try {
			return expression.Evaluate(global);
		}
		catch (Exception ex) {
			System.Environment.Exit(70);
			return null;
		}
	}
	public void Run(List<Statement> statements) {
		try {
			foreach (var statement in statements)
			{
				statement.Execute(global);
			}
		}
		catch (Exception ex) {
			System.Environment.Exit(70);
		}
	}
}
