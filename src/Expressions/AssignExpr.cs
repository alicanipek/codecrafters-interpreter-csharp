public class AssignExpr : Expr {
    public Token Name { get; }
    public Expr Value { get; }

    public AssignExpr(Token name, Expr value) {
        Name = name;
        Value = value;
    }

    public override object Evaluate(Evaluator evaluator) {
        object value = Value.Evaluate(evaluator);
        bool res = evaluator._locals.TryGetValue(this, out int distance);
        if (res) {
            evaluator._environment.AssignAt(distance, Name, value);
        } else {
            evaluator.Globals.Assign(Name, value);
        }

        return value;
    }
}
