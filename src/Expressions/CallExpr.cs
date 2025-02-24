internal class CallExpr : Expr {
    private Expr callee;
    private Token paren;
    private List<Expr> arguments;

    public CallExpr(Expr callee, Token paren, List<Expr> arguments) {
        this.callee = callee;
        this.paren = paren;
        this.arguments = arguments;
    }

    public override object Evaluate(Environment environment) {
        object value = callee.Evaluate(environment);

        List<Object> args = [];
        foreach (Expr argument in arguments) {
            args.Add(argument.Evaluate(environment));
        }

        if (value is not Callable) {
            throw new RuntimeError(paren,
                "Can only call functions and classes.");
        }

        Callable function = value as Callable;
        if (args.Count != function.Arity()) {
            throw new RuntimeError(paren, $"Expected {function.Arity()} arguments but got {args.Count}.");
        }

        return function.Call(environment, args);
    }
}
