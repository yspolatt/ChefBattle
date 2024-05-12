using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CustomerManager : Singleton<CustomerManager>
{
    public List<Customer> customers = new List<Customer>();
    public List<Customer> customerQueue = new List<Customer>();
    private Queue<Customer> seatedCustomers = new Queue<Customer>();

    public int queueCapacity = 5;

    [SerializeField] private float minSpawnInterval = 2900f;
    [SerializeField] private float maxSpawnInterval = 3000f;

    [SerializeField] private Transform[] customerSpawnPoints;

    public Transform queuePoint;

    public Transform customerDisappearPoint;

    public List<GameObject> customerPrefabs;



    private ObjectPool<Customer> customerPool;



    protected override void Awake(){
        customerPool = new ObjectPool<Customer>(createFunction, actionOnGet, actionOnRelease, actionOnDestroy, true,  defaultCapacity: 10);
        Customer.OnArrivedExit += OnReturnedToPool;
        base.Awake();
    }

    private void Start(){
        StartCoroutine(SpawnCustomers());
    }


    private Customer createFunction(){
        return Instantiate(customerPrefabs[UnityEngine.Random.Range(0, customerPrefabs.Count)]).GetComponent<Customer>();
        
    }

    private void actionOnGet(Customer customer){
        int randomIndex = UnityEngine.Random.Range(0, customerSpawnPoints.Length);
        customer.gameObject.transform.position = customerSpawnPoints[randomIndex].position;
        customer.gameObject.SetActive(true);
    }

    private void actionOnRelease(Customer customer){
        customer.gameObject.SetActive(false);
    }
    private void actionOnDestroy(Customer customer){
        Destroy(customer.gameObject);
    }
   

    // implement spawning algorithm here
    private IEnumerator SpawnCustomers(){
        while(true){
            SpawnCustomer();
            float interval = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
        
    }

    
    private void SpawnCustomer(){
        Customer customer = customerPool.Get();
        customers.Add(customer);
    }



    //TODO: implement customer destroying logic
    private void OnReturnedToPool(Customer customer){
        customers.Remove(customer);

        // these two methods are called to be safe
        customerPool.Release(customer);
    }


   public bool isQueueFull(){
       return customerQueue.Count >= queueCapacity;
   }
   public void AddToWaitingQueue(Customer customer){
       seatedCustomers.Enqueue(customer);
        //Debug.Log($"{customer} is Added to queue");
   }
   public Customer RemoveFromWaitingQueue(){
       return seatedCustomers.Dequeue();
   }
   public bool isAnyCustomerWaiting(){
       return seatedCustomers.Count > 0;
   }


    
}
