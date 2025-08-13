using UnityEngine;

public class PreyPhysics : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Food food = collision.gameObject.GetComponent<Food>();
        if (food != null)
        {
            Destroy(food.gameObject);
        }
    }
}