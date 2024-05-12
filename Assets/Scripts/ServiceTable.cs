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
    private int nextEmptySpawnPoint;
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
        nextEmptySpawnPoint = emptySpawnPoints.Dequeue();
        Transform spawnPoint = plateSpawnPoints[nextEmptySpawnPoint];
        steakTransform.parent = spawnPoint;
        steakTransform.localPosition = Vector3.zero;
        emptyPlaceCount--;
        return spawnPoint;
    }
    public int getEmptyPlaceCount()
    {
        return emptyPlaceCount;
    }
    public Transform getNextSteak()
    {
        return readySteakQueue.Dequeue();
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
