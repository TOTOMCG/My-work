using UnityEngine;

public class CullingTarget : MonoBehaviour
{
    public float radius = 0.5f;
    public Vector3 initScale;
    public void Start()
    {
        initScale = transform.localScale;
    }

    public void SetHighlighted(bool highlighted)
    {
        transform.localScale = highlighted ? initScale * 1.2f : initScale;
    }
}