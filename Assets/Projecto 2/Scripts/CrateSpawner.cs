using UnityEngine;

public class CrateSpawner : MonoBehaviour
{

    /** 
    Explanation:
    The script has several public variables that you can adjust in the inspector:
    "cratePrefab": the prefab to use for spawning the crates.
    "minInterval" and "maxInterval": the minimum and maximum time between crate spawns.
    "crateLifetime": how long the crates should exist before being destroyed.
    "minSpeed" and "maxSpeed": the minimum and maximum speed at which the crates should fall.
    "maxDistance": the maximum distance from the spawner that the crates should appear.
    The script keeps track of when the next crate should spawn using the "nextSpawnTime" variable. In the "Update" 
    method, it checks if the current time is greater than or equal to "nextSpawnTime" and if so, spawns a crate 
    and sets the "nextSpawnTime" to a new random value.

    The "SpawnCrate" method generates a random position within "maxDistance" of the spawner's position, with a random y-coordinate 
    at the spawner's height. It instantiates the crate prefab at this position and applies a downward force with a random speed. 
    Finally, it schedules the crate for destruction after "crateLifetime" seconds using the "Destroy" method.
    */


    [Header("Variables del spawneo")]
    public GameObject[] cratesPrefab;

    [Range(0f,5f)]
    public float minInterval = 2f;
    [Range(0f, 5f)]
    public float maxInterval = 5f;
    [Range(1f, 20f)]
    public float crateLifetime = 10f;
    [Range(1f, 10f)]
    public float minSpeed = 2f;
    [Range(1f, 10f)]
    public float maxSpeed = 6f;
    [Range(1f, 12f)]
    public float maxDistance = 10f;

    [Space(20)]

    [Header("Sonido de bola chochando con caja")]
    public AudioClip audioClip;
    private AudioSource audioSource;


    private float nextSpawnTime;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        nextSpawnTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCrate();
            PlayCrateSound();

            nextSpawnTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    private void SpawnCrate()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * maxDistance;
        spawnPosition.y = transform.position.y;
        spawnPosition.z = transform.position.z;

        int randomIndex = Random.Range(0, cratesPrefab.Length); // get a random index from the array
        GameObject cratetToSpawn = cratesPrefab[randomIndex]; // get the game object at the random index

        GameObject crate = Instantiate(cratetToSpawn, spawnPosition, Quaternion.identity);
        Rigidbody crateRb = crate.GetComponent<Rigidbody>();

        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 force = Vector3.down * speed;
        crateRb.AddForce(force, ForceMode.Impulse);

        Destroy(crate, crateLifetime);
    }

    private void PlayCrateSound()
    {
        audioSource.Play();
    }
}
