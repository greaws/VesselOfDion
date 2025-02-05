using UnityEngine;

public class Zeus : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f; // Maximum movement distance (up and down)
    [SerializeField] private float speed = 1f;     // Speed of the movement

    public Vector3 chase, gameOver;

    public float smoothing;

    public Vector2 targetPosition = new Vector2(-4,7.5f);

    public Transform prometheus;
    private Animator animator;


    public bool dead = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!dead)
            targetPosition = new Vector2(-4.5f, prometheus.localPosition.y);        
        else
            targetPosition = new Vector3(0,10);
        //animator.SetBool("Dead", dead);
        float newY = Mathf.Lerp(transform.localPosition.y, targetPosition.y + 1.5f, Time.deltaTime * smoothing) + Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, targetPosition.x, Time.deltaTime * smoothing), newY, 0);
    }

    public void Reset()
    {
        dead = false;
        transform.localPosition = new Vector2(-7.5f, 7.5f);
    }
}
