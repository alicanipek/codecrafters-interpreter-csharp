public class SetExpr : Expr {
    public Expr Object;
    public Token Name;
    public Expr Value;

    public SetExpr(Expr obj, Token name, Expr value) {
        Object = obj;
        Name = name;
        Value = value;
    }

    public override object Evaluate(Evaluator evaluator) {
        var obj = Object.Evaluate(evaluator);
        if (obj is Instance instance) {
            var value = Value.Evaluate(evaluator);
            instance.Set(Name, value);
            return value;
        }

        throw new RuntimeError(Name, "Only instances have fields.");
    }
}
