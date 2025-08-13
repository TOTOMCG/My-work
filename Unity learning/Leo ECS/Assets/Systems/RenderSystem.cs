using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

class RenderSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private readonly EcsFilter<Ghost, NewGhost> _newGhostsFilter = null;
    private readonly EcsFilter<Ghost> _ghostsFilter = null;
    
    private readonly Dictionary<int, GameObject> _ghostObjects = new Dictionary<int, GameObject>();
    
    public void Run()
    {
        // Обработка новых призраков
        foreach (var i in _newGhostsFilter)
        {
            var entity = _newGhostsFilter.GetEntity(i);
            ref var ghost = ref _newGhostsFilter.Get1(i);
            
            var ghostObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Object.Destroy(ghostObj.GetComponent<Collider>());
            
            ghostObj.transform.position = new Vector3(ghost.Position.x, ghost.Position.y, 0);
            ghostObj.transform.localScale = Vector3.one * ghost.Radius * 2;
            ghostObj.GetComponent<Renderer>().material.color = ghost.Color;
            
            _ghostObjects[entity.GetInternalId()] = ghostObj;
            entity.Del<NewGhost>();
        }
        
        foreach (var i in _ghostsFilter)
        {
            var entity = _ghostsFilter.GetEntity(i);
            ref var ghost = ref _ghostsFilter.Get1(i);
            
            if (_ghostObjects.TryGetValue(entity.GetInternalId(), out var ghostObj))
            {
                ghostObj.transform.position = new Vector3(ghost.Position.x, ghost.Position.y, 0);
            }
        }
        
        var aliveIds = new HashSet<int>();
        foreach (var i in _ghostsFilter)
        {
            aliveIds.Add(_ghostsFilter.GetEntity(i).GetInternalId());
        }
        
        var toRemove = _ghostObjects
            .Where(p => !aliveIds.Contains(p.Key))
            .ToList();
            
        foreach (var pair in toRemove)
        {
            Object.Destroy(pair.Value);
            _ghostObjects.Remove(pair.Key);
        }
    }
}