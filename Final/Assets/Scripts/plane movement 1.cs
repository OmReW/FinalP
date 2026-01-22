using UnityEngine;
using System.Collections;

public class planemovement1 : MonoBehaviour
{
    public float mm;
    void Start()
    
        {
            StartCoroutine(PlaneMovement());
        }

        IEnumerator PlaneMovement() {
            transform.Translate(Vector3.forward *mm);
            yield return new WaitForSeconds(2);
            transform.Translate(Vector3.forward * -mm);
            yield return new WaitForSeconds(2);
        transform.Translate(Vector3.forward * -mm);
        yield return new WaitForSeconds(2f);
            StartCoroutine(PlaneMovement());
        }
}
