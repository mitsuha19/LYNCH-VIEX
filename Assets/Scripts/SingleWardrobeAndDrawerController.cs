using UnityEngine;

public class Single : MonoBehaviour
{
    public GameObject wardrobe; // Objek pintu lemari
    public GameObject drawer;   // Objek laci
    public string playerTag = "Player"; // Tag untuk mendeteksi pemain

    private bool isDoorOpen = false;
    private bool isDrawerOpen = false;
    private bool isPlayerInRange = false; // Menyimpan status apakah pemain berada di area kontrol

    void Update()
    {
        if (isPlayerInRange)
        {
            // Buka atau tutup pintu lemari
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isDoorOpen)
                {
                    wardrobe.GetComponent<Animator>().Play("Close");
                    isDoorOpen = false;
                }
                else
                {
                    wardrobe.GetComponent<Animator>().Play("Open");
                    isDoorOpen = true;
                }
            }

            // Buka atau tutup laci
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (isDrawerOpen)
                {
                    drawer.GetComponent<Animator>().Play("CloseDraw");
                    isDrawerOpen = false;
                }
                else
                {
                    drawer.GetComponent<Animator>().Play("OpenDraw");
                    isDrawerOpen = true;
                }
            }
        }
    }

    // Memeriksa jika pemain masuk ke area kontrol
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
        }
    }

    // Memeriksa jika pemain keluar dari area kontrol
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
        }
    }
}
