using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class EvadingPrometheus : MonoBehaviour
{
    public float speed, radius, stun;
    private Animator _animator;
    private bool _canMove = true;
    public int lifes;

    public float rollTime, rollSpeed;

    public GameObject vase2;

    public TextMeshPro text;
    public string[] gameOverMessage;

    public GameObject forscher, door2;
    public HealthBar healthBar;
    public PlayableDirector timeline;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        //text.enabled = false;
        currentSpeed = speed;
        rollCooldown = 0;        
        Debug.Log("rfs");
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

    private void OnDisable()
    {
        forscher.GetComponent<Machete>().enabled = true;
        forscher.SetActive(true);
        door2.SetActive(false);
    }
  
    private int i = 0;

    public Vector2 shakeIntensity;

    private IEnumerator hit(int damage)
    {
        i = (i + 1) % (gameOverMessage.Length - 1);        
        text.text = gameOverMessage[i];
        text.gameObject.SetActive(true);
        lifes-= damage;
        healthBar.SetLifes(lifes);
        _canMove = false;
        _animator.SetBool("Struck", true);
        if (lifes <= 0)
        {
            timeline.Play();
        }
        else
        {
            yield return new WaitForSeconds(stun);
            _animator.SetBool("Struck", false);
            _canMove = true;
            text.gameObject.SetActive(false);
        }
    }

    internal void Hit(int damage)
    {
        if (_canMove)
            StartCoroutine(hit(damage));
    }
}
