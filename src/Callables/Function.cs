
public class Function : Callable {
    private FunctionStatement _declaration;
    private Environment _closure;

    private bool _isInitializer;
    public Function(FunctionStatement declaration, Environment closure, bool isInitializer = false) {
        _declaration = declaration;
        _closure = closure;
        _isInitializer = isInitializer;
    }

    public int Arity() => _declaration.Parameters.Count;

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
            if(_isInitializer) return _closure.GetAt(0, "this");
            return ret.returnValue;
        }
        if(_isInitializer) return _closure.GetAt(0, "this");
        return null;
    }

    public Function Bind(Instance instance) {
        var env = new Environment(_closure);
        env.Define("this", instance);
        return new Function(_declaration, env, _isInitializer);
    }



    public override string ToString() {
        return "<fn " + _declaration.Name.Lexeme + ">";
    }
}
