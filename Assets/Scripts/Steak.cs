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

    public static float maxCookingDuration = 15f; 

    public static event System.Action<Steak> OnSteakCooked;


    
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
        startColor = steakRenderer.material.color;
    }
    public void StartCooking()
    {
        cookingRoutine = StartCoroutine(CookingRoutine());
    }
    public void StopCooking()
    {
        StopCoroutine(cookingRoutine);
        OnSteakCooked?.Invoke(this);
        
    }
    public float GetCookingDuration()
    {
        return cookingDuration;
    }

    IEnumerator CookingRoutine()
    {
        if (steakRenderer.material == null)
    {
        yield break; 
    }
        while (true)
        {
            
            cookingDuration += Time.deltaTime;
            
            steakRenderer.material.color = Color.Lerp(startColor, endColor, cookingDuration/maxCookingDuration);

            yield return null;
        }


    }
    private Renderer findRenderer(){
        GameObject steakRenderer = transform.Find("Steak").gameObject;
        return steakRenderer.GetComponent<Renderer>();
    }
    public Color GetColor(){
        return steakRenderer.material.color;
    }
}
