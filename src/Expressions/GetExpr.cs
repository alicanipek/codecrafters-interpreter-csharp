public class GetExpr : Expr {
    public Expr Object { get; }
    public Token Name { get; }

    public GetExpr(Expr @object, Token name) {
        Object = @object;
        Name = name;
    }

    public override object Evaluate(Evaluator evaluator) {
        var obj = Object.Evaluate(evaluator);
        if (obj is Instance instance) {
            return instance.Get(Name);
        }

        throw new RuntimeError(Name, "Only instances have properties.");

    }
}
