public interface Callable {
    object Call(Evaluator evaluator, List<object> arguments);
    int Arity { get; }
}
