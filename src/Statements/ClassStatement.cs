public class ClassStatement : Statement {
    public Token Name { get; }
    public List<FunctionStatement> Methods { get; }
    public ClassStatement(Token name, List<FunctionStatement> methods) {
        Name = name;
        Methods = methods;
    }

    public override string ToString() {
        return $"(class {Name.Lexeme})";
    }

    public override void Execute(Evaluator evaluator) {
        evaluator._environment.Define(Name.Lexeme, null);
        Dictionary<string, Function> methods = new();
        foreach (var method in Methods) {
            var function = new Function(method, evaluator._environment);
            methods[method.Name.Lexeme] = function;
        }
        Cls cls = new Cls(Name.Lexeme, methods);
        evaluator._environment.Assign(Name, cls);
    }
}
