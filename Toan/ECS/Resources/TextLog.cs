using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toan.ECS.Resources;

public class TextLog : Resource
{
    private readonly Stack<string> _entries = new();

    public void Log(string message)
        => _entries.Push(message);

    public string GetEntries(int entryCount)
    {
        StringBuilder stringBuilder = new();
        var entries = entryCount > 0
            ? _entries.Take(entryCount)
            : _entries;
        foreach (string entry in entries)
        {
            stringBuilder.AppendLine(entry);
        }
        return stringBuilder.ToString();
    }
}

