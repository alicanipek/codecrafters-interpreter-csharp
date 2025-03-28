public class SuperExpr : Expr {
    public Token Keyword { get; }
    public Token Method { get; }

    public SuperExpr(Token keyword, Token method) {
        Keyword = keyword;
        Method = method;
    }

    public override object Evaluate(Evaluator evaluator) {
        bool res = evaluator._locals.TryGetValue(this, out int distance);
        Cls supercls = (Cls)evaluator._environment.GetAt(distance, "super");

        Instance obj = (Instance)evaluator._environment.GetAt(distance - 1, "this");

        Function method = supercls.FindMethod(Method.Lexeme) ?? throw new RuntimeError(Method, "Undefined property '" + Method.Lexeme + "'.");
        return method.Bind(obj);

    }

    public override string ToString() => "super";
}
