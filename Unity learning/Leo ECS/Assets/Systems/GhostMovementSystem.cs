using Leopotam.Ecs;
using UnityEngine;

class MovementSystem : IEcsRunSystem
{
    private readonly EcsFilter<Ghost> _ghostsFilter = null;
    private readonly float _deltaTime = Time.deltaTime;
    
    public void Run()
    {
        foreach (var i in _ghostsFilter)
        {
            ref var ghost = ref _ghostsFilter.Get1(i);
            ghost.Position += ghost.Velocity * _deltaTime;
        }
    }
}