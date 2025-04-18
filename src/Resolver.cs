
public class Resolver {
    private readonly Evaluator evaluator;

    public Resolver(Evaluator evaluator) {
        this.evaluator = evaluator;
    }

    private readonly List<Dictionary<string, bool>> scopes = new();
    private FunctionType currentFunction = FunctionType.NONE;
    private ClassType currentClass = ClassType.NONE;
    public void Resolve(List<Statement> statements) {
        try {

            foreach (var statement in statements) {
                ResolveStatement(statement);
            }
        }
        catch (RuntimeError error) {
            Console.Error.WriteLine($"[line {error.token.Line}] Error at '{error.token.Lexeme}': {error.Message}");
            System.Environment.Exit(65);
        }
    }

    private void ResolveStatement(Statement statement) {
        switch (statement) {
            case BlockStatement block:
                BeginScope();
                Resolve(block.Statements);
                EndScope();
                break;
            case ClassStatement cls:
                ClassType enclosingClass = currentClass;
                currentClass = ClassType.CLASS;
                Declare(cls.Name);
                Define(cls.Name);

                if (cls.Superclass != null && cls.Name.Lexeme.Equals(cls.Superclass.Name.Lexeme)) {
                    throw new RuntimeError(cls.Superclass.Name, "A class can't inherit from itself.");
                }

                if (cls.Superclass != null) {
                    currentClass = ClassType.SUBCLASS;
                    ResolveExpression(cls.Superclass);
                }

                if(cls.Superclass != null) {
                    BeginScope();
                    scopes[scopes.Count - 1]["super"] = true;
                }

                BeginScope();
                scopes[scopes.Count - 1]["this"] = true;
                foreach (var method in cls.Methods) {
                    FunctionType declaration = FunctionType.METHOD;
                    if (method.Name.Lexeme.Equals("init")) {
                        declaration = FunctionType.INITIALIZER;
                    }
                    ResolveFunction(method, declaration);
                }
                EndScope();
                if (cls.Superclass != null) {
                    EndScope();
                }
                currentClass = enclosingClass;
                break;
            case VarStatement var:
                Declare(var.Name);
                if (var.Initializer != null) {
                    ResolveExpression(var.Initializer);
                }
                Define(var.Name);
                break;
            case ExpressionStatement expression:
                ResolveExpression(expression.Expression);
                break;
            case IfStatement ifStatement:
                ResolveExpression(ifStatement.Condition);
                ResolveStatement(ifStatement.ThenBranch);
                if (ifStatement.ElseBranch != null) {
                    ResolveStatement(ifStatement.ElseBranch);
                }
                break;
            case WhileStatement whileStatement:
                ResolveExpression(whileStatement.Condition);
                ResolveStatement(whileStatement.Body);
                break;
            case FunctionStatement function:
                Declare(function.Name);
                Define(function.Name);
                ResolveFunction(function, FunctionType.FUNCTION);
                break;
            case PrintStatement print:
                ResolveExpression(print.Expression);
                break;
            case ReturnStatement returnStatement:
                if (currentFunction == FunctionType.NONE) {
                    throw new RuntimeError(returnStatement.Keyword, "Cannot return from top-level code.");
                }
                if (returnStatement.Value != null) {
                    if (currentFunction == FunctionType.INITIALIZER) {
                        throw new RuntimeError(returnStatement.Keyword, "Cannot return a value from an initializer.");
                    }
                    ResolveExpression(returnStatement.Value);
                }
                break;
            default:
                break;
        }
    }


    private void ResolveExpression(Expr expression) {
        switch (expression) {
            case AssignExpr assign:
                ResolveExpression(assign.Value);
                ResolveLocal(assign, assign.Name);
                break;
            case BinaryExpr binary:
                ResolveExpression(binary.Left);
                ResolveExpression(binary.Right);
                break;
            case CallExpr call:
                ResolveExpression(call.Callee);
                foreach (var argument in call.Arguments) {
                    ResolveExpression(argument);
                }
                break;
            case GroupingExpr grouping:
                ResolveExpression(grouping.Expression);
                break;
            case LiteralExpr literal:
                break;
            case LogicalExpr logical:
                ResolveExpression(logical.Left);
                ResolveExpression(logical.Right);
                break;
            case UnaryExpr unary:
                ResolveExpression(unary.Right);
                break;
            case GetExpr get:
                ResolveExpression(get.Object);
                break;
            case SetExpr set:
                ResolveExpression(set.Value);
                ResolveExpression(set.Object);
                break;
            case SuperExpr super:
                if(currentClass == ClassType.NONE){
                    throw new RuntimeError(super.Keyword, "Can't use 'super' outside of a class.");
                }else if (currentClass != ClassType.SUBCLASS) {
                    throw new RuntimeError(super.Keyword, "Can't use 'super' in a class with no superclass.");
                }
                ResolveLocal(super, super.Keyword);
                break;
            case ThisExpr t:
                if (currentClass == ClassType.NONE) {
                    throw new RuntimeError(t.Keyword, "Cannot use 'this' outside of a class.");
                }
                ResolveLocal(t, t.Keyword);
                break;
            case VarExpr variable:
                if (scopes.Count != 0) {
                    if (scopes[scopes.Count - 1].TryGetValue(variable.Name.Lexeme, out bool value) && value == false) {
                        throw new RuntimeError(variable.Name, "Cannot read local variable in its own initializer.");
                    }
                }
                ResolveLocal(variable, variable.Name);
                break;
            default:
                break;
        }
    }

    private void ResolveFunction(FunctionStatement function, FunctionType type) {
        FunctionType enclosingFunction = currentFunction;
        currentFunction = type;

        BeginScope();

        foreach (Token param in function.Parameters) {
            Declare(param);
            Define(param);
        }
        Resolve(function.Body);
        EndScope();
        currentFunction = enclosingFunction;
    }
    private void Declare(Token name) {
        if (scopes.Count == 0) return;

        Dictionary<string, bool> scope = scopes[scopes.Count - 1];
        if (scope.ContainsKey(name.Lexeme)) {
            throw new RuntimeError(name, "Variable with this name already declared in this scope.");
        }

        scope[name.Lexeme] = false;
    }

    private void Define(Token name) {
        if (scopes.Count == 0) return;
        var scope = scopes[scopes.Count - 1];
        scope[name.Lexeme] = true;
    }

    public void BeginScope() {
        scopes.Add(new Dictionary<string, bool>());
    }

    public void EndScope() {
        scopes.RemoveAt(scopes.Count - 1);
    }

    private void ResolveLocal(Expr expr, Token name) {
        for (int i = scopes.Count - 1; i >= 0; i--) {
            if (scopes[i].ContainsKey(name.Lexeme)) {
                evaluator.Resolve(expr, scopes.Count - 1 - i);
                return;
            }
        }
    }
}

public enum FunctionType {
    NONE,
    FUNCTION,
    INITIALIZER,
    METHOD
}

public enum ClassType {
    NONE,
    CLASS,
    SUBCLASS
}
