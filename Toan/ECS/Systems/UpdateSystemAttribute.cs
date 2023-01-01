using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toan.ECS.Systems;

[AttributeUsage(AttributeTargets.Method)]
public class UpdateSystemAttribute : Attribute
{
}
