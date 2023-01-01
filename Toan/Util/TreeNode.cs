using System.Collections.Generic;

namespace Toan;

public class TreeNode<T>
    where T : notnull
{
    public TreeNode<T>? Parent { get; set; }
    public readonly HashSet<TreeNode<T>> Children = new();

    public required T Value { get; set; }
}
