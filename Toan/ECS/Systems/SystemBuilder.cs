using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public struct SystemBuilder
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
        MethodInfo? updateSystem = systemType.GetFirstMethodWithAttribute<UpdateSystemAttribute>();
        MethodInfo? renderSystem = systemType.GetFirstMethodWithAttribute<RenderSystemAttribute>();
        MethodInfo? entitySystem = systemType.GetFirstMethodWithAttribute<EntitySystemAttribute>();

        // If it has none of them, it's not a real system
        if (updateSystem == null
         && renderSystem == null
         && entitySystem == null)
			throw new ArgumentException($"System type of {systemType.Name} is not renderable, updatable, or an entity system");

        // Reflection has no real type-safety
        if (updateSystem != null) ValidateUpdateSystem(updateSystem);
        if (renderSystem != null) ValidateRenderSystem(renderSystem);
        PropertyInfo? entityQuery = GetArchetypeProperty(systemType, entitySystem);
        if (entitySystem != null) ValidateEntitySystem(entitySystem, entityQuery);

        Systems.Add(new()
        {
            System       = system,
            EntitySystem = entitySystem,
            EntityQuery  = entityQuery,
            RenderSystem = renderSystem,
            UpdateSystem = updateSystem,
        });
    }

    private void ValidateUpdateSystem(MethodInfo updateMethod)
    {
        ValidateSystem(updateMethod, new Type[] { typeof(World), typeof(GameTime) });
    }

    private void ValidateRenderSystem(MethodInfo renderMethod)
    {
        ValidateSystem(renderMethod, new Type[] { typeof(World), typeof(Renderer), typeof(GameTime) });
    }

    private void ValidateEntitySystem(MethodInfo entityMethod, PropertyInfo? archetypeProperty)
    {
        ValidateSystem(entityMethod, new Type[] { typeof(World), typeof(IReadOnlySet<Guid>) });

        ValidateArchetypeProperty(entityMethod, archetypeProperty);
    }

    private void ValidateArchetypeProperty(MethodInfo entityMethod, PropertyInfo? archetypeProperty)
    {
        if (archetypeProperty == null)
            throw new Exception($"Archetype query {entityMethod.GetCustomAttribute<EntitySystemAttribute>()!.MemberName} on type {entityMethod.DeclaringType!.FullName} does not exist!");

        Type queryMemberType = archetypeProperty.PropertyType;

        if (queryMemberType == null || !queryMemberType.ImplementsInterface(typeof(IWorldQuery)))
            throw new Exception($"Archetype query {archetypeProperty.Name} on type {archetypeProperty.DeclaringType!.FullName} must be of type {nameof(IWorldQuery)}!");
    }

    private PropertyInfo? GetArchetypeProperty(Type systemType, MethodInfo? entityMethod)
    {
        if (entityMethod == null)
            return null;

        MemberInfo? queryMember = GetArchetypeMember(systemType, entityMethod);

        if (queryMember == null)
            return null;

        return systemType.GetProperty(queryMember.Name);
    }

    private MemberInfo? GetArchetypeMember(Type systemType, MethodInfo entityMethod)
    {
        var queryMemberName = entityMethod
            .GetCustomAttribute<EntitySystemAttribute>()!
            .MemberName;

        return systemType.GetMember(queryMemberName).FirstOrDefault();
    }

    private void ValidateSystem(MethodInfo method, Type[] expectedParamTypes)
    {
        if (!method.ParamsMatchTypes(expectedParamTypes))
            throw new Exception($"Method {method.Name} on type {method.DeclaringType?.FullName} is not valid! Parameters must match the signature of ({string.Join(", ", expectedParamTypes.Select(t => t.Name))})");
    }
}

