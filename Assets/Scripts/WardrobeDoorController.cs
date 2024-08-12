using UnityEngine;

public class WardrobeAndDrawerController : MonoBehaviour
{
    public GameObject rightWardrobe; // Objek pintu lemari kanan
    public GameObject leftWardrobe;  // Objek pintu lemari kiri
    public GameObject rightDrawer;   // Objek laci kanan
    public GameObject leftDrawer;    // Objek laci kiri
    public string playerTag = "Player"; // Tag untuk mendeteksi pemain

    private bool isRightDoorOpen = false;
    private bool isLeftDoorOpen = false;
    private bool isRightDrawerOpen = false;
    private bool isLeftDrawerOpen = false;
    private bool isPlayerInRange = false; // Menyimpan status apakah pemain berada di area kontrol

    void Update()
    {
        if (isPlayerInRange)
        {
            // Buka atau tutup pintu kanan
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isRightDoorOpen)
                {
                    rightWardrobe.GetComponent<Animator>().Play("LemariKananClose");
                    isRightDoorOpen = false;
                }
                else
                {
                    rightWardrobe.GetComponent<Animator>().Play("LemariKanan");
                    isRightDoorOpen = true;
                }
            }

            // Buka atau tutup pintu kiri
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isLeftDoorOpen)
                {
                    leftWardrobe.GetComponent<Animator>().Play("LemariKiriClose");
                    isLeftDoorOpen = false;
                }
                else
                {
                    leftWardrobe.GetComponent<Animator>().Play("LemariKiri");
                    isLeftDoorOpen = true;
                }
            }

            // Buka atau tutup laci kanan
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (isRightDrawerOpen)
                {
                    rightDrawer.GetComponent<Animator>().Play("LaciKananClose");
                    isRightDrawerOpen = false;
                }
                else
                {
                    rightDrawer.GetComponent<Animator>().Play("LaciKanan");
                    isRightDrawerOpen = true;
                }
            }

            // Buka atau tutup laci kiri
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isLeftDrawerOpen)
                {
                    leftDrawer.GetComponent<Animator>().Play("LaciKiriClose");
                    isLeftDrawerOpen = false;
                }
                else
                {
                    leftDrawer.GetComponent<Animator>().Play("LaciKiri");
                    isLeftDrawerOpen = true;
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
