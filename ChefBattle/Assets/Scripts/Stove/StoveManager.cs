using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveManager : MonoBehaviour
{
    private Dictionary<int, bool> stovesEmptiness = new Dictionary<int, bool>();
    private Dictionary<int, float> stoveSteakDuration = new Dictionary<int, float>();
    private Dictionary<int, Steak> stoveSteak = new Dictionary<int, Steak>();

    Coroutine cookingRoutine;

    public StoveManager()
    {
        stovesEmptiness.Add(1, true);
        stovesEmptiness.Add(2, true);
        stovesEmptiness.Add(3, true);
        stoveSteakDuration.Add(1, 0);
        stoveSteakDuration.Add(2, 0);
        stoveSteakDuration.Add(3, 0);
        stoveSteak.Add(1, null);
        stoveSteak.Add(2, null);
        stoveSteak.Add(3, null);
    }
    public Steak GetSteak(int num){
        return stoveSteak[num];
    }
    public void SetSteak(int num, Steak steak){
        stoveSteak[num] = steak;
    }
    public bool IsStoveEmpty(int stoveNumber)
    {
        return stovesEmptiness[stoveNumber];
    }
    public float GetStoveSteakDuration(int stoveNumber)
    {
        return stoveSteakDuration[stoveNumber];
    }
    public void SetStoveSteakDuration(int stoveNumber, float duration)
    {
        stoveSteakDuration[stoveNumber] = duration;
    }
    public void SetStoveEmpty(int stoveNumber, bool isEmpty)
    {
        stovesEmptiness[stoveNumber] = isEmpty;
    }
    public int EmptyStoveCount()
    {
        int count = 0;
        foreach (bool isEmpty in stovesEmptiness.Values)
        {
            if (isEmpty)
            {
                count++;
            }
        }
        return count;
    }
    public void StartCooking(int num)
    {   
        GetSteak(num).StartCooking();
    }
    public float StopCooking(int num)
    {   
        float duration = GetSteak(num).StopCooking();
        SetStoveEmpty(num, true);
        SetSteak(num, null);
        SetStoveSteakDuration(num, 0);
        return duration;
    }
}
