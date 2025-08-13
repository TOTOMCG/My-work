using UnityEngine;

public class FoodSpawnSystem : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject preyPrefab;
    public float foodSpawnInterval = 3f;
    public float preySpawnInterval = 4f;
    public GameObject spawnArea;

    
    private float foodSpawnTimer = 0f;
    private float preySpawnTimer = 0f;
    
    void Update()
    {
        foodSpawnTimer += Time.deltaTime;
        preySpawnTimer += Time.deltaTime;
        
        if (foodSpawnTimer >= foodSpawnInterval)
        {
            SpawnFood();
            foodSpawnTimer = 0f;
        }

        if (preySpawnTimer >= preySpawnInterval)
        {
            SpawnPrey();
            preySpawnTimer = 0f;
        }
    }

    void SpawnFood()
    {
        BoxCollider2D spawnBounds = spawnArea.GetComponent<BoxCollider2D>();
        Vector2 randomPosition = new Vector3(Random.Range(spawnBounds.bounds.min.x, spawnBounds.bounds.max.x), 
            Random.Range(spawnBounds.bounds.min.y, spawnBounds.bounds.max.y));
        GameObject foodGO = Instantiate(foodPrefab, randomPosition, Quaternion.identity);
        Food food = foodGO.GetComponent<Food>();
        
        if (food == null)
        {
            food = foodGO.AddComponent<Food>();
        }
    }

    void SpawnPrey()
    {
        BoxCollider2D spawnBounds = spawnArea.GetComponent<BoxCollider2D>();
        Vector2 randomPosition = new Vector3(Random.Range(spawnBounds.bounds.min.x, spawnBounds.bounds.max.x), 
            Random.Range(spawnBounds.bounds.min.y, spawnBounds.bounds.max.y));
        GameObject preyGO = Instantiate(preyPrefab, randomPosition, Quaternion.identity);
        Prey prey = preyGO.GetComponent<Prey>();
        
        if (prey == null)
        {
            prey = preyGO.AddComponent<Prey>();
        }
    }
}
