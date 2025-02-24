[Serializable]
internal class ReturnException : Exception {
    public object? returnValue;

    public ReturnException() {
    }

    public ReturnException(object? returnValue) {
        this.returnValue = returnValue;
    }
}
