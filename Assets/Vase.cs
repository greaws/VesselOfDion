using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public int missingPieces = 3;
    public Animator poof;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Key>()!= null)
        {

        }
    }

    public void AddKey()
    {
        poof.SetTrigger("Poof");
        missingPieces--;
        if (missingPieces == 0)
        {
            print("complete");
        }
    }
}
