using System.Globalization;

public class Tokenizer(string content) {
	public List<Token> tokens = new();
	public List<string> errors = new List<string>();
	public int errorCode = 0;
	List<string> reserved = new() { "and", "class", "else", "false", "for", "fun", "if", "nil", "or", "print", "return", "super", "this", "true", "var", "while" };
	int line = 1;
	public void Tokenize() {
		// Uncomment this block to pass the first stage
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
					AddToken(TokenType.LEFT_PAREN, "(");
					break;
				case ')':
					AddToken(TokenType.RIGHT_PAREN, ")");
					break;
				case '{':
					AddToken(TokenType.LEFT_BRACE, "{");
					break;
				case '}':
					AddToken(TokenType.RIGHT_BRACE, "}");
					break;
				case ',':
					AddToken(TokenType.COMMA, ",");
					break;
				case '.':
					AddToken(TokenType.DOT, ".");
					break;
				case '-':
					AddToken(TokenType.MINUS, "-");
					break;
				case '+':
					AddToken(TokenType.PLUS, "+");
					break;
				case ';':
					AddToken(TokenType.SEMICOLON, ";");
					break;
				case '*':
					AddToken(TokenType.STAR, "*");
					break;
				case '=':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new(TokenType.EQUAL_EQUAL, "==", null, line);
					}
					else {
						tk = new(TokenType.EQUAL, "=", null, line);
					}
					tokens.Add(tk);
					break;
				case '!':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new(TokenType.BANG_EQUAL, "!=", null, line);
					}
					else {
						tk = new(TokenType.BANG, "!", null, line);
					}
					tokens.Add(tk);
					break;
				case '<':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new(TokenType.LESS_EQUAL, "<=", null, line);
					}
					else {
						tk = new(TokenType.LESS, "<", null, line);
					}
					tokens.Add(tk);
					break;
				case '>':
					if (i + 1 < content.Length && content[i + 1] == '=') {
						i++;
						tk = new(TokenType.GREATER_EQUAL, ">=", null, line);
					}
					else {
						tk = new(TokenType.GREATER, ">", null, line);
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
						AddToken(TokenType.SLASH, "/");
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
					AddToken(TokenType.STRING, $"\"{value}\"", value);
					break;
				case char digit when char.IsDigit(digit):
					start = i;
					while (i < content.Length && (char.IsDigit(content[i]) || content[i] == '.')) {
						i++;
					}
					value = content[start..i];
					var v = double.Parse(value, CultureInfo.InvariantCulture);
					var literal = v.ToString().Contains(".") ? v.ToString() : $"{v}.0";
					AddToken(TokenType.NUMBER, value, literal);
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
						tk = new(Enum.Parse<TokenType>(value.ToUpper()), value, null, line);
					}else if(value == "print"){
						tk = new(TokenType.PRINT, value, null, line);
					}
					else {
						tk = new(TokenType.IDENTIFIER, value, null, line);
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
		AddToken(TokenType.EOF, "");
	}

	private void AddToken(TokenType type, string lexeme) {
		AddToken(type, lexeme, null);
	}

	private void AddToken(TokenType type, string lexeme, Object literal) {
		tokens.Add(new Token(type, lexeme, literal, line));
	}
}