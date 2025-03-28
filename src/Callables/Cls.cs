
public class Cls : Callable {
    public string Name;
    public Cls? Superclass;
    public Dictionary<string, Function> Methods;
    public Cls(string name, Cls? superclass, Dictionary<string, Function> methods) {
        Name = name;
        Superclass = superclass;
        Methods = methods;
    }

    public int Arity() {
        Function initializer = FindMethod("init");
        if (initializer == null) return 0;
        return initializer.Arity();
    }

    public object Call(Evaluator evaluator, List<object> arguments) {
        var instance = new Instance(this);
        Function? initializer = FindMethod("init");
        if (initializer != null) {
            initializer.Bind(instance).Call(evaluator, arguments);
        }
        return instance;
    }

    public Function? FindMethod(string name) {
        if (Methods.TryGetValue(name, out Function? value)) {
            return value;
        }

        if (Superclass != null) {
            return Superclass.FindMethod(name);
        }

        return null;
    }

    public override string ToString() {
        return Name;
    }
}
