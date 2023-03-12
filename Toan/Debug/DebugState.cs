using System.Collections.Generic;

using Toan.ECS.Resources;

namespace Toan.Debug;

public class DebugState : Resource
{
    public DebugDisplayState DisplayState { get; set; }
    public bool ShouldDisplay
        => DisplayState != DebugDisplayState.None;
    public bool HasTextDisplay
        => _textDisplayStates.Contains(DisplayState);

    private static readonly HashSet<DebugDisplayState> _textDisplayStates = new()
    {
        DebugDisplayState.Log,
        DebugDisplayState.Stats,
    };
}
