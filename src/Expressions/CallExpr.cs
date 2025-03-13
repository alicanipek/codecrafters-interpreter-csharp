
internal class CallExpr : Expr {
    public Expr Callee { get; }
    public Token Paren { get; }
    public List<Expr> Arguments { get; }

    public CallExpr(Expr callee, Token paren, List<Expr> arguments) {
        Callee = callee;
        Paren = paren;
        Arguments = arguments;
    }

    public override object Evaluate(Evaluator evaluator) {
        object value = Callee.Evaluate(evaluator);

        List<Object> args = [];
        foreach (Expr argument in Arguments) {
            args.Add(argument.Evaluate(evaluator));
        }

        if (value is not Callable) {
            throw new RuntimeError(Paren,
                "Can only call functions and classes.");
        }

        Callable function = value as Callable;
        if (args.Count != function.Arity) {
            throw new RuntimeError(Paren, $"Expected {function.Arity} arguments but got {args.Count}.");
        }

        return function.Call(evaluator, args);
    }
}
