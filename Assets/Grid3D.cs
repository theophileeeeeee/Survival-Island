using UnityEngine;

public class Grid3D : MonoBehaviour
{
    [SerializeField] public float sizeX = 7.2f;
    [SerializeField] public float sizeY = 6.0f;
    [SerializeField] public float sizeZ = 7.2f;
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        int xCount = Mathf.RoundToInt(position.x / sizeX);
        int yCount = Mathf.RoundToInt(position.y / sizeY);
        int zCount = Mathf.RoundToInt(position.z / sizeZ);
        return new Vector3(xCount * sizeX, yCount * sizeY, zCount * sizeZ);
    }
}
