using System;

using Toan.ECS.Components;

namespace Toan.Debug;

public struct DebugLog : IComponent
{
    public required Guid LogResourceID { get; init; }
    public required int EntryCount { get; set; }
}

