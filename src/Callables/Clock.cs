
public class Clock : Callable {


    int Callable.Arity() => 0;

    public object Call(Evaluator evaluator, List<object> arguments) {
        return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
    }

}

