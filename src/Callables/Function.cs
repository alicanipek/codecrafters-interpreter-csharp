
public class Function : Callable {
    private FunctionStatement _declaration;
    private Environment _closure;


    public Function(FunctionStatement declaration, Environment closure) {
        _declaration = declaration;
        _closure = closure;
    }

    public int Arity => _declaration.Parameters.Count;

    public object Call(Evaluator evaluator, List<object> arguments) {
        var env = new Environment(_closure);
        for (int i = 0; i < _declaration.Parameters.Count; i++) {
            env.Define(_declaration.Parameters[i].Lexeme, arguments[i]);
        }
        try {
            var block = new BlockStatement(_declaration.Body);
            block.EvaluateBlock(evaluator, block.Statements, env);
            // evaluator.EvaluateBlock(_declaration.Body, env);
        }
        catch (ReturnException ret) {
            return ret.returnValue;
        }
        return null;
    }



    public override string ToString() {
        return "<fn " + _declaration.Name.Lexeme + ">";
    }
}
