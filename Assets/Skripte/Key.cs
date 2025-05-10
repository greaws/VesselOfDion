using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Vector3 m_Position;
    public float amplitude, frequency;
    public Transform target;
    public float magnetism;
    public Transform sprite;
    public float effectspeed;

    void Update()
    {
        sprite.position = transform.position + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
        if (target!= null) 
        { 
            transform.position = Vector3.Lerp(transform.position, target.position + Vector3.up, Time.deltaTime * magnetism);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null && collision.CompareTag("Player"))
        {
            target = collision.transform;
            //GetComponent<Collider2D>().enabled = false;
            StartCoroutine(CollectEffect());
        }
        if (target != null && collision.GetComponent<Vase>()!= null)
        {
            print("hit");
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PutInPlace(collision));
        }
    }

    private IEnumerator CollectEffect()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime/ effectspeed;
            transform.localScale = Vector3.one * (1 + Mathf.Sin(t * Mathf.PI));
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    private IEnumerator PutInPlace(Collider2D target)
    {
        Vector3 startPos = transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target.bounds.center, t);
            yield return null;
        }
        target.GetComponent<Vase>().AddKey();
        Destroy(gameObject);
    }
}
