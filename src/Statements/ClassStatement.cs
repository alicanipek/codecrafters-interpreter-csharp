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
        Cls cls = new Cls(Name.Lexeme);
        evaluator._environment.Assign(Name, cls);
    }
}
