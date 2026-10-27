using Toan.ECS.Components;

namespace Toan.Test.ECS;

public class ComponentStorage_Tests
{
	private record struct IntComponent(int Value);

	[Theory]
	// Remove first entity, middle, and last
	// Note that when first entity is removed, last entity is moved to its location
	// To target last entity after removals, target `entityCount - entitiesToRemove.Length`
	[InlineData(new int[] { 0, 10, 17 })]
	// Remove first entity, last, and middle
	[InlineData(new int[] { 0, 18, 10 })]
	// Remove last entity, middle, and first
	[InlineData(new int[] { 19, 10, 0 })]
	// Remove last entity, first, and middle
	[InlineData(new int[] { 19, 0, 10 })]
	// Remove middle entity, last, and first
	[InlineData(new int[] { 10, 18, 0 })]
	// Remove middle entity, first, and last
	[InlineData(new int[] { 10, 0, 17 })]
	public void RemovalsPreserveExistingData(int[] entitiesToRemove)
	{
		// Populate ComponentPool with several entity values
		// Remove from beginning, middle, and end of entity list
		// Confirm mappings of remaining entities are preserved and removed entities are removed

		const int entityCount = 20;
		Guid[] entities = new Guid[entityCount];
		List<Guid> removedEntities = [];
		ComponentPool<IntComponent> pool = [];

		for (int i = 0; i < entityCount; i++)
		{
			entities[i] = Guid.NewGuid();
			pool.Add(entities[i], new IntComponent(i));
		}

		Assert.Equal(entityCount, pool.Count);
		for (int i = 0; i < entityCount; i++)
		{
			Guid entity = entities[i];
			IntComponent component = pool.Get(entity);

			Assert.Equal(i, component.Value);
		}

		for (int i = 0; i < entitiesToRemove.Length; i++)
		{
			int index = entitiesToRemove[i];
			Assert.True(pool.Remove(entities[index]));

			Assert.Equal(entityCount - (i + 1), pool.Count);
			// Confirm remaining entities present
			for (int j = 0; j < entityCount; j++)
			{
				// Skip indices already removed
				if (entitiesToRemove.Take(i + 1).Contains(j)) { continue; }

				Guid entity = entities[j];
				IntComponent component = pool.Get(entity);

				Assert.Equal(j, component.Value);
			}

			removedEntities.Add(entities[index]);
			foreach (Guid removed in removedEntities)
			{
				Assert.False(pool.HasEntity(removed));
			}
		}
	}

	[Fact]
	public void RemovalsPreserveMappings()
	{
		// Confirm entities are moved as expected as they are removed, to ensure internal array remains packed
		const int entityCount = 3;
		Guid[] entities = new Guid[entityCount];
		List<Guid> removedEntities = [];
		ComponentPool<IntComponent> pool = [];

		for (int i = 0; i < entityCount; i++)
		{
			Guid entity = Guid.NewGuid();
			entities[i] = entity;
			pool.Add(entity, new IntComponent(i));
		}

		Assert.Equal(entityCount, pool.Count);

		for (int i = 0; i < entityCount; i++)
		{
			Guid entity = i == 0
				? entities[i]
				: entities[^i];

			Assert.True(pool.Remove(entity));
			removedEntities.Add(entity);
			Assert.Equal(entityCount - removedEntities.Count, pool.Count);

			for (int j = 0; j < entityCount; j++)
			{
				Guid checkEntity = entities[j];
				if (removedEntities.Contains(checkEntity))
				{
					Assert.False(pool.HasEntity(checkEntity));
				}
				else
				{
					IntComponent component = pool.Get(checkEntity);
					Assert.Equal(j, component.Value);
				}
			}
		}
	}

