using System.Text;

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
