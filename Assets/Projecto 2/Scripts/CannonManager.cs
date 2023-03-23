using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public GameObject cannonBallPrefab;
    [Range(1f, 2f)]
    public float forceSpeed = 1.1f;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    private const int N_TRAJECTORY_POINTS = 16;

    public Camera cam;


    private bool pressingMouse = false;

    private Vector3 initialVelocity;

    void Start()
    {
        //cam = Camera.main;

        lineRenderer.positionCount = N_TRAJECTORY_POINTS;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressingMouse = true;
            lineRenderer.enabled = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressingMouse = false;
            lineRenderer.enabled = false;
            Fire();
        }

        if (pressingMouse)
        {
            // coordinate transform screen > world
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
            mousePos.z = 0;

            // look at
            transform.LookAt(mousePos);

            initialVelocity = mousePos - firePoint.position;

            initialVelocity *= forceSpeed;

            UpdateLineRenderer();
        }
    }

    private void Fire()
    {
        // instantiate a cannon ball
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, Quaternion.identity);
        // apply some force
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(initialVelocity, ForceMode.Impulse);
        Destroy(cannonBall, 11f);
    }

    private void UpdateLineRenderer()
    {
        float g = Physics.gravity.magnitude;
        float velocity = initialVelocity.magnitude;
        float angle = Mathf.Atan2(initialVelocity.y, initialVelocity.x);

        Vector3 start = firePoint.position;

        float timeStep = 0.1f;
        float fTime = 0f;
        for (int i = 0; i < N_TRAJECTORY_POINTS; i++)
        {
            float dx = 1.5f * velocity * fTime * Mathf.Cos(angle);
            float dy = 1.5f * velocity * fTime * Mathf.Sin(angle) - (g * fTime * fTime / 2f) ;
            //Debug.Log("dx= " + dx + " dy = " + dy);
            Vector3 pos = new Vector3(start.x + dx, start.y + dy, 0);
            lineRenderer.SetPosition(i, pos);
            fTime += timeStep;
        }
    }
}
