using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelGoal : MonoBehaviour
{
    public GameObject prometheus;
    public Forscher player;
    public PlayableDirector timeline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == prometheus)
        {
            print("Level complete");
            StartCoroutine(Reverse());
        }
    }

    private IEnumerator Reverse()
    {
        player.torch.SetActive(true);
        player.SetHasTorch(true);
        print("Level rtzhjtuk");

        float dt = (float)timeline.duration;
        print("Level ihlihl: " + dt);
        while (dt > 0)
        {
            dt -= Time.deltaTime;
            timeline.time = Mathf.Max(dt, 0);
            timeline.Evaluate();
            yield return null;
        }
        print("Level ertgh");
        timeline.time = 0;
        timeline.Evaluate();
        timeline.Stop();        
    }
}
