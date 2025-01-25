using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedProm : MonoBehaviour
{
    private Animator animator;
    public int life = 5;
    private CinemachineImpulseSource impulseSource;
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
