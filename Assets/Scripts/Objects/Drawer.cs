using UnityEngine;

public class Drawer : MonoBehaviour, IInteractable
{
    public GameObject drawer;
    public AudioClip openSound;
    public AudioClip closeSound;
    public CanvasGroup openText;
    public float startPos = 1.790596f;
    public float endPos = 3.386f;
    public float moveSpeed = 2f;
    public LayerMask obstacleMask;

    private AudioSource audioSource;
    private bool isClose = true;
    private bool isMoving = false;
    private Vector3 openPosition;
    private Vector3 closePosition;

    private void Start()
    {
        closePosition = new Vector3(startPos, drawer.transform.position.y, drawer.transform.position.z);
        openPosition = new Vector3(endPos, drawer.transform.position.y, drawer.transform.position.z);

        if (drawer.transform.position.x > startPos)
            isClose = false;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!isMoving)
        {
            if (isClose)    
                Open();
            else
                Close();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveDrawer();
        }
    }

    public void Open()
    {
        audioSource.PlayOneShot(openSound);
        isClose = false;
        isMoving = true;
    }

    public void Close()
    {
        audioSource.PlayOneShot(closeSound);
        isClose = true;
        isMoving = true;
    }

    private void MoveDrawer()
    {
        Vector3 targetPosition = isClose ? closePosition : openPosition;

        if (!IsBlocked(targetPosition))
        {
            drawer.transform.position = Vector3.MoveTowards(drawer.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (drawer.transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private bool IsBlocked(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - drawer.transform.position).normalized;
        float distance = Vector3.Distance(drawer.transform.position, targetPosition);

        if (Physics.Raycast(drawer.transform.position, direction, distance, obstacleMask))
        {
            return true;
        }

        return false;
    }
}
