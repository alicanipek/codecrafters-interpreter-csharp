
public class Cls : Callable {
    public string Name;
    public Dictionary<string, Function> Methods;
    public Cls(string name, Dictionary<string, Function> methods) {
        Name = name;
        Methods = methods;
    }

    public int Arity => 0;

    public object Call(Evaluator evaluator, List<object> arguments) {
        var instance = new Instance(this);
        return instance;
    }

    public Function? FindMethod(string name) {
        if (Methods.TryGetValue(name, out Function? value)) {
            return value;
        }
        return null;
    }

    public override string ToString() {
        return Name;
    }
}
