using UnityEngine;
using UnityEngine.UI;

public class PadLockRay : MonoBehaviour
{
    public GameObject padlockTextObject;
    private FirstPersonController playerController;

    void Start()
    {
        padlockTextObject.SetActive(false);

        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<FirstPersonController>();

    }
}