	[Fact]
	public void CapacityGrowthPreservesMappings()
	{
		// Fill a pool enough to trigger capacity growth
		// Remove several entities
		// Refill it enough to reuse the removed entities' space, *and* trigger another capacity growth
		// Verify expected entities remain present and removed entities are removed throughout

		// Due to equality check, pool can never be full
		// Insert N, where N is a power of 2, resulting in capacity 2N
		const int initialEntityCount = 4;
		int valueAccumulator = 0;
		Dictionary<Guid, int> entities = [];
		List<Guid> removedEntities = [];
		ComponentPool<IntComponent> pool = [];

		for (int i = 0; i < initialEntityCount; i++)
		{
			Guid entity = Guid.NewGuid();
			entities.Add(entity, valueAccumulator);
			pool.Add(entity, new IntComponent(valueAccumulator++));
		}

		Assert.Equal(entities.Count, pool.Count);
		for (int i = 0; i < entities.Count; i++)
		{
			Guid entity = entities.Keys.ElementAt(i);
			int expectedValue = entities[entity];
			IntComponent component = pool.Get(entity);

			Assert.Equal(expectedValue, component.Value);
		}

		// Removing N/2 entities results in N/2 entityCount and 2N capacity
		const int entitiesToRemove = initialEntityCount / 2;

		for (int i = 0; i < entitiesToRemove; i++)
		{
			Guid entity = entities.Keys.ElementAt(i);
			entities.Remove(entity);
			Assert.True(pool.Remove(entity));
			removedEntities.Add(entity);
		}

		Assert.Equal(initialEntityCount - entitiesToRemove, pool.Count);
		Assert.Equal(entities.Count, pool.Count);
		for (int i = 0; i < entities.Count; i++)
		{
			Guid entity = entities.Keys.ElementAt(i);
			int expectedValue = entities[entity];
			IntComponent component = pool.Get(entity);

			Assert.Equal(expectedValue, component.Value);
		}

		for (int i = 0; i < removedEntities.Count; i++)
		{
			Guid entity = removedEntities[i];
			Assert.False(pool.HasEntity(entity));
		}

		// Adding 2N entities results in 2.5N entities, guarantees capacity growth from previous capacity of 2N
		const int entitiesToRefill = initialEntityCount * 2;

		for (int i = 0; i < entitiesToRefill; i++)
		{
			Guid entity = Guid.NewGuid();
			entities.Add(entity, valueAccumulator);
			pool.Add(entity, new IntComponent(valueAccumulator++));
		}

		Assert.Equal(entitiesToRefill + initialEntityCount - entitiesToRemove, pool.Count);
		Assert.Equal(entities.Count, pool.Count);
		for (int i = 0; i < entities.Count; i++)
		{
			Guid entity = entities.Keys.ElementAt(i);
			int expectedValue = entities[entity];
			IntComponent component = pool.Get(entity);

			Assert.Equal(expectedValue, component.Value);
		}

		for (int i = 0; i < removedEntities.Count; i++)
		{
			Guid entity = removedEntities[i];
			Assert.False(pool.HasEntity(entity));
		}
	}

	[Fact]
	public void AddingComponentToExistingEntityOverwrites()
	{
		const int entityCount = 10;
		Guid[] entities = new Guid[entityCount];
		ComponentPool<IntComponent> pool = [];

		for (int i = 0; i < entityCount; i++)
		{
			Guid entity = Guid.NewGuid();
			entities[i] = entity;
			pool.Add(entity, new IntComponent(i));
		}

		Assert.Equal(entityCount, pool.Count);
		// Remove first entity to force a move
		Guid removedEntity = entities[0];
		Assert.True(pool.Remove(removedEntity));
		Assert.False(pool.HasEntity(removedEntity));

		Guid checkEntity = entities[1];
		Guid movedEntity = entities[^1];

		Assert.Equal(1, pool.Get(checkEntity).Value);
		Assert.Equal(entityCount - 1, pool.Get(movedEntity).Value);

		pool.Add(checkEntity, new IntComponent(entityCount));
		pool.Add(movedEntity, new IntComponent(entityCount + 1));

		Assert.Equal(entityCount - 1, pool.Count);
		Assert.False(pool.HasEntity(removedEntity));
		Assert.Equal(entityCount, pool.Get(checkEntity).Value);
		Assert.Equal(entityCount + 1, pool.Get(movedEntity).Value);
	}
}
