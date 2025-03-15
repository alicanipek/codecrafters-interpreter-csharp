public class Environment {
    private readonly Environment enclosing;
    private readonly Dictionary<string, object> values = new();
    public Environment() {
        enclosing = null;
    }
    public Environment(Environment enclosing) {
        this.enclosing = enclosing;
    }
    public object Get(Token name) {
        if (values.TryGetValue(name.Lexeme, out object? value)) {
            return value;
        }

        if (enclosing != null) {
            return enclosing.Get(name);
        }

        throw new RuntimeError(name, $"Undefined variable {name.Lexeme}. {name.Line}");
    }

    public void Assign(Token name, object value) {
        if (values.ContainsKey(name.Lexeme)) {
            values[name.Lexeme] = value;
            return;
        }

        if (enclosing != null) {
            enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public void Define(string name, object value) {
        values[name] = value;
    }

    public Environment Ancestor(int distance) {
        Environment environment = this;
        for (int i = 0; i < distance; i++) {
            environment = environment.enclosing;
        }

        return environment;
    }

    public object GetAt(int distance, string name) {
        return Ancestor(distance).values[name];
    }

    public void AssignAt(int distance, Token name, object value) {
        Ancestor(distance).Assign(name, value);
    }

    public override string ToString() {
        String result = values.ToString();
        if (enclosing != null) {
            result += " -> " + enclosing.ToString();
        }

        return result;
    }
}
