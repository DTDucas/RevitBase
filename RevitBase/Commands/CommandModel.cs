namespace RevitBase.Commands;

/// <summary>
/// Data model representing a command execution record with user information and metadata.
/// </summary>
public class CommandModel(
    string commandName,
    string userName,
    string developerName,
    DateTime dateTime,
    string note = null!)
{
    /// <summary>
    /// Gets or sets the name of the command that was executed.
    /// </summary>
    public string CommandName { get; set; } = commandName;

    /// <summary>
    /// Gets or sets the username of the person who executed the command.
    /// </summary>
    public string UserName { get; set; } = userName;

    /// <summary>
    /// Gets or sets the name of the developer who created the command.
    /// </summary>
    public string DeveloperName { get; set; } = developerName;

    /// <summary>
    /// Gets or sets the date and time when the command was executed.
    /// </summary>
    public DateTime Datetime { get; set; } = dateTime;

    /// <summary>
    /// Gets or sets additional notes or comments about the command execution.
    /// </summary>
    public string Note { get; set; } = note;
}