public class Instance {
    private readonly Cls _cls;
    private readonly Dictionary<string, object> _fields = new();
    public Instance(Cls cls) {
        _cls = cls;
    }

    public override string ToString() {
        return _cls.Name + " instance";
    }

    public object Get(Token name) {
        if(_fields.ContainsKey(name.Lexeme)) {
            return _fields[name.Lexeme];
        }
        throw new RuntimeError(name, $"Undefined property '{name.Lexeme}'.");
    }

    public void Set(Token name, object value) {
        _fields[name.Lexeme] = value;
    }
}
