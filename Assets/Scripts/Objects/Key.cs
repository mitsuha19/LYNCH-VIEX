using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public string keyID;  
    public AudioClip pickupSound; 
    public GameObject keyText;
    private MeshCollider MeshCollider;
    private MeshRenderer Mesh;
    public GameObject keyPickedUpText;
    public GameObject glowing;
    public GameObject monolouge;

    private AudioSource audioSource;

    void Start()
    {
        monolouge.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        MeshCollider = GetComponent<MeshCollider>();
        Mesh = GetComponent<MeshRenderer>();
        keyText.SetActive(false);
        keyPickedUpText.SetActive(false);
    }

    public void Interact()
    {
        PickupKey();
        glowing.SetActive(false);
        monolouge.SetActive(true);
    }

    private void PickupKey()
    {
        Inventory.AddItem(keyID); 
        audioSource.PlayOneShot(pickupSound);
        keyPickedUpText.SetActive(true);
        MeshCollider.enabled = false;
        Mesh.enabled = false;
    }
}
