using System;
using System.Reflection;

namespace Toan.ECS.Systems;

public class ToanSystemException : Exception
{
    public Type SystemType { get; set; }
    public MethodInfo SystemMethod { get; set; }

    public ToanSystemException(
        string message,
        Type systemType,
        MethodInfo systemMethod
    ) : base(message)
    {
        SystemType = systemType;
        SystemMethod = systemMethod;
    }
}
