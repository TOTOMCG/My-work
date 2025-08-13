using UnityEngine;

public class PreyBehaviour : MonoBehaviour
{
    private Prey[] preys;
    
    void Update()
    {
        preys = FindObjectsOfType<Prey>();
        
        Food[] allFood =  FindObjectsOfType<Food>();
        Predator[] predators = FindObjectsOfType<Predator>();
        
        foreach (var prey in preys)
        {   
            if (prey == null) continue;

            Transform nearestPredator = GetNearestPredator(prey.transform.position, predators);
            bool shouldBeScared = nearestPredator != null && 
                                  Vector3.Distance(prey.transform.position, nearestPredator.position) <= prey.scareRadius;

            prey.scared = shouldBeScared;
            if (!prey.scared)
            {
                Transform nearestFood = GetNearestFood(prey.transform.position, allFood);

                if (nearestFood != null)
                {
                    Vector2 direction = nearestFood.position - prey.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    prey.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                    prey.transform.Translate( prey.speed * Time.deltaTime * Vector3.up,Space.Self);
                }
            }
            else
            {
                Vector2 direction = nearestPredator.position - prey.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                prey.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                prey.transform.Translate( -prey.speed * Time.deltaTime * Vector3.up,Space.Self);
            }
        }
    }
    
    Transform GetNearestFood(Vector3 preyPosition, Food[] foodArray)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Food food in foodArray)
        {
            if (food == null) continue;
            
            float distance = Vector3.Distance(preyPosition, food.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = food.transform;
            }
        }
        return nearest;
    }
    
    Transform GetNearestPredator(Vector3 predPosition, Predator[] predArray)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Predator pred in predArray)
        {
            if (pred == null) continue;
            
            float distance = Vector3.Distance(predPosition, pred.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = pred.transform;
            }
        }
        return nearest;
    }
}
