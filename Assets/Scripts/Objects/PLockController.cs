using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLockController : MonoBehaviour
{
    private int[] result, correctCombination;
    void Start()
    {
        result = new int[] { 0, 0, 0, 0 };
        correctCombination = new int[] { 3, 7, 9, 1 };
        PadLock.Rotated += CheckResults;
    }

    private void CheckResults(string wheelName, int number)
    {
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
