using Autodesk.Revit.UI;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace RevitBase.Commands.Events;

/// <summary>
/// Singleton external event handler that executes actions in the Revit UI thread context.
/// </summary>
public class ExternalEventHandlerAction : IExternalEventHandler
{
    private static Action? _action;

    private static ExternalEventHandlerAction? _instance;
    private static ExternalEvent? _externalEvent;

    private static readonly object Lock = new();

    /// <summary>
    /// Gets the singleton instance of the ExternalEventHandlerAction using thread-safe lazy initialization.
    /// </summary>
    public static ExternalEventHandlerAction Instance
    {
        get
        {
            if (_instance != null) return _instance;
            lock (Lock)
            {
                _instance ??= new ExternalEventHandlerAction();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Executes the stored action in the UI thread context if an active document exists.
    /// </summary>
    /// <param name="app">The Revit UI application instance</param>
    public void Execute(UIApplication app)
    {
        if (app.ActiveUIDocument == null)
        {
            TaskDialog.Show("Notification", "No active document, nothing to do.");
            return;
        }

        _action?.Invoke();
    }

    /// <summary>
    /// Gets the name identifier for this external event handler.
    /// </summary>
    /// <returns>The name identifier string</returns>
    public string GetName()
    {
        return "DTDucas S.T.C";
    }

    /// <summary>
    /// Creates or returns the existing ExternalEvent instance using lazy initialization.
    /// </summary>
    /// <returns>The ExternalEvent instance</returns>
    public ExternalEvent Create()
    {
        return _externalEvent ??= ExternalEvent.Create(Instance);
    }

    /// <summary>
    /// Sets the action to be executed when the external event is raised.
    /// </summary>
    /// <param name="parameter">The action to execute</param>
    public void SetAction(Action parameter)
    {
        _action = parameter;
    }
}