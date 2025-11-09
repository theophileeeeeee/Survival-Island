using UnityEngine;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIPanels;
    [SerializeField]
    private ThirdPersonOrbitCamBasic playerCameraScript;

    private float defaultHorizontakAimingSpeed;
    private float defaultVerticalAimingSpeed;
    [HideInInspector] public bool AtLeastOnePanelActive;

    void Start()
    {
        defaultHorizontakAimingSpeed = 6;
        defaultVerticalAimingSpeed = 6;
    }

    void Update()
    {
        AtLeastOnePanelActive = UIPanels.Any((panel) => panel == panel.activeSelf);
        if (AtLeastOnePanelActive)
        {
            playerCameraScript.horizontalAimingSpeed = 0;
            playerCameraScript.verticalAimingSpeed = 0;
        }
        else
        {
            playerCameraScript.horizontalAimingSpeed = defaultHorizontakAimingSpeed;
    
            playerCameraScript.verticalAimingSpeed = defaultVerticalAimingSpeed;
        }
    }
}
