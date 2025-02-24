public interface Callable {
    int Arity();
    object Call(Environment environment, List<object> arguments);
}
