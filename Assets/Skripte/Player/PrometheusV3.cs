using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrometheusV3 : Player
{
    public DialogueUI DialogueUI { get; private set; }
    [SerializeField] private float moveSpeed = 5f; //! Movement speed of the player

    public IInteractable Interactable { get; set; }

    private void Awake()
    {
        DialogueUI = FindObjectOfType<DialogueUI>(); // Assign the active DialogueUI
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isPressingUp = Input.GetKey(KeyCode.W);
        bool isPressingDown = Input.GetKey(KeyCode.S);

        if (isPressingUp)
        {
            transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);
        }

        if (isPressingDown)
        {
            transform.Translate(Vector2.down * Time.deltaTime * moveSpeed);
        }
    }
}
