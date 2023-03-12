using Toan.ECS.Components;

namespace Toan.Test
{
    public class ComponentPoolTest
    {
        private struct StubComponent { }

        private readonly ComponentPool<StubComponent> _componentPool;

        public ComponentPoolTest()
        {
            _componentPool = new();
        }

        [Fact]
        public void Add_WithIncorrectType_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => _componentPool.Add(
                    Guid.NewGuid(),
                    new object()
                )
            );
        }
    }
}