
public class Cls : Callable {
    public string Name;
    public Cls(string name) {
        Name = name;
    }

    public int Arity => 0;

    public object Call(Evaluator evaluator, List<object> arguments) {
        var instance = new Instance(this);
        return instance;
    }

    public override string ToString() {
        return Name;
    }
}
