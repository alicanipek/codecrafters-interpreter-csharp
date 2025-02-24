public class AssignExpr : Expr
{
	public Token Name { get; }
	public Expr Value { get; }

	public AssignExpr(Token name, Expr value)
	{
		Name = name;
		Value = value;
	}

    public override object Evaluate(Environment environment) {
		object value = Value.Evaluate(environment);
		environment.Assign(Name, value);
		return value;
    }
}