using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class EvadingPrometheus : MonoBehaviour
{
    public float speed, radius, stun;
    private Animator _animator;
    private bool _canMove = true;
    public int lives;

    public float rollTime, rollSpeed;

    public TextMeshPro text;
    public string[] gameOverMessage;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        text.enabled = false;
        currentSpeed = speed;
        rollCooldown = 0;
    }
    float rollCooldown;
    float currentSpeed;
    // Update is called once per frame
    void Update()
    {
        if (!_canMove)
            return;
        float x = Input.GetAxis("Horizontal");
        _animator.SetBool("Run", x != 0);
        if (x < 0)
            transform.localScale = new Vector3 (-1, 1, 1);
        else if (x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        
        transform.localPosition += Vector3.right * x * Time.deltaTime * currentSpeed;

        if (rollCooldown > 0)
        {
            rollCooldown -= Time.deltaTime;
            
        }
        else
        {
            currentSpeed = speed;
            if (Input.GetButtonDown("Jump"))
            {
                _animator.SetTrigger("Roll");
                currentSpeed = rollSpeed;
                rollCooldown = rollTime;
            }
        }

        transform.localPosition = Vector3.right * Mathf.Clamp(transform.localPosition.x, -radius, radius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(hit());
    }
    private int i = 0;
    private IEnumerator hit()
    {
        i = (i + 1) % (gameOverMessage.Length - 1);
        text.enabled = true;
        text.text = gameOverMessage[i];
        lives--;
        _canMove = false;
        _animator.SetBool("Struck", true);
        yield return new WaitForSeconds(stun);
        _animator.SetBool("Struck",false);
        _canMove = true;
        text.enabled = false;
    }
}
