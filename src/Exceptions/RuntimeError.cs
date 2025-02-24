[Serializable]
public class RuntimeError : Exception {
    private Token token;
    public RuntimeError(Token token, string message): base(message) {
        this.token = token;
    }
}
