using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PadLock : MonoBehaviour
{
    public static event Action<string, int> Rotated;

    private bool coroutineAllowed;
    private int numberShown;

    void Start()
    {
        coroutineAllowed = true;
        numberShown = 0;
        Debug.Log("PadLock script is active on " + gameObject.name);
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on " + gameObject.name);
        if (coroutineAllowed)
        {
            StartCoroutine(RotateWheel());
        }
    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;
        for (int i = 0; i <= 11; i++)
        {
            transform.Rotate(0f, 3f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;
        numberShown += 1;

        if (numberShown > 9)
        {
            numberShown = 0;
        }
        Rotated(name, numberShown);
    }
}
