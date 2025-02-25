using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Renderer vaseRen;

    private bool t,k;

    private void FixedUpdate()
    {
        if (t)
        {
            k = !k;
            vaseRen.material.SetInt("_invert", k?1:0);
        }
    }

    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EvadingPrometheus evadingPrometheus = collision.GetComponent<EvadingPrometheus>();
        if (evadingPrometheus != null) {
            evadingPrometheus.Hit(damage);
        }
    }

    public void Invert(int flag)
    {
        t = flag==1;
        if (flag==0)
        {
            vaseRen.material.SetInt("_invert", 0);
        }
    }
}
