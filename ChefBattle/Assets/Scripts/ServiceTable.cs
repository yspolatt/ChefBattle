using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceTable : MonoBehaviour
{
    private Queue readySteakQueue = new Queue();
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
    public void drop(float duration)
    {
        readySteakQueue.Enqueue(duration);
        nextEmptySpawnPoint = emptySpawnPoints.Dequeue();
        Transform spawnPoint = plateSpawnPoints[nextEmptySpawnPoint];
        steakpicked = Instantiate(cookedPrefab, spawnPoint);
        steakpicked.localPosition = Vector3.zero;
        emptyPlaceCount--;
    }
    public int getEmptyPlaceCount()
    {
        return emptyPlaceCount;
    }
    public float getNextSteakDuration()
    {
        return (float)readySteakQueue.Dequeue();
    }
}
