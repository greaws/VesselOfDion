using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plattformSpawner : MonoBehaviour
{
    public GameObject plattform;
    public float range, speed, width;

    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(MovePlattform());
    }

    public IEnumerator MovePlattform()
    {
        while (true)
        {
            float t = 0;
            float y = Random.Range(-range, range);
            while (t < 1)
            {
                t += Time.deltaTime/ speed;
                plattform.transform.localPosition = new Vector2(Mathf.Lerp(width, -width, t), y);
                yield return null;
            }
            yield return new WaitForSeconds(0);
        }
    }
}
