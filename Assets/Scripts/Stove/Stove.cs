using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField] private Transform panPoint;
    [SerializeField] private Transform steakPrefab;
    private Steak steak;

    private Transform steakdropped;
    private Transform steakpicked;
    private bool isEmpty = true;
    private float cookingDuration;

    public void drop(int num, StoveManager stoveManager)
    {
        steakdropped = Instantiate(steakPrefab, panPoint);
        steakdropped.localPosition = Vector3.zero;
        Steak steakObject = steakdropped.GetComponent<Steak>();
        stoveManager.SetSteak(num, steakObject);
        isEmpty = false;
        stoveManager.StartCooking(num);
    }
    public Transform pick(int num, StoveManager stoveManager)
    {
        steak = stoveManager.GetSteak(num);
        steak.StopCooking();
        steakpicked = steak.transform;
        isEmpty = true;
        return steakpicked;
    }
    public float GetCookingDuration()
    {
        return this.cookingDuration;
    }
    public bool IsEmpty()
    {
        return isEmpty;
    }
    public void IsNotEmpty()
    {
        isEmpty= false;
    }
}
