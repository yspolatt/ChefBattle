using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceTable : MonoBehaviour
{
    private ShopManager shopmanager;
    private Queue<Transform> readySteakQueue = new Queue<Transform>();
    [SerializeField] private Transform cookedPrefab;
    [SerializeField] private Transform[] plateSpawnPoints;
    private Queue<int> emptySpawnPoints = new Queue<int>();
    private int emptyPlaceCount = 4;

    public ServiceTable()
    {
        emptySpawnPoints.Enqueue(3);
        emptySpawnPoints.Enqueue(2);
        emptySpawnPoints.Enqueue(1);
        emptySpawnPoints.Enqueue(0);
    }

    private Transform steakpicked;
    public Transform drop(Transform steakTransform)
    {
        readySteakQueue.Enqueue(steakTransform);
        int nextEmptySpawnPoint = getNextEmptySpawnPoint();
        Transform spawnPoint = plateSpawnPoints[nextEmptySpawnPoint];
        nextEmptySpawnPoint--;
        steakTransform.parent = spawnPoint;
        steakTransform.localPosition = Vector3.zero;
        emptyPlaceCount--;
        return spawnPoint;
    }
    public int getEmptyPlaceCount()
    {
        return emptyPlaceCount;
    }
    public int getNextEmptySpawnPoint()
    {
        int nextEmpty = emptySpawnPoints.Dequeue();
        emptySpawnPoints.Enqueue(nextEmpty);
        return nextEmpty;
    }
    public Transform getNextSteak()
    {   
        Transform steak = readySteakQueue.Dequeue();
        
        return steak;
    }
    public void incrementEmptyPlaceCount()
    {
        emptyPlaceCount++;
    }
    public bool isPlateThere()
    {
        return emptyPlaceCount < 4;
    }
}