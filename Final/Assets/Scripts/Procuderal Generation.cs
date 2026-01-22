using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [Header("Player Referansı")]
    public Transform player;

    [Header("Obstacle Ayarları")]
    public GameObject obstaclePrefab;
    public float obstacleSpawnHeight = 0.61f;
    public float obstacleSpawnDistance = 15f;
    [Range(0f, 1f)]
    public float obstacleSpawnChance = 0.6f;
    public bool allowMultipleObstaclesPerRow = false;
    [Range(1, 3)]
    public int maxObstaclesPerRow = 2;

    [Header("City Ayarları")]
    public GameObject[] cityPrefabs;
    public float citySpawnDistance = 25f;
    [Range(0f, 1f)]
    public float citySpawnChance = 0.8f;
    public bool spawnCityOnBothSides = true;
    public float cityXOffset = 15f;
    public float cityHeight = 0f;
    public float cityRandomRotation = 30f;

    [Header("Beton Ayarları")]
    public GameObject beton; // Beton prefab'ı
    public float betonSpawnDistance = 20f; // Betonlar arası mesafe
    public float betonXOffset = 10f; // Betonların yoldan uzaklığı
    public float betonHeight = 0f; // Betonların Y pozisyonu
    public bool spawnBetonOnBothSides = true; // Her iki tarafta da beton

    [Header("Genel Ayarlar")]
    public float spawnDistanceAhead = 100f;
    public float despawnDistanceBehind = 50f;
    public int initialObstacleCount = 10;
    public int initialCityCount = 5;
    public int initialBetonCount = 8; // Başlangıçta kaç beton

    // Privates
    private float nextObstacleSpawnZ = 0f;
    private float nextCitySpawnZ = 0f;
    private float nextBetonSpawnZ = 0f;
    private float[] lanePositions = new float[] { -6f, -2f, 2f, 6f };
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player referansı atanmamış!");
            return;
        }

        nextObstacleSpawnZ = player.position.z + 15f;
        nextCitySpawnZ = player.position.z + 15f;
        nextBetonSpawnZ = player.position.z + 10f;

        // Başlangıç spawn'ları
        for (int i = 0; i < initialObstacleCount; i++)
        {
            SpawnObstacle();
        }

        for (int i = 0; i < initialCityCount; i++)
        {
            SpawnCity();
        }

        for (int i = 0; i < initialBetonCount; i++)
        {
            SpawnBeton();
        }
    }

    void Update()
    {
        if (player == null) return;

        // Obstacle spawn
        while (nextObstacleSpawnZ < player.position.z + spawnDistanceAhead)
        {
            SpawnObstacle();
        }

        // City spawn
        while (nextCitySpawnZ < player.position.z + spawnDistanceAhead)
        {
            SpawnCity();
        }

        // Beton spawn
        while (nextBetonSpawnZ < player.position.z + spawnDistanceAhead)
        {
            SpawnBeton();
        }

        CleanupOldObjects();
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogWarning("Obstacle prefab atanmamış!");
            nextObstacleSpawnZ += obstacleSpawnDistance;
            return;
        }

        if (Random.value > obstacleSpawnChance)
        {
            nextObstacleSpawnZ += obstacleSpawnDistance;
            return;
        }

        if (allowMultipleObstaclesPerRow)
        {
            SpawnMultipleObstacles();
        }
        else
        {
            SpawnSingleObstacle();
        }

        nextObstacleSpawnZ += obstacleSpawnDistance;
    }

    void SpawnSingleObstacle()
    {
        int randomLane = Random.Range(0, 4);
        float xPos = lanePositions[randomLane];

        Vector3 spawnPos = new Vector3(xPos, obstacleSpawnHeight, nextObstacleSpawnZ);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        Rigidbody[] allRigidbodies = obstacle.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        Rigidbody mainRb = obstacle.GetComponent<Rigidbody>();
        if (mainRb != null)
        {
            mainRb.useGravity = false;
            mainRb.isKinematic = true;
            mainRb.linearVelocity = Vector3.zero;
        }

        obstacle.transform.position = spawnPos;
        spawnedObjects.Add(obstacle);
    }

    void SpawnMultipleObstacles()
    {
        int obstacleCount = Random.Range(1, Mathf.Min(maxObstaclesPerRow + 1, 4));
        List<int> availableLanes = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < obstacleCount; i++)
        {
            if (availableLanes.Count == 0) break;

            int randomIndex = Random.Range(0, availableLanes.Count);
            int selectedLane = availableLanes[randomIndex];
            availableLanes.RemoveAt(randomIndex);

            float xPos = lanePositions[selectedLane];
            Vector3 spawnPos = new Vector3(xPos, obstacleSpawnHeight, nextObstacleSpawnZ);
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

            Rigidbody[] allRigidbodies = obstacle.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in allRigidbodies)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
            }

            Rigidbody mainRb = obstacle.GetComponent<Rigidbody>();
            if (mainRb != null)
            {
                mainRb.useGravity = false;
                mainRb.isKinematic = true;
                mainRb.linearVelocity = Vector3.zero;
            }

            obstacle.transform.position = spawnPos;
            spawnedObjects.Add(obstacle);
        }
    }

    void SpawnCity()
    {
        if (cityPrefabs == null || cityPrefabs.Length == 0)
        {
            nextCitySpawnZ += citySpawnDistance;
            return;
        }

        if (Random.value > citySpawnChance)
        {
            nextCitySpawnZ += citySpawnDistance;
            return;
        }

        float randomZOffset = Random.Range(-5f, 5f);

        if (spawnCityOnBothSides)
        {
            // SOL TARAF
            int leftCityIndex = Random.Range(0, cityPrefabs.Length);
            float leftX = -6f - cityXOffset;
            Vector3 leftPos = new Vector3(leftX, cityHeight, nextCitySpawnZ + randomZOffset);
            Quaternion leftRotation = Quaternion.Euler(0, Random.Range(-cityRandomRotation, cityRandomRotation), 0);
            GameObject leftCity = Instantiate(cityPrefabs[leftCityIndex], leftPos, leftRotation);
            spawnedObjects.Add(leftCity);

            // SAĞ TARAF
            int rightCityIndex = Random.Range(0, cityPrefabs.Length);
            float rightX = 6f + cityXOffset;
            Vector3 rightPos = new Vector3(rightX, cityHeight, nextCitySpawnZ + randomZOffset);
            Quaternion rightRotation = Quaternion.Euler(0, Random.Range(-cityRandomRotation, cityRandomRotation), 0);
            GameObject rightCity = Instantiate(cityPrefabs[rightCityIndex], rightPos, rightRotation);
            spawnedObjects.Add(rightCity);
        }
        else
        {
            int cityIndex = Random.Range(0, cityPrefabs.Length);
            bool isLeft = Random.value > 0.5f;
            float xPos = isLeft ? (-6f - cityXOffset) : (6f + cityXOffset);
            Vector3 spawnPos = new Vector3(xPos, cityHeight, nextCitySpawnZ + randomZOffset);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(-cityRandomRotation, cityRandomRotation), 0);
            GameObject city = Instantiate(cityPrefabs[cityIndex], spawnPos, rotation);
            spawnedObjects.Add(city);
        }

        nextCitySpawnZ += citySpawnDistance;
    }

    void SpawnBeton()
    {
        if (beton == null)
        {
            nextBetonSpawnZ += betonSpawnDistance;
            return;
        }

        if (spawnBetonOnBothSides)
        {
            // SOL TARAF - Betonlar city'lerden daha içeride (yola yakın)
            float leftX = -6f - betonXOffset;
            Vector3 leftPos = new Vector3(leftX, betonHeight, nextBetonSpawnZ);
            GameObject leftBeton = Instantiate(beton, leftPos, Quaternion.identity);
            spawnedObjects.Add(leftBeton);

            // SAĞ TARAF
            float rightX = 6f + betonXOffset;
            Vector3 rightPos = new Vector3(rightX, betonHeight, nextBetonSpawnZ);
            GameObject rightBeton = Instantiate(beton, rightPos, Quaternion.identity);
            spawnedObjects.Add(rightBeton);

            Debug.Log($"Beton spawn: Sol X={leftX}, Sağ X={rightX}, Z={nextBetonSpawnZ}");
        }
        else
        {
            // Tek taraf
            bool isLeft = Random.value > 0.5f;
            float xPos = isLeft ? (-6f - betonXOffset) : (6f + betonXOffset);
            Vector3 spawnPos = new Vector3(xPos, betonHeight, nextBetonSpawnZ);
            GameObject betonObj = Instantiate(beton, spawnPos, Quaternion.identity);
            spawnedObjects.Add(betonObj);
        }

        nextBetonSpawnZ += betonSpawnDistance;
    }

    void CleanupOldObjects()
    {
        float despawnZ = player.position.z - despawnDistanceBehind;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (spawnedObjects[i].transform.position.z < despawnZ)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        // Spawn alanı (yeşil)
        Gizmos.color = Color.green;
        Vector3 spawnStart = new Vector3(0, 0, player.position.z);
        Vector3 spawnEnd = new Vector3(0, 0, player.position.z + spawnDistanceAhead);
        Gizmos.DrawLine(spawnStart, spawnEnd);

        // Despawn alanı (kırmızı)
        Gizmos.color = Color.red;
        Vector3 despawnPoint = new Vector3(0, 0, player.position.z - despawnDistanceBehind);
        Gizmos.DrawLine(player.position, despawnPoint);

        // Lane pozisyonları (mavi)
        Gizmos.color = Color.blue;
        foreach (float laneX in lanePositions)
        {
            Vector3 laneStart = new Vector3(laneX, 0, player.position.z - 10);
            Vector3 laneEnd = new Vector3(laneX, 0, player.position.z + 50);
            Gizmos.DrawLine(laneStart, laneEnd);
        }

        // City spawn pozisyonları (sarı)
        Gizmos.color = Color.yellow;
        float leftX = -6f - cityXOffset;
        float rightX = 6f + cityXOffset;
        Vector3 leftCityPos = new Vector3(leftX, cityHeight, player.position.z + 20);
        Vector3 rightCityPos = new Vector3(rightX, cityHeight, player.position.z + 20);
        Gizmos.DrawWireSphere(leftCityPos, 2f);
        Gizmos.DrawWireSphere(rightCityPos, 2f);

        // Beton spawn pozisyonları (turuncu)
        Gizmos.color = Color.magenta;
        float leftBetonX = -6f - betonXOffset;
        float rightBetonX = 6f + betonXOffset;
        Vector3 leftBetonPos = new Vector3(leftBetonX, betonHeight, player.position.z + 15);
        Vector3 rightBetonPos = new Vector3(rightBetonX, betonHeight, player.position.z + 15);
        Gizmos.DrawWireCube(leftBetonPos, new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(rightBetonPos, new Vector3(1, 1, 1));
    }
}