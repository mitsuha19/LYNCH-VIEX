using UnityEngine;

public class NotifManager : MonoBehaviour
{
    [SerializeField] private Transform notifsParent;

    private GameObject activeMonologue = null;

    public void ActivateMonologue(GameObject monologue)
    {
        if (activeMonologue != null && activeMonologue != monologue)
        {
            activeMonologue.SetActive(false);
        }

        activeMonologue = monologue;
        activeMonologue.SetActive(true);
    }

    private void Update()
    {
        foreach (Transform child in notifsParent)
        {
            if (child.gameObject.activeSelf && child.gameObject != activeMonologue)
            {
                ActivateMonologue(child.gameObject);
            }
        }
    }
}
