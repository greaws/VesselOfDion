using Unity.VisualScripting;
using UnityEngine;

public class Machete : MonoBehaviour
{
    private Collision coll;
    private AnimationScript anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<Collision>();
        anim = GetComponentInChildren<AnimationScript>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (coll.onGround)
                anim.SetTrigger("Attack");
        }
    }
}
