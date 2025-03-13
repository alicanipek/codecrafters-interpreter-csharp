public class ReturnStatement : Statement {
    public Token Keyword { get; }
    public Expr? Value { get; }

    public ReturnStatement(Token keyword, Expr? value) {
        Keyword = keyword;
        Value = value;
    }

    public override void Execute(Evaluator evaluator) {
        object returnValue = null;
        if (Value != null) {
            returnValue = Value.Evaluate(evaluator);
        }
        throw new ReturnException(returnValue);
    }
}
