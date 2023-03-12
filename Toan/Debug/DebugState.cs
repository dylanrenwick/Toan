using System.Collections.Generic;
using System.Collections.Frozen;

using Toan.ECS.Resources;

namespace Toan.Debug;

public class DebugState : Resource
{
    public DebugDisplayState DisplayState { get; set; }
    public bool ShouldDisplay
        => DisplayState != DebugDisplayState.None;
    public bool HasTextDisplay
        => _textDisplayStates.Contains(DisplayState);

    private static readonly FrozenSet<DebugDisplayState> _textDisplayStates
        = FrozenSet.ToFrozenSet(new HashSet<DebugDisplayState>()
    {
        DebugDisplayState.Log,
        DebugDisplayState.Stats,
    });
}
