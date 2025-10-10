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

    void Start()
    {
        defaultHorizontakAimingSpeed = 6;
        defaultVerticalAimingSpeed = 6;
    }

    void Update()
    {
        if (UIPanels.Any((panel) => panel == panel.activeSelf))
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
