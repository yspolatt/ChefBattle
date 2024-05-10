using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Customer> customers = new List<Customer>();
    // public List<Customer> waitingCustomers = new List<Customer>();
    // public List<Customer> seatedCustomers = new List<Customer>();

    [SerializeField] private float minSpawnInterval = 10f;
    [SerializeField] private float maxSpawnInterval = 30f;

    [SerializeField] private Transform[] customerSpawnPoints;

    public GameObject[] customerPrefabs;

    private void Start(){
        StartCoroutine(SpawnCustomers());
    }
    

    private IEnumerator SpawnCustomers(){
        while(true){
            SpawnCustomer();
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
        
    }

    private void SpawnCustomer(){
        GameObject customerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        Transform spawnPoint = customerSpawnPoints[Random.Range(0, customerSpawnPoints.Length)];
        Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
    }




    
}
