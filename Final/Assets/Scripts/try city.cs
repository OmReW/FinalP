using UnityEngine;

public class tryCity : MonoBehaviour {
    public GameObject[] Cityref;
    public float lastspawnposition = 20;
    public int CityCount= 10;
    public float distance;
    public GameObject PlayerC;
    private GameObject[] CityList;


    void Start() {


    }

    // Update is called once per frame
    void Update() {
        if (lastspawnposition - PlayerC.transform.position.z < 30) {
            spawnCity();
            Debug.Log("çalışıyor");
        }


    }

    void spawnCity() {
        int rndCity = Random.Range(0, CityCount);
        int rnd = Random.Range(-1, 1);
        GameObject spawnedobstacle = Instantiate(Cityref[rndCity], new Vector3(rnd * 20, 1, lastspawnposition), Quaternion.identity);
        lastspawnposition += distance;

    }
}
