using UnityEngine;

public class PredatorPhysics : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Food food = collision.gameObject.GetComponent<Food>();
        Prey prey = collision.gameObject.GetComponent<Prey>();
        
        if (food != null)
        {
            Destroy(food.gameObject);
        } else if (prey != null)
        {
            Destroy(prey.gameObject);
        }
    }
}
