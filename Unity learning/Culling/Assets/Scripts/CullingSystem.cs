using UnityEngine;

public enum CullingType
{
    Sphere,
    Frustum
}

public class CullingSystem : MonoBehaviour
{
    [Header("Тип каллинга")]
    public CullingType cullingType;
    
    
    public DetectionCircle detectionCircle;
    public Frustum frustum;

    void Update()
    {
        if (cullingType == CullingType.Sphere)
        {
            UpdateCircle();
        }
        else if (cullingType == CullingType.Frustum)
        {
            UpdateFrustum();
        }
    }

    void UpdateCircle()
    {
        CullingTarget[] targets = FindObjectsByType<CullingTarget>(FindObjectsSortMode.None);
        
        foreach (CullingTarget target in targets)
        {
            float distanceToTarget = Vector3.Distance(detectionCircle.GetDetectionCenter(), target.transform.position);
            target.SetHighlighted(distanceToTarget <= detectionCircle.detectionRadius * 0.5f + target.radius);
        }
    }

    void UpdateFrustum()
    {
        Vector3[] vertices = frustum.ReturnVertices();
        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }

        Plane[] planes = new Plane[6];
        
        planes[0] = new Plane(vertices[0], vertices[1], vertices[2]);
        planes[1] = new Plane(vertices[6], vertices[5], vertices[4]);
        planes[2] = new Plane(vertices[4], vertices[0], vertices[3]);
        planes[3] = new Plane(vertices[1], vertices[5], vertices[6]);
        planes[4] = new Plane(vertices[0], vertices[4], vertices[5]);
        planes[5] = new Plane(vertices[2], vertices[6], vertices[7]);
        
        CullingTarget[] targets = FindObjectsByType<CullingTarget>(FindObjectsSortMode.None);
    
        foreach (CullingTarget target in targets)
        {
            bool flag = true;
            
            foreach (Plane plane in planes)
            {
                float distance = plane.GetDistanceToPoint(target.transform.position);
                if (distance < -target.radius)
                {
                    flag = false;
                }
            }
            target.SetHighlighted(flag);
        }
    } 
}
