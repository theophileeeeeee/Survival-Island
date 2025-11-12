using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetectionEdge : MonoBehaviour
{
    [SerializeField] private float radius;
    private Collider[] hitColliders;
    [SerializeField] private Vector3 offset;
    [SerializeField] PointDetectionEdge[] detectionPoints;
    public MeshRenderer meshRenderer;
    public bool CheckConnection()
    {
        hitColliders = Physics.OverlapSphere(transform.position + offset, radius);
        if (hitColliders.Length > 0)
        {
            if (hitColliders.Any(collider => collider.CompareTag(transform.tag)))
            {
                return false;
            }
            else if(hitColliders.Any(collider => collider.CompareTag("Terrain")))
            {
                return true;
            }
        }
            foreach (var point in detectionPoints)
            {
            point.CheckOverlap();
                if(point.connected)
                {
                    return true;
                }
            }
        
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
