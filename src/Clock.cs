public class Clock : Callable {
	public int Arity() {
		return 0;
	}

	public object Call(Environment environment, List<object> arguments) {
		return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
	}
}
