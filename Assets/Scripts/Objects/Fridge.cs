using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour, IInteractable
{
    public GameObject interactText;
    public GameObject monolougeText;

    void Start()
    {
        monolougeText.SetActive(false);
        interactText.SetActive(false);
    }

    public void Interact()
    {
        monolougeText.SetActive(false);
        monolougeText.SetActive(true);
    }
}