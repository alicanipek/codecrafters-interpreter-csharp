public class ThisExpr : Expr {
    public Token Keyword { get; }

    public ThisExpr(Token keyword) {
        Keyword = keyword;
    }

    public override object Evaluate(Evaluator evaluator) {
        return evaluator.LookupVariable(Keyword, this);
    }

    public override string ToString() => "this";
}
