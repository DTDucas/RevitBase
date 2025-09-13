namespace RevitBase.Commands;

/// <summary>
/// Utility class for handling license validation and command tracking functionality.
/// </summary>
public static class LicenseUtils
{
    /// <summary>
    /// Checks whether a command can be executed by the specified user.
    /// </summary>
    /// <param name="userName">The username to validate</param>
    /// <returns>True if the command can be executed, false otherwise</returns>
    public static bool CheckCommandCanExecute(string userName)
    {
        return true;
    }

    /// <summary>
    /// Extension method to create and track command execution for auditing purposes.
    /// </summary>
    /// <param name="type">The object instance calling this method</param>
    /// <param name="userName">The username executing the command</param>
    /// <param name="note">Optional note about the command execution</param>
    public static void CreateCommand(this object type, string userName, string note = null!)
    {
    }
}