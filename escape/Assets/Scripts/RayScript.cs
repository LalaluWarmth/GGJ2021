using UnityEngine;

/// <summary>
/// descript:射线反射
/// author: wushengnuo
/// </summary>
public class RayScript : MonoBehaviour
{
    public Transform p1;
    public Transform p2;
    private LayerMask _layerMask;
    private void Update()
    {
        RaycastHit hit;
        Vector3 direction = p2.position - p1.position;
        direction.Normalize();
        _layerMask = ~(1 << 8);
        if (Physics.Raycast(p1.position, direction, out hit,1000,_layerMask))
        {
            Debug.Log(hit.collider.name);
            rayPoint = hit.point;
            Vector3 reflect= Vector3.Reflect(direction, hit.normal);
            reflectRayEnd = reflect * 100+hit.point;
        }
    }
 
 
    Vector3 rayPoint;
    Vector3 reflectRayEnd;
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(p1.position, rayPoint);
        Gizmos.DrawLine(rayPoint, reflectRayEnd);
    }
 
 
 
}