using Leopotam.Ecs;
using UnityEngine;

class GhostSpawnSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private readonly EcsFilter<WorldBounds> _boundsFilter = null;
    
    public void Run()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        
        foreach (var i in _boundsFilter)
        {
            ref var bounds = ref _boundsFilter.Get1(i);
            SpawnGhost(bounds.Bounds);
        }
    }
    
    private void SpawnGhost(Rect bounds)
    {
        var entity = _world.NewEntity();
        
        ref var ghost = ref entity.Get<Ghost>();
        ghost.Radius = 1f;
        ghost.Position = new Vector2(
            Random.Range(bounds.xMin + ghost.Radius, bounds.xMax - ghost.Radius),
            Random.Range(bounds.yMin + ghost.Radius, bounds.yMax - ghost.Radius)
        );
        
        float angle = Random.Range(0, Mathf.PI * 2);
        float speed = Random.Range(1f, 3f);
        ghost.Velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        
        ghost.Color = new Color(1f, 1f, 1f, 1f);
        
        entity.Get<NewGhost>();
    }
}