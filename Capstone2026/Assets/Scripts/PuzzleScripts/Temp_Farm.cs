using System.Collections;
using UnityEngine;

public class Temp_Farm : MonoBehaviour
{
    public GameObject Boulder, River_A, River_B, River_C, CropsA_A, CropsA_B, CropsA_C, CropsD_A, CropsD_B, CropsD_C;
    public Transform BoulderInitPosition;
    public bool hasntfiredyet = true;

    void Update()
    {
        if (Boulder.transform.position != BoulderInitPosition.position && hasntfiredyet)
        {
            hasntfiredyet = false;
            StartCoroutine(Flow());            
        }
    }

    IEnumerator Flow()
    {
        River_A.SetActive(true);
        CropsA_A.SetActive(true);
        CropsD_A.SetActive(false);
        CropsA_B.SetActive(true);
        CropsD_B.SetActive(false);
        CropsA_C.SetActive(true);
        CropsD_C.SetActive(false);
        yield return new WaitForSeconds(2f);
        River_B.SetActive(true);
        yield return new WaitForSeconds(1f);
        River_C.SetActive(true);
        yield return new WaitForSeconds(1f);
    }
}
