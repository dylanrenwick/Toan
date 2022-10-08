using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toan.ECS.Resources;

public class TextLog : Resource
{
    private Stack<LogEntry> _entries = new();

    public void Log(string message)
    {
        _entries.Push(new() { Content = message });
    }

    public string GetEntries(int entryCount)
    {
        StringBuilder stringBuilder = new();
        var entries = entryCount > 0
            ? _entries.Take(entryCount)
            : _entries;
        foreach (LogEntry entry in entries)
        {
            stringBuilder.AppendLine(entry.ToString());
        }
        return stringBuilder.ToString();
    }

    private record LogEntry
    {
        public required string Content { get; init; }
        public DateTime Timestamp { get; } = DateTime.Now;

        public override string ToString() => $"[{Timestamp.ToString("O")}] {Content}";
    }
}

