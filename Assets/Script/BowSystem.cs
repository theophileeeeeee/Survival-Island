using UnityEngine;
using System.Collections;

public class BowSystem : MonoBehaviour
{
    public Animator bowAnimator;
    public Animator playerAnimator;
    public Transform arrowSpawnPoint;
    public GameObject arrowPrefab;
    public AimBehaviourBasic aimBehaviour;
    public GameObject bowVisual;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioSource audioSource;
    public LineRenderer trajectoryLine;

    [Header("Trajectory Settings")]
    public int maxSteps = 200;
    public float timeStep = 0.02f;
    public float arrowSpeed = 40f;

    [Header("Angle Offset")]
    public float angleOffset = 5f;

    [Header("Angle Limit")]
    public float maxAngleFromForward = 90f;

    [HideInInspector] public bool isAiming = false;
    bool arrowLoaded = false;

    // --- Ajouts pour délai + validation tir ---
    bool trajectoryStarted = false;
    bool trajectoryReady = false;     
    public float trajectoryDelay = 0.3f;
    // ------------------------------------------

    void Update()
    {
        if (!bowVisual.activeSelf)
        {
            if (isAiming)
                StopAiming();

            aimBehaviour.enabled = true;
            trajectoryLine.positionCount = 0;
            return;
        }

        // Début visée
        if (Input.GetMouseButtonDown(1))
        {
            StartAiming();
            StartCoroutine(TrajectoryRoutine());
            StartCoroutine(aimBehaviour.ToggleAimOff());
        }
        if (isAiming)
        {
            DrawTrajectory();
            StartCoroutine(aimBehaviour.ToggleAimOff());
        }

        // Fin visée
        if (Input.GetMouseButtonUp(1))
        {
            StopAiming();
            StartCoroutine(aimBehaviour.ToggleAimOff());
        }

        // --- Tir protégé : seulement si trajectoire prête ---
        if (Input.GetMouseButtonDown(0) && isAiming && arrowLoaded && trajectoryReady)
        {
            Vector3 dir = Quaternion.Euler(
                Camera.main.transform.eulerAngles.x,
                Camera.main.transform.eulerAngles.y,
                0) * Vector3.forward;
            
            if (Camera.main != null)
                dir = Quaternion.AngleAxis(angleOffset, Camera.main.transform.up) * dir;

            if (IsValidShootingAngle(dir))
            {
                arrowLoaded = false;

                bowAnimator.SetTrigger("BowShoot");
                playerAnimator.SetTrigger("BowShoot");
                ShootArrow();

                StopAiming();
            }
        }
    }
    void StartAiming()
    {
            isAiming = true;
            arrowLoaded = true;

            bowAnimator.SetBool("BowTension", true);
            playerAnimator.SetBool("BowTension", true);
            aimBehaviour.enabled = false;

            // Reset trajectoire
            trajectoryStarted = false;
            trajectoryReady = false;
    }

    void StopAiming()
    {
        isAiming = false;
        arrowLoaded = false;

        trajectoryStarted = false;
        trajectoryReady = false;

        bowAnimator.SetBool("BowTension", false);
        playerAnimator.SetBool("BowTension", false);

        aimBehaviour.enabled = false;
        trajectoryLine.positionCount = 0;

        StopAllCoroutines();
    }

    bool IsValidShootingAngle(Vector3 shootDirection)
    {
        Vector3 playerForward = transform.forward;
        float angle = Vector3.Angle(playerForward, shootDirection);
        return angle <= maxAngleFromForward;
    }

    void ShootArrow()
    {
        audioSource.PlayOneShot(shootSound);
        float camX = Camera.main.transform.eulerAngles.x;
        float camY = Camera.main.transform.eulerAngles.y;
        Vector3 baseDir = Quaternion.Euler(camX, camY, 0) * Vector3.forward;

        if (Camera.main != null)
            baseDir = Quaternion.AngleAxis(angleOffset, Camera.main.transform.up) * baseDir;

        Quaternion arrowRotation = Quaternion.LookRotation(baseDir) * Quaternion.Euler(90, 0, 0);
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowRotation);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.linearVelocity = baseDir * arrowSpeed;
    }

    //----------------------------------
    // TRAJECTOIRE (COROUTINE + CALCUL)
    //----------------------------------

    IEnumerator TrajectoryRoutine()
    {
        trajectoryStarted = true;
        trajectoryReady = false;

        // délai avant affichage
        yield return new WaitForSeconds(trajectoryDelay);

        trajectoryReady = true;

        while (isAiming)
        {
            DrawTrajectory();
            yield return null;
        }
    }

    void DrawTrajectory()
    {
        Vector3 pos = arrowSpawnPoint.position;

        Vector3 dir = Quaternion.Euler(
            Camera.main.transform.eulerAngles.x,
            Camera.main.transform.eulerAngles.y,
            0) * Vector3.forward;

        Vector3 baseDir = dir;

        if (Camera.main != null)
            dir = Quaternion.AngleAxis(angleOffset, Camera.main.transform.up) * baseDir;

        if (!IsValidShootingAngle(dir))
        {
            trajectoryLine.positionCount = 0;
            return;
        }

        Vector3 vel = dir * arrowSpeed;
        Vector3 gravity = Physics.gravity;

        Vector3[] points = new Vector3[maxSteps];
        int count = 0;

        for (int i = 0; i < maxSteps; i++)
        {
            points[count] = pos;
            count++;

            if (Physics.Raycast(pos, vel.normalized, out RaycastHit hit, vel.magnitude * timeStep))
            {
                points[count] = hit.point;
                count++;
                break;
            }

            pos += vel * timeStep;
            vel += gravity * timeStep;
        }

        trajectoryLine.positionCount = count;
        trajectoryLine.SetPositions(points);
    }
}
