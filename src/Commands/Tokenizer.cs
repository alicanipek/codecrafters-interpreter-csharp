using System.Globalization;

public class Tokenizer(string content) {
	public List<Token> tokens = new();
	public List<string> errors = new List<string>();
	public int errorCode = 0;
	List<string> reserved = new() { "and", "class", "else", "false", "for", "fun", "if", "nil", "or", "print", "return", "super", "this", "true", "var", "while" };
	public void Tokenize() {
		// Uncomment this block to pass the first stage
		int line = 1;
		for (int i = 0; i < content.Length; i++) {
			char c = content[i];
			Token tk;
			switch (c) {
				case '\n':
					line++;
					break;
				case '\r':
					if (i + 1 < content.Length && content[i + 1] == '\n') {
						i++;
						line++;
					}
					break;
				case '(':
					tk = new("LEFT_PAREN", "(", null, line);
					tokens.Add(tk);
					break;
				case ')':
					tk = new("RIGHT_PAREN", ")", null, line);
					tokens.Add(tk);
					break;
				case '{':
					tk = new("LEFT_BRACE", "{", null, line);
					tokens.Add(tk);
					break;
				case '}':
					tk = new("RIGHT_BRACE", "}", null, line);
					tokens.Add(tk);
					break;
				case ',':
					tk = new("COMMA", ",", null, line);
					tokens.Add(tk);
					break;
				case '.':
					tk = new("DOT", ".", null, line);
					tokens.Add(tk);
					break;
				case '-':
					tk = new("MINUS", "-", null, line);
					tokens.Add(tk);
					break;
				case '+':
					tk = new("PLUS", "+", null, line);
					tokens.Add(tk);
					break;
				case ';':
					tk = new("SEMICOLON", ";", null, line);
					tokens.Add(tk);
					break;
				case '*':
					tk = new("STAR", "*", null, line);
					tokens.Add(tk);
					break;
				case '=':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new("EQUAL_EQUAL", "==", null, line);
					}
					else {
						tk = new("EQUAL", "=", null, line);
					}
					tokens.Add(tk);
					break;
				case '!':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new("BANG_EQUAL", "!=", null, line);
					}
					else {
						tk = new("BANG", "!", null, line);
					}
					tokens.Add(tk);
					break;
				case '<':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new("LESS_EQUAL", "<=", null, line);
					}
					else {
						tk = new("LESS", "<", null, line);
					}
					tokens.Add(tk);
					break;
				case '>':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new("GREATER_EQUAL", ">=", null, line);
					}
					else {
						tk = new("GREATER", ">", null, line);
					}
					tokens.Add(tk);
					break;
				case '/':
					if (i + 1 < content.Length && content[i + 1] == '/') {
						i += 2; // Skip the initial "//"
						while (i < content.Length && content[i] != '\n' &&
							!(content[i] == '\r' && i + 1 < content.Length && content[i + 1] == '\n')) {
							i++;
						}
						i--;
					}
					else {
						tk = new("SLASH", "/", null, line);
						tokens.Add(tk);
					}
					break;
				case '\"':
					int start = i;
					i++;
					while (i < content.Length && content[i] != '"') {
						if (content[i] == '\n') {
							line++;
						}
						i++;
					}
					if (i >= content.Length) {
						errors.Add($"[line {line}] Error: Unterminated string.");
						errorCode = 65;
						break;
					}
					string value = content.Substring(start + 1, i - start - 1);
					tk = new("STRING", $"\"{value}\"", value, line);
					tokens.Add(tk);
					break;
				case char digit when char.IsDigit(digit):
					start = i;
					while (i < content.Length && (char.IsDigit(content[i]) || content[i] == '.')) {
						i++;
					}
					value = content[start..i];
					var v = double.Parse(value, CultureInfo.InvariantCulture);
					var literal = v.ToString().Contains(".") ? v.ToString() : $"{v}.0";

					tk = new("NUMBER", value, literal, line);
					tokens.Add(tk);
					i--;
					break;
				case '_':
				case char letter when char.IsLetter(letter):
					start = i;
					while (i < content.Length && (char.IsLetterOrDigit(content[i]) || content[i] == '_')) {
						i++;
					}
					value = content.Substring(start, i - start);
					if (reserved.Contains(value)) {
						tk = new(value.ToUpper(), value, null, line);
					}
					else {
						tk = new("IDENTIFIER", value, null, line);
					}
					tokens.Add(tk);
					i--;
					break;
				case '\t':
				case ' ': // Ignore whitespace
					break;
				default:
					errors.Add($"[line {line}] Error: Unexpected character: {c}");
					errorCode = 65;
					break;
			}
		}
		tokens.Add(new("EOF", "", null, line));
	}
}