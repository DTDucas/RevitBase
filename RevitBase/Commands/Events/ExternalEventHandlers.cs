using Autodesk.Revit.UI;

namespace RevitBase.Commands.Events;

/// <summary>
/// External event handler that can execute multiple actions sequentially.
/// </summary>
public class ExternalEventHandlers : IExternalEventHandler
{
    /// <summary>
    /// Gets or sets the list of actions to be executed sequentially.
    /// </summary>
    public List<Action> ListActions { get; set; } = new();

    /// <summary>
    /// Executes all actions in the ListActions collection sequentially.
    /// </summary>
    /// <param name="app">The Revit UI application instance</param>
    public void Execute(UIApplication app)
    {
        ListActions.ForEach(x => x());
    }

    /// <summary>
    /// Gets the name identifier for this external event handler.
    /// </summary>
    /// <returns>The name identifier string</returns>
    public string GetName()
    {
        return "DTDucas S.T.C";
    }
}