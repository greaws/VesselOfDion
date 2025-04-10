using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject);
        Forscher f = collision.gameObject.GetComponent<Forscher>();
        if (f != null && f.hasTorch)
        {
            anim.SetTrigger("Burn");
            StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
