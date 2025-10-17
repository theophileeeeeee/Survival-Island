using UnityEngine;

public class POV_Camera : MonoBehaviour
{
    [Header("Target")]
    public Transform bearHead; // mettez ici la tête/transform de l'ours

    [Header("Follow")]
    public float positionSmooth = 8f;
    public float rotationSmooth = 8f;
    public Vector3 positionOffset = Vector3.zero;

    [Header("Head motion (subtil)")]
    public float bobAmplitude = 0.6f;
    public float bobFrequency = 1.5f;
    public float noiseStrength = 0.8f;
    public float noiseFrequency = 0.8f;

    [Header("FOV / Bear-sense")]
    public Camera cam;
    public float normalFOV = 60f;
    public float bearSenseFOV = 75f;
    public float fovSmooth = 4f;
    public KeyCode toggleKey = KeyCode.V;

    bool bearSenseActive = false;
    float fovTarget;

    void Start()
    {
        if (cam == null) cam = GetComponent<Camera>();
        fovTarget = normalFOV;
        if (cam != null) cam.fieldOfView = normalFOV;
    }

    void Update()
    {
        HandleInput();
        if (bearHead != null)
        {
            ApplyPositionAndRotation();
            ApplyHeadMotion();
        }
        ApplyFOV();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetBearSense(!bearSenseActive);
        }
    }

    public void SetBearSense(bool on)
    {
        bearSenseActive = on;
        fovTarget = on ? bearSenseFOV : normalFOV;
    }

    void ApplyPositionAndRotation()
    {
        Vector3 desiredPos = bearHead.position + bearHead.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-positionSmooth * Time.deltaTime));

        Quaternion desiredRot = bearHead.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, 1f - Mathf.Exp(-rotationSmooth * Time.deltaTime));
    }

    void ApplyHeadMotion()
    {
        // bob (sinus)
        float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        // perlin noise
        float nx = (Mathf.PerlinNoise(Time.time * noiseFrequency, 0f) - 0.5f) * 2f * noiseStrength;
        float nz = (Mathf.PerlinNoise(0f, Time.time * noiseFrequency) - 0.5f) * 2f * noiseStrength;

        // apply small local rotation offsets for organic feel
        Quaternion extra = Quaternion.Euler(bob + nx * 2f, 0f, nz * 2f);
        transform.rotation = transform.rotation * Quaternion.Slerp(Quaternion.identity, extra, 0.6f * Time.deltaTime * rotationSmooth * 5f);
    }

    void ApplyFOV()
    {
        if (cam != null)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fovTarget, 1f - Mathf.Exp(-fovSmooth * Time.deltaTime));
        }
    }
}
