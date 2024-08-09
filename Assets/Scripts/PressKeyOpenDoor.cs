using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyOpenDoor : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject AnimeObject;
    public GameObject ThisTrigger;
    public AudioSource DoorOpenSound;
    public AudioSource DoorCloseSound; 
    public bool Action = false;
    bool isOpen = false;

    void Start()
    {
        Instruction.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instruction.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instruction.SetActive(false);
            Action = false;
        }
    }

   
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && Action)
            {
                if (!isOpen)
                {
                    Debug.Log("Opening the door"); 
                    //Instruction.SetActive(false);
                    AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                    //ThisTrigger.SetActive(false);
                    //DoorOpenSound.Play();
                    isOpen = true;
                }
                else
                {
                    Debug.Log("Closing the door"); 
                    //Instruction.SetActive(false);
                    AnimeObject.GetComponent<Animator>().Play("DoorClose");
                    //ThisTrigger.SetActive(true);
                    //DoorCloseSound.Play();
                    isOpen = false;
                }
            }
        }

    
}
