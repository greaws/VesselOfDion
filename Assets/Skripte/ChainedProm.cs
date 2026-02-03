using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class ChainedProm : MonoBehaviour
{
    private Animator animator;
    public int life = 5;
    private CinemachineImpulseSource impulseSource;

    public GameObject vase3;

    [Header("Delayed Activation")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private float activationDelay = 2f; // seconds

    void Start()
    {
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
        life--;
        impulseSource.GenerateImpulse();

        if (life < 0)
        {
            vase3.SetActive(false);
        }
    }

    // CALLED BY EAGLE
    public void TriggerDone()
    {
        animator.SetTrigger("done");

        if (objectToActivate != null)
        {
            StartCoroutine(ActivateAfterDelay());
        }
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        objectToActivate.SetActive(true);
        objectToDeactivate.SetActive(false);
    }
}
