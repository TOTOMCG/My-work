using UnityEngine;

public class PredatorBehaviour : MonoBehaviour
{
    private Predator[] predators;
    
    void Update()
    {
        predators = FindObjectsOfType<Predator>();
        
        Food[] allFood =  FindObjectsOfType<Food>();
        Prey[] preys = FindObjectsOfType<Prey>();
        
        if (allFood.Length == 0 && preys.Length == 0) return;
        
        foreach (var predator in predators)
        {
            Transform nearestFood = GetNearestFood(predator.transform.position, allFood, preys);
        
            if (nearestFood != null)
            {
                Vector2 direction = nearestFood.position - predator.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                predator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                predator.transform.Translate(predator.speed * Time.deltaTime * Vector3.up,Space.Self);
            }
            
            
        }
    }
    Transform GetNearestFood(Vector3 predatorPosition, Food[] foodArray, Prey[] preyArray)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Food food in foodArray)
        {
            if (food == null) continue;
            
            float distance = Vector3.Distance(predatorPosition, food.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = food.transform;
            }
        }
        
        foreach (Prey prey in preyArray)
        {
            if (prey == null) continue;
            
            float distance = Vector3.Distance(predatorPosition, prey.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = prey.transform;
            }
        }
        return nearest;
    }
}
