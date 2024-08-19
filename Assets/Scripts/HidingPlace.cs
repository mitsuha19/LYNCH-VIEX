using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public GameObject hideText;
    public GameObject normalPlayer, hidingPlayer;
    public EnemyAI monsterScript;
    public Transform monsterTransform;
    public float InteractRange = 3f; // The range within which the player can interact
    public float loseDistance;

    private bool hiding, playerInTrigger;
    private const string WardrobeDoorTag = "WardrobeDoor"; // The tag to identify the wardrobe door

    void Start()
    {
        hiding = false;
        playerInTrigger = false;
        hideText.SetActive(false); // Hide the text by default
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            hideText.SetActive(false);
        }
    }

    void Update()
    {
        if (hiding)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                normalPlayer.SetActive(true);
                hidingPlayer.SetActive(false);
                hiding = false;
            }
            return; 
        }

        if (playerInTrigger && IsAimingAtWardrobeDoor(out _))
        {
            hideText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                hideText.SetActive(false);
                hidingPlayer.SetActive(true);

                float distance = Vector3.Distance(monsterTransform.position, normalPlayer.transform.position);
                if (distance > loseDistance && monsterScript.chasing)
                {
                    monsterScript.StopChase();
                }

                hiding = true;
                normalPlayer.SetActive(false);
            }
        }
        else
        {
            hideText.SetActive(false);
        }
    }

    private bool IsAimingAtWardrobeDoor(out Collider wardrobeDoorCollider)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.CompareTag(WardrobeDoorTag))
            {
                wardrobeDoorCollider = hitInfo.collider;
                return true;
            }
        }

        wardrobeDoorCollider = null;
        return false;
    }
}
