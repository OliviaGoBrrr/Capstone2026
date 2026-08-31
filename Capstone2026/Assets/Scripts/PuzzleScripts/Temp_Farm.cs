using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Temp_Farm : MonoBehaviour
{
    public GameObject Boulder, Mask0, Mask1, Mask2, Mask3, Mask4, Mask5, DeadCrops, AliveCrops, ColliderForRiverInFarm;
    public Transform BoulderInitPosition;
    public bool hasntfiredyet = true;

    GameObject[] ArrayOfMasks; // It's been a hot second, this was the best i could do to get this to work.

    void Update()
    {
        if (Boulder.transform.position != BoulderInitPosition.position && hasntfiredyet)
        {
            hasntfiredyet = false;
            StartCoroutine(Flow());            
        }
    }

    private void Start()
    {
        ArrayOfMasks = new GameObject[6];
        ArrayOfMasks[0] = Mask0;
        ArrayOfMasks[1] = Mask1;
        ArrayOfMasks[2] = Mask2;
        ArrayOfMasks[3] = Mask3;
        ArrayOfMasks[4] = Mask4;
        ArrayOfMasks[5] = Mask5;
    }
    IEnumerator Flow()
    {
        foreach (GameObject Mask in ArrayOfMasks)
        {
            var startScale = Mask.transform.localScale;
            var endScale = Vector3.one * 0;
            var elapsed = 0f;

            while (elapsed < 0.5f)
            {
                var t = elapsed / 0.5f;
                Mask.transform.localScale = new Vector3(
                    Mathf.Lerp(startScale.x, 0f, t),
                    startScale.y, 
                    startScale.z);
                elapsed += Time.deltaTime;
                yield return null;
            }


            Mask.transform.localScale = endScale;
            yield return null;
        }
        DeadCrops.SetActive(false);
        AliveCrops.SetActive(true);
        ColliderForRiverInFarm.SetActive(true);
    }
}
