using UnityEngine;
using System.Collections;

public class HeadIKController : MonoBehaviour
{
    [SerializeField] public Transform lookTarget;
    [SerializeField] GameObject objPivot;
    private Animator animator;
    public bool ikActive = false;
    [Range(0f, 1f)] public float lookWeight = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        objPivot.transform.LookAt(lookTarget);
        float pivotRotY = objPivot.transform.localRotation.y;

        float dist = Vector3.Distance(objPivot.transform.position, lookTarget.position);

        if (pivotRotY < 0.60f && pivotRotY > -0.60f && dist < 15f)
        {
            lookWeight = Mathf.Lerp(lookWeight, 1, Time.deltaTime * 2.5f);
        }

        else
        {
            lookWeight = Mathf.Lerp(lookWeight, 0, Time.deltaTime * 2.5f);
        }
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive && lookTarget != null)
            {
                animator.SetLookAtWeight(lookWeight);
                animator.SetLookAtPosition(lookTarget.position);
            }

            else
            {
                animator.SetLookAtWeight(0f);
            }
        }
    }
}
