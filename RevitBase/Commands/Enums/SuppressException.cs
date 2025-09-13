namespace RevitBase.Commands.Enums;

/// <summary>
/// Enumeration specifying how exceptions should be suppressed during command execution.
/// </summary>
[PublicAPI]
internal enum SuppressException
{
    /// <summary>
    /// No exception suppression is applied.
    /// </summary>
    None,

    /// <summary>
    /// All exceptions are fully suppressed.
    /// </summary>
    Full,

    /// <summary>
    /// Exceptions are suppressed and handled by a custom handler function.
    /// </summary>
    Handler
}