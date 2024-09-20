using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWakeUpTrigger(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger("TrWakeUp");
        }
    }
}
