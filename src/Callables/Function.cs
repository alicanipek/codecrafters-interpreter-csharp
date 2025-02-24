
public class Function : Callable {
    private readonly FunctionStatement declaration;
    private readonly Environment closure;
    public Function(FunctionStatement declaration, Environment closure) {
        this.declaration = declaration;
        this.closure = closure;
    }
    public int Arity() {
        return declaration.Parameters.Count;
    }

    public override String ToString() {
        return "<fn " + declaration.Name.Lexeme + ">";
    }

    public object Call(Environment environment, List<object> arguments) {
        Environment functionEnvironment = new Environment(closure);
        for (int i = 0; i < declaration.Parameters.Count; i++) {
            functionEnvironment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
        }
        try {
            BlockStatement blockStatement = new BlockStatement(declaration.Body);
            blockStatement.Execute(functionEnvironment);
        }
        catch (ReturnException returnValue) {
            return returnValue.returnValue;
        }
        return null;
    }
}
