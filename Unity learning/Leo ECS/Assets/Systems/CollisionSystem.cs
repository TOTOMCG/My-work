using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

class CollisionSystem : IEcsRunSystem
{
    private readonly EcsFilter<Ghost> _ghostsFilter = null;
    private readonly EcsFilter<WorldBounds> _boundsFilter = null;
    
    public void Run()
    {
        foreach (var i in _boundsFilter)
        {
            ref var bounds = ref _boundsFilter.Get1(i);
            HandleWallCollisions(bounds.Bounds);
        }
        
        HandleGhostCollisions();
    }

    private void HandleWallCollisions(Rect bounds)
    {
        foreach (var i in _ghostsFilter)
        {
            ref var ghost = ref _ghostsFilter.Get1(i);
            bool collided = false;
            
            if (ghost.Position.x - ghost.Radius < bounds.xMin)
            {
                ghost.Position.x = bounds.xMin + ghost.Radius;
                ghost.Velocity.x = Mathf.Abs(ghost.Velocity.x);
                collided = true;
            }
            else if (ghost.Position.x + ghost.Radius > bounds.xMax)
            {
                ghost.Position.x = bounds.xMax - ghost.Radius;
                ghost.Velocity.x = -Mathf.Abs(ghost.Velocity.x);
                collided = true;
            }

            if (ghost.Position.y - ghost.Radius < bounds.yMin)
            {
                ghost.Position.y = bounds.yMin + ghost.Radius;
                ghost.Velocity.y = Mathf.Abs(ghost.Velocity.y);
                collided = true;
            }
            else if (ghost.Position.y + ghost.Radius > bounds.yMax)
            {
                ghost.Position.y = bounds.yMax - ghost.Radius;
                ghost.Velocity.y = -Mathf.Abs(ghost.Velocity.y);
                collided = true;
            }
        }
    }

    private void HandleGhostCollisions()
    {
        var indices = new List<int>();
        foreach (var i in _ghostsFilter)
        {
            indices.Add(i);
        }
        
        for (int i = 0; i < indices.Count - 1; i++)
        {
            ref var ghost1 = ref _ghostsFilter.Get1(indices[i]);
        
            for (int j = i + 1; j < indices.Count; j++)
            {
                ref var ghost2 = ref _ghostsFilter.Get1(indices[j]);
                
                Vector2 delta = ghost1.Position - ghost2.Position;
                float distance = delta.magnitude;
                float minDistance = ghost1.Radius + ghost2.Radius;
            
                if (distance < minDistance)
                {
                    Vector2 normal = delta / distance;
                    Vector2 relativeVelocity = ghost1.Velocity - ghost2.Velocity;
                    float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

                    float correction = (minDistance - distance) / 2f;
                    ghost1.Position += normal * correction;
                    ghost2.Position -= normal * correction;

                    ghost1.Velocity -= velocityAlongNormal * normal;
                    ghost2.Velocity += velocityAlongNormal * normal;
                }
            }
        }
    }
}
