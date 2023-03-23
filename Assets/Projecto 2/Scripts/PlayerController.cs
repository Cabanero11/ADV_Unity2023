using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [Range(0.0f, 50.0f)]
    public float speed = 10.0f; // movement speed
    public GameObject bulletPrefab; // prefab to use for bullets
    public Transform bulletSpawn; // transform where bullets will spawn

    private float pitch = 0.0f; // pitch of the camera
    private float yaw = 0.0f; // yaw of the camera

    void Update()
    {
        // handle movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);

        // handle rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        yaw += mouseX;
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // handle shooting
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void OnGUI()
    {
        // draw crosshair in the middle of the screen
        int size = 12;
        float posX = Screen.width / 2 - size / 4;
        float posY = Screen.height / 2 - size / 4;
        GUI.Label(new Rect(posX, posY, size, size), "+");
    }

    void Shoot()
    {
        // create bullet and set its velocity
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * 20;
    }
}
