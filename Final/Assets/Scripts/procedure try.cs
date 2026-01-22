using UnityEngine;

public class Procuderaltry : MonoBehaviour {
    public GameObject obstacleref;
    public float lastspawnposition = 20;

    public float distance;
    public GameObject PlayerC;
    private GameObject[] obstaclelist;


    void Start() {


    }

    // Update is called once per frame
    void Update() {
        if (lastspawnposition - PlayerC.transform.position.z < 30) {
            spawnobstacle();
            Debug.Log("çalışıyor");
        }


    }

    void spawnobstacle() {
        int rnd = Random.Range(-1, 1);
        GameObject spawnedobstacle = Instantiate(obstacleref, new Vector3(rnd * 4, 1, lastspawnposition), Quaternion.identity);
        lastspawnposition += distance;

    }
}
