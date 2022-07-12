namespace BakedEnv.Interpreter;

public static class ErrorCodes
{

    #region Processor Statement Errors

    public const string UnregisteredProcStatement = "PRST0001";
    public const string UnexpectedProcStatementValue = "PRST0002";
    public const string ExpectedProcStatementEnum = "PRST0003";

    #endregion
    
    #region Variable Errors

    public const string InvalidVariableOrPath = "VAR0001";

    #endregion

    #region Invocation Errors

    public const string InvokeNull = "INVK0001";
    public const string InvokeNonCallable = "INVK0002";
    public const string InvokeArgumentMismatch = "INVK0003";
    public const string InvokeParameterCountMismatch = "INVK0003";

    #endregion

    #region Token Errors

    public const string InvalidTokenType = "TOK0001";
    public const string EndOfFile = "TOK0002";

    #endregion

    #region Value Errors

    public const string InvalidValue = "VAL0001";
    public const string InvalidOperator = "VAL0002";

    #endregion

}