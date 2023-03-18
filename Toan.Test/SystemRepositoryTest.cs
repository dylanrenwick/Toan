using Toan.ECS.Systems;
using Toan.Test.Stub;

namespace Toan.Test;

public class SystemRepositoryTest
{
    private readonly SystemRepository _systemRepository;
    private readonly StubSystem _system;

    public SystemRepositoryTest()
    {
        _systemRepository = new();
        _system = new();
    }

    [Fact]
    public void Add_AddsUpdateSystem()
    {
        var systemInfo = _system.SystemInfo;

        _systemRepository.Add(systemInfo);

        Assert.Equal(3, _systemRepository.Count);
    }

    [Fact]
    public void Update_ExecutesUpdateSystems()
    {
        var systemInfo = _system.SystemInfo;

        _systemRepository.Add(systemInfo);
        _systemRepository.Update(
            world: new() { Log = StubLogger.Instance },
            gameTime: new()
        );

        Assert.True(_system.UpdateCalled);
    }
}
