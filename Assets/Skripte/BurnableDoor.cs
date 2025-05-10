using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableDoor : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Forscher>() != null)
        {
            animator.speed = 1;
            StartCoroutine(Burn());
        }
    }

    public IEnumerator Burn()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
