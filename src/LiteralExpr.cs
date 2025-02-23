public class LiteralExpr : Expr {
	public object Value { get; }

	public LiteralExpr(object value) {
		Value = value;
	}

	public override object Evaluate(Environment environment) {
		return Value;
	}

	public override string ToString() {
		if(Value == null) {
			return "nil";
		}
		if(Value is bool) {
			return Value.ToString().ToLower();
		}
		if(Value is double) {
			var p = Value.ToString();
			var literal = p.Contains(".") ? p.ToString() : $"{p}.0";
			return literal;
		}
		return $"{Value}";
	}
}
