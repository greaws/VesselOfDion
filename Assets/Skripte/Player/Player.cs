using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    protected Rigidbody2D rb;
    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

}
