
public class Evaluator {
	public object Evaluate(Expr expression) {
		try {
			return expression.Evaluate();
		}
		catch (Exception ex) {
			Environment.Exit(70);
			return null;
		}
	}
	public void Run(List<Statement> statements) {
		try {
			foreach (var statement in statements)
			{
				statement.Execute();
			}
		}
		catch (Exception ex) {
			Environment.Exit(70);
		}
	}
}