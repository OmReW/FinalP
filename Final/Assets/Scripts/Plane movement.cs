using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Planemovement : MonoBehaviour
{
    float randomSpeed = Random.Range(1, 3);
    void Start() {
        StartCoroutine(ObstacleSpeed());
    }

    IEnumerator ObstacleSpeed() {
        transform.DOLocalMoveX(randomSpeed, 1);
        yield return new WaitForSeconds(1);
        transform.DOLocalMoveX(randomSpeed, 1);
        yield return new WaitForSeconds(1);
        StartCoroutine(ObstacleSpeed());
    }
}
