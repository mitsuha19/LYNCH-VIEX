using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour, IInteractable
{
    public GameObject readText;
    public GameObject monolougeText;

    void Start()
    {
        monolougeText.SetActive(false);
        readText.SetActive(false);
    }

    public void Interact()
    {
        monolougeText.SetActive(false);
        monolougeText.SetActive(true);
    }
}