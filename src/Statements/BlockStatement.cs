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

    public override void Execute(Evaluator evaluator) {
        EvaluateBlock(evaluator, Statements, new Environment(evaluator._environment));
    }
    public void EvaluateBlock(Evaluator evaluator, List<Statement> statements, Environment environment) {
        Environment previous = evaluator._environment;
        try {
            evaluator._environment = environment;
            foreach (Statement statement in statements) {
                statement.Execute(evaluator);
            }
        }
        finally {
            evaluator._environment = previous;
        }
    }
}
