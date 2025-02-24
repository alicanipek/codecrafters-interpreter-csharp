public class ReturnStatement : Statement {
	private Token keyword;
	private Expr? value;

	public ReturnStatement(Token keyword, Expr? value) {
		this.keyword = keyword;
		this.value = value;
	}

	public override void Execute(Environment environment) {
		object returnValue = null;
		if (value != null) {
			returnValue = value.Evaluate(environment);
		}
		throw new ReturnException(returnValue);
	}
}
