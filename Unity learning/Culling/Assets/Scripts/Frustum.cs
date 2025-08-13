using UnityEngine;

public class Frustum : MonoBehaviour
{
    [Header("Параметры фрустума")]
    public float fov = 60f;
    public float aspect = 1.77f;
    public float near = 1f;
    public float far = 3f;

    [Header("Рисовать фрустум")]
    public bool drawing = false;
    
    

    void OnDrawGizmos()
    {
        if (drawing)
        {
            Vector3[] vertices =  ReturnVertices();
            
            Gizmos.color = Color.blue;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            // Ближняя плоскость
            Gizmos.DrawLine(vertices[0], vertices[1]);
            Gizmos.DrawLine(vertices[1], vertices[2]);
            Gizmos.DrawLine(vertices[2], vertices[3]);
            Gizmos.DrawLine(vertices[3], vertices[0]);

            // Дальняя плоскость
            Gizmos.DrawLine(vertices[4], vertices[5]);
            Gizmos.DrawLine(vertices[5], vertices[6]);
            Gizmos.DrawLine(vertices[6], vertices[7]);
            Gizmos.DrawLine(vertices[7], vertices[4]);

            // Соединяем ближнюю и дальнюю
            Gizmos.DrawLine(vertices[0], vertices[4]);
            Gizmos.DrawLine(vertices[1], vertices[5]);
            Gizmos.DrawLine(vertices[2], vertices[6]);
            Gizmos.DrawLine(vertices[3], vertices[7]);
        }
    }

    public Vector3[] ReturnVertices()
    {
        Vector3[] vertices = new Vector3[8];
        
        // Размеры ближней
        float nearHeight = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * near;
        float nearWidth = nearHeight * aspect;

        // Размеры дальней
        float farHeight = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * far;
        float farWidth = farHeight * aspect;
        
        vertices[0] = new Vector3(-nearWidth / 2, -nearHeight / 2, near);
        vertices[1] = new Vector3(nearWidth / 2, -nearHeight / 2, near);
        vertices[2] = new Vector3(nearWidth / 2, nearHeight / 2, near);
        vertices[3] = new Vector3(-nearWidth / 2, nearHeight / 2, near);
        
        vertices[4] = new Vector3(-farWidth / 2, -farHeight / 2, far);
        vertices[5] = new Vector3(farWidth / 2, -farHeight / 2, far);
        vertices[6] = new Vector3(farWidth / 2, farHeight / 2, far);
        vertices[7] = new Vector3(-farWidth / 2, farHeight / 2, far);
        
        return vertices;
    }
}
