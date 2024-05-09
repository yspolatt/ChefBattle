using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField] private Transform panPoint;
    [SerializeField] private Transform steakPrefab;

    private Transform steakTransform;
    public bool isEmpty = true;

    public void drop()
    {
        steakTransform = Instantiate(steakPrefab, panPoint);
        steakTransform.localPosition = Vector3.zero;
        isEmpty = false;
    }
    public void pick()
    {
        Debug.Log("I want to pick up from the stove");
        isEmpty = true;
    }
    public bool IsEmpty(){
        return isEmpty;
    }
}
