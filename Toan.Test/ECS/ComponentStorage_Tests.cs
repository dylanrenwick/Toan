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
}
