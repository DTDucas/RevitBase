using System.ComponentModel;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RevitBase.Commands.Enums;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace RevitBase.Commands;

/// <summary>
/// Abstract base class for external commands in Revit, providing common functionality
/// for command execution, exception handling, and dialog suppression.
/// </summary>
public abstract class BaseExternalCommand : IExternalCommand
{
    #region Private Fields

    private Action<DialogBoxShowingEventArgs>? _dialogBoxHandler;
    private int _dialogResultCode;

    private Action<Exception>? _exceptionHandler;
    private SuppressDialog _suppressDialog = SuppressDialog.None;
    private SuppressException _suppressException = SuppressException.None;
    private bool _suppressFailures;

    #endregion

    #region Public Properties

    public Application Application => UiApplication.Application;

    public Status Status { get; set; }

    public Result Result { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public ElementSet? ElementSet { get; private set; }

    public ExternalCommandData? ExternalCommandData { get; private set; }

    public UIApplication UiApplication { get; private set; } = null!;

    #endregion

    #region Execute

    /// <summary>
    /// Executes the external command with the provided command data.
    /// </summary>
    /// <param name="commandData">The command data containing application context</param>
    /// <param name="message">Reference to error message string</param>
    /// <param name="elements">Collection of problem elements</param>
    /// <returns>Result indicating success or failure of the command execution</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        ElementSet = elements;
        ErrorMessage = message;
        ExternalCommandData = commandData;
        UiApplication = commandData.Application;

        var userName = UiApplication.Application.Username;

        try
        {
            if (LicenseUtils.CheckCommandCanExecute(userName))
            {
                Execute();

                Status = Result switch
                {
                    Result.Succeeded => Status.Success,
                    Result.Failed => Status.Error,
                    _ => Status.Error
                };

                this.CreateCommand(userName, Status.ToString());
            }
        }
        catch (Exception ex)
        {
            this.CreateCommand(userName, $"{Status.Error}: {ex}");
        }
        finally
        {
            message = ErrorMessage;
            RestoreFailures();
            RestoreDialogs();
        }

        return Result;
    }

    /// <summary>
    /// Abstract method that must be implemented by derived classes to define the command execution logic.
    /// </summary>
    /// <returns>Result indicating success or failure of the command execution</returns>
    public abstract Result Execute();

    #endregion

    #region Suppress Exception and Dialogs

    /// <summary>
    /// Suppresses all exceptions by setting the suppression mode to Full.
    /// </summary>
    public void SuppressExceptions()
    {
        _suppressException = SuppressException.Full;
    }

    /// <summary>
    /// Suppresses exceptions and directs them to a custom handler.
    /// </summary>
    /// <param name="handler">The exception handler to process caught exceptions</param>
    public void SuppressExceptions(Action<Exception> handler)
    {
        _suppressException = SuppressException.Handler;
        _exceptionHandler = handler;
    }

    /// <summary>
    /// Suppresses failure messages by automatically resolving them during command execution.
    /// </summary>
    public void SuppressFailures()
    {
        if (_suppressFailures)
            return;

        _suppressFailures = true;

        Application.FailuresProcessing -= ResolveFailures;
        Application.FailuresProcessing += ResolveFailures;
    }

    /// <summary>
    /// Restores dialog box handling to normal behavior by removing any suppression.
    /// </summary>
    public void RestoreDialogs()
    {
        if (_suppressDialog != SuppressDialog.None)
            UiApplication.DialogBoxShowing -= ResolveDialogBox;

        _suppressDialog = SuppressDialog.None;
    }

    /// <summary>
    /// Restores failure processing to normal behavior by removing the automatic resolution.
    /// </summary>
    public void RestoreFailures()
    {
        if (!_suppressFailures)
            return;

        Application.FailuresProcessing -= ResolveFailures;
        _suppressFailures = false;
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Handles dialog box suppression based on the configured suppression mode.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="args">Dialog box showing event arguments</param>
    private void ResolveDialogBox(object? sender, DialogBoxShowingEventArgs args)
    {
        switch (_suppressDialog)
        {
            case SuppressDialog.ResultCode:
                args.OverrideResult(_dialogResultCode);
                break;

            case SuppressDialog.Handler:
                _dialogBoxHandler?.Invoke(args);
                break;
        }
    }

    /// <summary>
    /// Automatically resolves failures by deleting all warnings.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="args">Failures processing event arguments</param>
    private void ResolveFailures(object? sender, FailuresProcessingEventArgs args)
    {
        args.GetFailuresAccessor().DeleteAllWarnings();
    }

    #endregion
}