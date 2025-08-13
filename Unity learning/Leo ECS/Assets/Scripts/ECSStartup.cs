using Leopotam.Ecs;
using UnityEngine;

public class EcsBootstrap : MonoBehaviour
{
	private EcsWorld _world;
	private EcsSystems _systems;
    
	[SerializeField] private Rect _worldBounds = new Rect(-10, -5, 20, 10);
    
	void Start()
	{
		_world = new EcsWorld();
		_systems = new EcsSystems(_world);
		
		_systems
			.Add(new GhostSpawnSystem())
			.Add(new MovementSystem())
			.Add(new CollisionSystem())
			.Add(new RenderSystem());

		var boundsEntity = _world.NewEntity();
		boundsEntity.Get<WorldBounds>().Bounds = _worldBounds;
        
		_systems.Init();
	}
    
	void Update()
	{
		_systems.Run();
	}
    
	void OnDestroy()
	{
		_systems.Destroy();
		_world.Destroy();
	}
}