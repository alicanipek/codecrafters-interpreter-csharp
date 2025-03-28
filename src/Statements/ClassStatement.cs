public class ClassStatement : Statement {
    public Token Name { get; }
    public VarExpr? Superclass { get; }
    public List<FunctionStatement> Methods { get; }
    public ClassStatement(Token name, VarExpr? superclass, List<FunctionStatement> methods) {
        Name = name;
        Superclass = superclass;
        Methods = methods;
    }

    public override string ToString() {
        return $"(class {Name.Lexeme})";
    }

    public override void Execute(Evaluator evaluator) {
        object superclass = null;
        if (Superclass != null) {
            superclass = evaluator.Evaluate(Superclass);
            if (superclass is not Cls) {
                throw new RuntimeError(Superclass.Name, "Superclass must be a class");
            }
        }
        evaluator._environment.Define(Name.Lexeme, null);

        if (Superclass != null) {
            evaluator._environment = new Environment(evaluator._environment);
            evaluator._environment.Define("super", superclass);
        }

        Dictionary<string, Function> methods = new();
        foreach (var method in Methods) {
            var function = new Function(method, evaluator._environment, method.Name.Lexeme.Equals("init"));
            methods[method.Name.Lexeme] = function;
        }
        Cls cls = new Cls(Name.Lexeme, (Cls)superclass, methods);
        if(superclass != null) {
            evaluator._environment = evaluator._environment.Enclosing;
        }
        evaluator._environment.Assign(Name, cls);
    }
}
