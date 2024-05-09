using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] private Transform steakPrefab;
    [SerializeField] private Transform carryPoint;
    private Transform steakTransform;

    public void interact(){
        Debug.Log("I want to pick a Steak!");
        steakTransform = Instantiate(steakPrefab, carryPoint);
        steakTransform.localPosition = Vector3.zero;
    }
    public Transform getSteak(){
        return steakTransform;
    }
}
