using UnityEngine;

public class DetectionCircle : MonoBehaviour
{
    [Header("Параметры cферы")]
    public float detectionRadius = 3f; 
    
    [Header("Рисовать сферу")]
    public bool drawing = false;
    
    void OnDrawGizmos()
    {
        if (drawing)
        {
            Gizmos.color = Color.green;
            Vector3 forward = transform.forward;
            Vector3 center = transform.position + forward * detectionRadius * 0.5f;
            Gizmos.DrawWireSphere(center, detectionRadius * 0.5f);
        }
    }
    
    public Vector3 GetDetectionCenter()
    {
        return transform.position + detectionRadius * 0.5f * transform.forward;
    }
    
}
