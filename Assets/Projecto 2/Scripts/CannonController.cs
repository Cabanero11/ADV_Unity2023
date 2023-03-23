using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject trajectoryMarkerPrefab;
    public float bulletSpeed = 20f;
    public float trajectoryMarkerSpacing = 0.1f;

    private GameObject[] trajectoryMarkers;
    private int numTrajectoryMarkers = 30;
    private Vector3 velocity;

    void Start()
    {
        trajectoryMarkers = new GameObject[numTrajectoryMarkers];
        for (int i = 0; i < numTrajectoryMarkers; i++)
        {
            trajectoryMarkers[i] = Instantiate(trajectoryMarkerPrefab, transform.position, Quaternion.identity);
            trajectoryMarkers[i].SetActive(false);
        }
    }

    void Update()
    {
        // Move the cannon
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += new Vector3(horizontalInput, 0, verticalInput);

        // Rotate the cannon
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(new Vector3(mousePosition.x, transform.position.y, mousePosition.z));

        // Show the trajectory markers
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateTrajectory();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            HideTrajectory();
            FireBullet();
        }
    }

    void CalculateTrajectory()
    {
        Vector3 startPos = transform.position;
        Vector3 startVelocity = transform.forward * bulletSpeed;
        float timeStep = trajectoryMarkerSpacing / startVelocity.magnitude;

        for (int i = 0; i < numTrajectoryMarkers; i++)
        {
            float t = i * timeStep;
            Vector3 pos = startPos + startVelocity * t + Physics.gravity * t * t * 0.5f;
            trajectoryMarkers[i].SetActive(true);
            trajectoryMarkers[i].transform.position = pos;
        }
    }

    void HideTrajectory()
    {
        foreach (GameObject marker in trajectoryMarkers)
        {
            marker.SetActive(false);
        }
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;
    }
}
