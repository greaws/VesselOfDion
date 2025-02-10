using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EvadingPrometheus evadingPrometheus = collision.GetComponent<EvadingPrometheus>();
        if (evadingPrometheus != null) {
            evadingPrometheus.Hit(damage);
        }
    }
}
