using UnityEngine;

public class CreateCity : MonoBehaviour {
    [Header("Prefab Ayarlarý")]
    public GameObject[] roadsidePrefabs; // Yol kenarýna spawn edilecek objeler

    [Header("Spawn Ayarlarý")]
    public float spawnInterval = 10f; // Her kaç metrede bir obje spawn olsun (z ekseninde)
    public float xPositionLeft = -10f; // Sol taraf X pozisyonu
    public float xPositionRight = 10f; // Sað taraf X pozisyonu

    [Header("Referanslar")]
    public Transform player; // Oyuncu referansý

    private float nextSpawnZ = 0f; // Bir sonraki spawn pozisyonu

    void Start() {
        // Baþlangýçta biraz ileride spawn et
        nextSpawnZ = 20f;
    }

    void Update() {
        // Oyuncu spawn noktasýna yaklaþtýðýnda yeni objeler oluþtur
        if (player.position.z > nextSpawnZ - 50f) {
            SpawnRoadsideObjects();
            nextSpawnZ += spawnInterval;
        }
    }

    void SpawnRoadsideObjects() {
       
        GameObject leftPrefab = roadsidePrefabs[Random.Range(0, roadsidePrefabs.Length)];
        Vector3 leftPos = new Vector3(xPositionLeft, 0, nextSpawnZ);
        Instantiate(leftPrefab, leftPos, Quaternion.identity, transform);

        GameObject rightPrefab = roadsidePrefabs[Random.Range(0, roadsidePrefabs.Length)];
        Vector3 rightPos = new Vector3(xPositionRight, 0, nextSpawnZ);
        Instantiate(rightPrefab, rightPos, Quaternion.identity, transform);
    }
}