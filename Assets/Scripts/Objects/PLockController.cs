using UnityEngine;

public class PLockController : MonoBehaviour
{
    public Camera playerCamera;  // Kamera utama
    public Camera camPadlock;    // Kamera padlock
    public GameObject player;  // Kontrol pemain
    public float interactionRange = 3f;  // Jarak maksimum untuk berinteraksi
    public LayerMask padlockLayer;  // Layer untuk objek padlock
    public PadLockRay padlock;

    private int[] result, correctCombination;
    private bool isInteractingWithPadlock = false;
    private RaycastHit hit;

    void Start()
    {
        result = new int[] { 0, 0, 0, 0 };
        correctCombination = new int[] { 3, 7, 9, 1 };

        PadLock.Rotated += CheckResults;
        camPadlock.gameObject.SetActive(false);
    }

    void Update()
    {
        // Raycast dari kamera pemain
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange, padlockLayer))
        {
            // Jika pemain menekan tombol E dan raycast mengenai padlock
            if (Input.GetKeyDown(KeyCode.E) && !isInteractingWithPadlock)
            {
                // Ganti ke kamera padlock
                StartPadlockInteraction();
            }
        }

        // Keluar dari mode padlock ketika pemain menekan 'Esc'
        if (Input.GetKeyDown(KeyCode.Escape) && isInteractingWithPadlock)
        {
            ExitPadlockInteraction();
        }

        if (isInteractingWithPadlock)
            padlock.padlockTextObject.SetActive(false);
    }

    private void StartPadlockInteraction()
    {
        // Matikan kamera pemain dan aktifkan kamera padlock
        playerCamera.gameObject.SetActive(false);
        camPadlock.gameObject.SetActive(true);

        // Nonaktifkan kontrol pemain
        player.gameObject.SetActive(false);

        // Aktifkan kursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isInteractingWithPadlock = true;
    }

    private void ExitPadlockInteraction()
    {
        // Aktifkan kembali kamera pemain dan matikan kamera padlock
        camPadlock.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        // Aktifkan kembali kontrol pemain
        player.gameObject.SetActive(true);

        // Kembalikan kursor ke kondisi terkunci
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isInteractingWithPadlock = false;
    }

    private void CheckResults(string wheelName, int number)
    {
        Debug.Log($"Gear Rotated: {wheelName} to {number}");
        switch (wheelName)
        {
            case "FirstGear":
                result[0] = number;
                break;
            case "SecondGear":
                result[1] = number;
                break;
            case "ThirdGear":
                result[2] = number;
                break;
            case "FourthGear":
                result[3] = number;
                break;
        }

        Debug.Log($"Current Result: {result[0]}, {result[1]}, {result[2]}, {result[3]}");

        if (result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2] && result[3] == correctCombination[3])
        {
            Debug.Log("Opened!");
        }
    }

    private void OnDestroy()
    {
        PadLock.Rotated -= CheckResults;
    }
}
