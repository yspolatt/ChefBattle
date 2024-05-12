using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : MonoBehaviour
{
    private float cookingDuration = 0.0f;
    private Coroutine cookingRoutine;
    private Renderer steakRenderer;
    public Color startColor = new Color(255f/255f, 151f/255f, 166f/255f, 1f);
    public Color endColor = Color.black;
    public Steak(float cookingDuration)
    {
        this.cookingDuration = cookingDuration;
    }
    void Start()
    {
    }
    void Awake()
    {
        // Initialize the material
        steakRenderer = findRenderer();
        Debug.Log(steakRenderer);
        startColor = steakRenderer.material.color;
    }
    public void StartCooking()
    {
        cookingRoutine = StartCoroutine(CookingRoutine());
    }
    public void StopCooking()
    {
        StopCoroutine(cookingRoutine);
    }
    public float GetCookingDuration()
    {
        return cookingDuration;
    }

    IEnumerator CookingRoutine()
    {
        if (steakRenderer.material == null)
    {
        Debug.LogError("Steak material is null.");
        yield break; // Stop the coroutine if the material is not assigned
    }
        while (true)
        {
            // Update the remaining time
            cookingDuration += Time.deltaTime;

            // Update the color based on the proportion of cooking time completed
            steakRenderer.material.color = Color.Lerp(startColor, endColor, cookingDuration/10);

            // Wait until the next frame
            yield return null;
        }

        // Ensure the color is set to fully cooked at the end
        steakRenderer.material.color = endColor;
    }
    private Renderer findRenderer(){
        GameObject steakRenderer = transform.Find("Steak").gameObject;
        return steakRenderer.GetComponent<Renderer>();
    }
    public Color GetColor(){
        return steakRenderer.material.color;
    }
}
