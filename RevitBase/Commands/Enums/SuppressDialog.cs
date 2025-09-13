namespace RevitBase.Commands.Enums;

/// <summary>
/// Enumeration specifying how dialog boxes should be suppressed during command execution.
/// </summary>
[PublicAPI]
internal enum SuppressDialog
{
    /// <summary>
    /// No dialog suppression is applied.
    /// </summary>
    None,

    /// <summary>
    /// Dialogs are suppressed using a predefined result code.
    /// </summary>
    ResultCode,

    /// <summary>
    /// Dialogs are suppressed using a custom handler function.
    /// </summary>
    Handler
}