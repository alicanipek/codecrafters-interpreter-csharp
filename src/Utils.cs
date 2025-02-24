public static class Utils {
	public static bool IsTruthy(object obj) {
		if (obj == null) return false;
		if (obj is bool v) return v;
		return true;
	}
}