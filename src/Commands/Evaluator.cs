
public class Evaluator {
    public Environment Globals { get; }
    public Environment _environment;
    public readonly Dictionary<Expr, int> _locals = new Dictionary<Expr, int>();
    public Evaluator() {
        Globals = new Environment();
        _environment = Globals;
        _environment.Define("clock", new Clock());
    }


    public void Resolve(Expr expr, int depth) {
        _locals[expr] = depth;
    }

    public object LookupVariable(Token name, Expr expr) {
        bool res = _locals.TryGetValue(expr, out int distance);
        if (res) {
            return _environment.GetAt(distance, name.Lexeme);
        }

        return Globals.Get(name);
    }
	public object Evaluate(Expr expression) {
		try {
			return expression.Evaluate(this);
		}
		catch (Exception ex) {
			System.Environment.Exit(70);
			return null;
		}
	}

    public void Run(List<Statement> statements) {
        try {
            foreach (Statement statement in statements) {
                statement.Execute(this);
            }
        }
        catch (RuntimeError error) {
            Console.Error.WriteLine($"[line {error.token.Line}] Error {""} : {error.Message}");
            System.Environment.Exit(70);
        }
    }
}
