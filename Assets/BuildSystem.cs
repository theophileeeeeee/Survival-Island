using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] private Structure[] structures;
    [SerializeField] Grid3D grid;
    [SerializeField] private Transform rotationRef;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private int structureMoveSpeed;
    [SerializeField] private Transform buildSystemPanel;
    [SerializeField] private GameObject buildingRequiredElements;
    private bool inPlace;
    private Structure currentStructure;
    private bool canBuild;
    private Vector3 finalPosition;
    private bool systemEnabled;

    void Awake()
    {
        currentStructure = structures.First();
        DisableSystem();
    }
    private void FixedUpdate()
    {
        if(!systemEnabled)
        {
            return;
        }
        canBuild = currentStructure.placementPrefab.GetComponentInChildren<CollisionDetectionEdge>().CheckConnection();
        finalPosition = grid.GetNearestPointOnGrid(transform.position);
        CheckPosition();
        RoundPlacementStructureRotation();
        UpdatePlacementStructureMaterial();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentStructure.structureType == StructureType.Stairs && systemEnabled)
            {
                DisableSystem();
            }
            else
            {
                ChangeStructure(GetStructureType(StructureType.Stairs));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentStructure.structureType == StructureType.Floor && systemEnabled)
            {
                DisableSystem();
            }
            else
            {
                ChangeStructure(GetStructureType(StructureType.Floor));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentStructure.structureType == StructureType.Wall && systemEnabled)
            {
                DisableSystem();
            }
            else
            {
                ChangeStructure(GetStructureType(StructureType.Wall));
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (inPlace && canBuild && systemEnabled && HasResources())
            {
                BuildStructure();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateStructure();
        }
    }
    void BuildStructure()
    {
        Instantiate(currentStructure.instantiatedPrefab, currentStructure.placementPrefab.transform.position, currentStructure.placementPrefab.transform.GetChild(0).transform.rotation);
        for(int y = 0; y < currentStructure.ressourcesCost.Length; y++)
        {  
            for (int i = 0; i < currentStructure.ressourcesCost[y].count; i++)
            {
                Inventory.instance.RemoveItem(currentStructure.ressourcesCost[y].itemData);
            }
        }
    }
    public void UpdateDisplayCosts()
    {
        foreach (Transform child in buildSystemPanel)
        {
            Destroy(child.gameObject);
        }
        foreach(ItemInInventory requiredRessource in currentStructure.ressourcesCost)
        {
            GameObject requiredElementsGO = Instantiate(buildingRequiredElements, buildSystemPanel);
            requiredElementsGO.GetComponent<buildingRequiredElements>().Setup(requiredRessource);
        }
    }
    bool HasResources()
    {
        buildingRequiredElements[] requiredElements = GameObject.FindObjectsOfType<buildingRequiredElements>();
        return requiredElements.All(requiredElements => requiredElements.hasRessource);
    }
    void DisableSystem()
    {
        systemEnabled = false;
        buildSystemPanel.gameObject.SetActive(false);
        currentStructure.placementPrefab.SetActive(false);
    }
    void RoundPlacementStructureRotation()
    {
        float yAngle = rotationRef.localEulerAngles.y;
        int roundedRotation = 0;
        if (yAngle > -45 && yAngle <= 45)
        {
            roundedRotation = 0;
        }
        else if (yAngle > 45 && yAngle <= 135)
        {
            roundedRotation = 90;
        }
        else if (yAngle > 135 && yAngle <= 225)
        {
            roundedRotation = 180;
        }
        else if( yAngle > 225 && yAngle <= 315)
        {
            roundedRotation = 270;
        }
        currentStructure.placementPrefab.transform.rotation = Quaternion.Euler(0, roundedRotation, 0);
    }
    void RotateStructure()
    {
        if (currentStructure.structureType != StructureType.Wall)
        {
             currentStructure.placementPrefab.transform.GetChild(0).Rotate(0, 90, 0);
        }
        }
    void UpdatePlacementStructureMaterial()
    {
        MeshRenderer placementMeshRenderer = currentStructure.placementPrefab.GetComponentInChildren<CollisionDetectionEdge>().meshRenderer;
        if (inPlace &&canBuild && HasResources())
        {
            placementMeshRenderer.material = blueMaterial;
        }
        else
        {
            placementMeshRenderer.material = redMaterial;
        }
    }
    void CheckPosition()
    {
        inPlace = currentStructure.placementPrefab.transform.position == finalPosition;
        if (inPlace == false)
        {
            SetPosition(finalPosition);
        }
    }
    void SetPosition(Vector3 targetPosition)
    {
        Transform placementPrefabTransform = currentStructure.placementPrefab.transform;
        Vector3 positionVelocity = Vector3.zero;
        if (Vector3.Distance(placementPrefabTransform.position, targetPosition) > 10)
        {
            placementPrefabTransform.position = targetPosition;
            return;
        }
        else
        {
            Vector3 newTargetPosition = Vector3.SmoothDamp(placementPrefabTransform.position, targetPosition, ref positionVelocity, 0, structureMoveSpeed);
            placementPrefabTransform.position = newTargetPosition;
        }
    }
    void ChangeStructure(Structure newStructure)
    {
        systemEnabled = true;
        buildSystemPanel.gameObject.SetActive(true);
        currentStructure = newStructure;
        foreach (var structure in structures)
        {
            structure.placementPrefab.SetActive(structure.structureType == currentStructure.structureType);
        }
        UpdateDisplayCosts();
    }
    private Structure GetStructureType(StructureType structureType)
    {
        return structures.Where(elem => elem.structureType == structureType).FirstOrDefault();
    }
}

[System.Serializable]
public class Structure
{
    public GameObject placementPrefab;
    public GameObject instantiatedPrefab;
    public StructureType structureType;
    public ItemInInventory[] ressourcesCost;
}
public enum StructureType
{
    Stairs,
    Wall,
    Floor
}