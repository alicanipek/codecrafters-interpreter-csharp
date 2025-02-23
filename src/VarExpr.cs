public class VarExpr : Expr {
	public Token Name { get; }

	public VarExpr(Token name) {
		Name = name;
	}

	public override string ToString() {
		return Name.Lexeme;
	}

    public override object Evaluate(Environment environment) {
		return environment.Get(Name);
    }
}