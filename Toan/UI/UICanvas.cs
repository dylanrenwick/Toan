using System;

using Toan.ECS.Resources;

namespace Toan.UI;

public class UICanvas : Resource
{
    public TreeNode<Guid> HeirarchyTree;

	public UICanvas(Guid rootNode)
	{
		HeirarchyTree = new() { Value= rootNode };
	}
}
