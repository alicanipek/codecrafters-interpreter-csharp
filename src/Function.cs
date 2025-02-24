
public class Function : Callable {
	private readonly FunctionStatement declaration;
	public Function(FunctionStatement declaration) {
		this.declaration = declaration;
	}
	public int Arity() {
		return declaration.Parameters.Count;
	}

	public override String ToString() {
		return "<fn " + declaration.Name.Lexeme + ">";
	}

	public object Call(Environment environment, List<object> arguments) {
		Environment functionEnvironment = new Environment(environment);
		for (int i = 0; i < declaration.Parameters.Count; i++) {
			functionEnvironment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
		}
		BlockStatement blockStatement = new BlockStatement(declaration.Body);
		blockStatement.Execute(functionEnvironment);
		return null;
	}
}