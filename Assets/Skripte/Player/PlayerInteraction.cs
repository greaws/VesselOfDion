using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] interactableObjectsSet1 = new GameObject[3];
    [SerializeField] private GameObject[] interactableObjectsSet2 = new GameObject[3];
    [SerializeField] private GameObject[] interactableObjectsSet3 = new GameObject[3];
    [SerializeField] private float interactionRange = 3f;
    private Animator animator;
    [SerializeField] private GameObject vesselObject1;
    [SerializeField] private GameObject vesselObject2;
    [SerializeField] private GameObject vesselObject3;

    private int objectsFoundSet1 = 0;
    private int objectsFoundSet2 = 0;
    private int objectsFoundSet3 = 0;
    private bool allObjectsCollectedSet1 = false;
    private bool allObjectsCollectedSet2 = false;
    private bool allObjectsCollectedSet3 = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Interaktion mit der ersten Gruppe
            foreach (GameObject obj in interactableObjectsSet1)
            {
                float distance = Vector2.Distance(player.position, obj.transform.position);
                if (distance <= interactionRange && obj.activeSelf)
                {
                    TriggerInteraction(obj, 1);
                    break;
                }
            }

            // Interaktion mit der zweiten Gruppe
            foreach (GameObject obj in interactableObjectsSet2)
            {
                float distance = Vector2.Distance(player.position, obj.transform.position);
                if (distance <= interactionRange && obj.activeSelf)
                {
                    TriggerInteraction(obj, 2);
                    break;
                }
            }

            // Interaktion mit der dritten Gruppe
            foreach (GameObject obj in interactableObjectsSet3)
            {
                float distance = Vector2.Distance(player.position, obj.transform.position);
                if (distance <= interactionRange && obj.activeSelf)
                {
                    TriggerInteraction(obj, 3);
                    break;
                }
            }

            // Überprüfen, ob der Spieler mit einem Vessel interagieren kann
            if (allObjectsCollectedSet1 && Vector2.Distance(player.position, vesselObject1.transform.position) < interactionRange)
            {
                TriggerVesselInteraction(1);
            }

            if (allObjectsCollectedSet2 && Vector2.Distance(player.position, vesselObject2.transform.position) < interactionRange)
            {
                TriggerVesselInteraction(2);
            }

            if (allObjectsCollectedSet3 && Vector2.Distance(player.position, vesselObject3.transform.position) < interactionRange)
            {
                TriggerVesselInteraction(3);
            }
        }
    }

    private void TriggerInteraction(GameObject obj, int setNumber)
    {
        animator.SetTrigger("inspect");
        obj.SetActive(false);

        if (setNumber == 1)
        {
            objectsFoundSet1++;
            if (objectsFoundSet1 == interactableObjectsSet1.Length)
            {
                allObjectsCollectedSet1 = true;
                Debug.Log("Alle Objekte in Set 1 gefunden! Du kannst jetzt mit Vessel 1 interagieren.");
            }
        }
        else if (setNumber == 2)
        {
            objectsFoundSet2++;
            if (objectsFoundSet2 == interactableObjectsSet2.Length)
            {
                allObjectsCollectedSet2 = true;
                Debug.Log("Alle Objekte in Set 2 gefunden! Du kannst jetzt mit Vessel 2 interagieren.");
            }
        }
        else if (setNumber == 3)
        {
            objectsFoundSet3++;
            if (objectsFoundSet3 == interactableObjectsSet3.Length)
            {
                allObjectsCollectedSet3 = true;
                Debug.Log("Alle Objekte in Set 3 gefunden! Du kannst jetzt mit Vessel 3 interagieren.");
            }
        }

        TextManager.Instance.UpdateProgress();
    }

    private void TriggerVesselInteraction(int vesselNumber)
    {
        Debug.Log("Mit Vessel " + vesselNumber + " interagiert! Spielerwechsel wird vorbereitet...");
        PlayerSwitcher.Instance.SwitchPlayer(vesselNumber);
    }
}
