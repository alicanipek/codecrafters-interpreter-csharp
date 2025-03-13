public class WhileStatement : Statement {
    public Expr Condition { get; }
    public Statement Body { get; }

    public WhileStatement(Expr condition, Statement body) {
        Condition = condition;
        Body = body;
    }

    public override void Execute(Evaluator evaluator) {
        while (Utils.IsTruthy(Condition.Evaluate(evaluator))) {
            Body.Execute(evaluator);
        }
    }
}
