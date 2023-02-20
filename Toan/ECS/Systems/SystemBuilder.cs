using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public readonly struct SystemBuilder
{
    public required World World { get; init; }
    public required SystemRepository Systems { private get; init; }

    public SystemBuilder Add<TSystem>()
        where TSystem : class, new()
        => Add(new TSystem());
    public SystemBuilder Add(object system)
    {
        AddByType(system);

        return this;
    }

    private void AddByType<TSystem>(TSystem system)
        where TSystem : class
    {
        Type systemType = system.GetType();
        MethodInfo? updateSystem = GetAndValidateSystem<UpdateSystemAttribute>(systemType, IsValidUpdateSystem);
        MethodInfo? renderSystem = GetAndValidateSystem<RenderSystemAttribute>(systemType, IsValidRenderSystem);
        MethodInfo? entitySystem = GetAndValidateSystem<EntitySystemAttribute>(systemType,
            method => IsValidEntitySystem(method)
                   && IsValidArchetype(GetArchetypeProperty(systemType, method))
        );

        // If it has none of them, it's not a real system
        if (updateSystem == null
         && renderSystem == null
         && entitySystem == null)
			throw new ArgumentException($"System type of {systemType.Name} is not renderable, updatable, or an entity system");

        PropertyInfo? entityQuery = null;
        if (entitySystem != null)
            entityQuery = GetArchetypeProperty(systemType, entitySystem);

        Systems.Add(new()
        {
            System       = system,
            EntitySystem = entitySystem,
            EntityQuery  = entityQuery,
            RenderSystem = renderSystem,
            UpdateSystem = updateSystem,
        });
    }

    private static MethodInfo? GetAndValidateSystem<TSystemAttribute>(Type systemType, Func<MethodInfo, bool> validationPredicate)
        where TSystemAttribute : Attribute
    {
        MethodInfo? system = systemType.GetFirstMethodWithAttribute<TSystemAttribute>();
        if (system != null && validationPredicate.Invoke(system))
            return system;

        return null;    
    }

    private static bool IsValidUpdateSystem(MethodInfo updateMethod)
    {
        return updateMethod.ParamsMatchTypes(new Type[] { typeof(World), typeof(GameTime) });
    }

    private static bool IsValidRenderSystem(MethodInfo renderMethod)
    {
        return renderMethod.ParamsMatchTypes(new Type[] { typeof(World), typeof(Renderer), typeof(GameTime) });
    }

    private static bool IsValidEntitySystem(MethodInfo entityMethod)
    {
        return entityMethod.ParamsMatchTypes(new Type[] { typeof(World), typeof(IReadOnlySet<Guid>) });
    }

    private static bool IsValidArchetype(PropertyInfo? archetypeProperty)
    {
        return archetypeProperty != null
            && archetypeProperty.PropertyType != null
            && archetypeProperty.PropertyType.ImplementsInterface(typeof(IWorldQuery));
    }

    private static PropertyInfo? GetArchetypeProperty(Type systemType, MethodInfo? entityMethod)
    {
        if (entityMethod == null)
            return null;

        MemberInfo? queryMember = GetArchetypeMember(systemType, entityMethod);

        if (queryMember == null)
            return null;

        var properties = systemType
            .GetProperties()
            .Where(
                prop => prop.Name == queryMember.Name
                     && prop.DeclaringType == systemType
            );

        return properties.FirstOrDefault();
    }

    private static MemberInfo? GetArchetypeMember(Type systemType, MethodInfo entityMethod)
    {
        var queryMemberName = entityMethod
            .GetCustomAttribute<EntitySystemAttribute>()!
            .MemberName;

        return systemType.GetMember(queryMemberName).FirstOrDefault();
    }
}

