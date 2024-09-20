using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    [SerializeField] private Transform monologuesParent;

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
        foreach (Transform child in monologuesParent)
        {
            if (child.gameObject.activeSelf && child.gameObject != activeMonologue)
            {
                ActivateMonologue(child.gameObject);
            }
        }
    }
}
