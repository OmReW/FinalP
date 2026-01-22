using UnityEngine;
using System.Collections.Generic;

public class CreateRoad : MonoBehaviour
{

    // Yeni başlayanlar genelde [SerializeField] yerine direkt public kullanır.
    public GameObject roadPrefab;
    public Transform player;

    // Listemiz (Yolları tutacak)
    public List<GameObject> roads = new List<GameObject>();

    float timer = 0f;

    void Update()
    {
        // Zaman sayacı
        timer += Time.deltaTime;

        // 1 saniye dolduysa yol yap (spawnTime değişkeni yerine direkt 1 yazdım)
        if (timer >= 1f)
        {
            MakeRoad();
            timer = 0;
        }
    }

    void MakeRoad()
    {
        // Eğer oyuncu veya yol yoksa hata vermesin diye basit kontrol
        if (player == null) return;

        // Yeni pozisyonu hesapla (Senin sayılarını aynen korudum)
        Vector3 newPos = new Vector3(0, 0.785f, player.position.z + 120f);

        // Orijinalindeki karışık Quaternion yerine, anlaşılır olan "Euler" kullandım.
        // (0, 180, 0) demek Y ekseninde 180 derece dönmüş demek.
        GameObject yol = Instantiate(roadPrefab, newPos, Quaternion.Euler(0, 270, 0));

        // Listeye ekle
        roads.Add(yol);

        // Eğer 15 taneden fazla yol varsa en eskisini sil
        if (roads.Count > 15)
        {
            Destroy(roads[0]); // Sahneden sil
            roads.RemoveAt(0); // Listeden sil
        }
    }
}