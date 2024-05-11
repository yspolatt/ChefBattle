using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : MonoBehaviour
{
    private float cookingDuration = 0.0f;
    Coroutine cookingRoutine;
    public Steak(float cookingDuration)
    {
        this.cookingDuration = cookingDuration;
    }
    public void StartCooking()
    {
        cookingRoutine = StartCoroutine(CookingRoutine());
    }
    public float StopCooking()
    {
        StopCoroutine(cookingRoutine);
        return cookingDuration;
    }
    public float GetCookingDuration()
    {
        return cookingDuration;
    }

    IEnumerator CookingRoutine()
    {

        while (true)
        {
            // Update the remaining time
            cookingDuration += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }
    }
}
