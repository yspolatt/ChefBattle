using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CustomerManager : Singleton<CustomerManager>
{
    public List<Customer> customers = new List<Customer>();
    public Queue<Customer> customerWaitingQueue = new Queue<Customer>();
    public Queue<Customer> seatedCustomers = new Queue<Customer>();

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



    // check queue, if there is a customer send to the shop
    // customer already deleted from seated customers
    public void HandleCustomerLeavingShop(){
        int numberOfCustomers = customerWaitingQueue.Count;
        Debug.Log("Number of customers in queue: " + numberOfCustomers);

        if(isAnyCustomerWaiting()){
            Customer customerToServe = customerWaitingQueue.Dequeue();
            customerToServe.StartMove();
        }
        MoveQueueOneStep(numberOfCustomers);

       
        
    }

    private void MoveQueueOneStep(int numberOfCustomers){
        queuePoint.position -= new Vector3(0, 0, numberOfCustomers* 1.2f);

        foreach(Customer customer in customerWaitingQueue){
            customer.MoveToDestination(queuePoint.position);
            queuePoint.position += new Vector3(0, 0, 1.2f);
        }
        

    }


   public bool isQueueFull(){
       return customerWaitingQueue.Count >= queueCapacity;
   }
   public void AddCustomerToServiceQueue(Customer customer){
       seatedCustomers.Enqueue(customer);
       
   }
   public Customer GetCustomerFromServiceQueue(){
       return seatedCustomers.Peek();
   }

   public Customer RemoveCustomerFromServiceQueue(){
       return seatedCustomers.Dequeue();
   }      
   public bool isAnyCustomerSeated(){
       return seatedCustomers.Count > 0;
   }

    public bool isAnyCustomerWaiting(){
         return customerWaitingQueue.Count > 0;
    }

    public void AddCustomerToQueue(Customer customer){
        customerWaitingQueue.Enqueue(customer);
    }
    public Customer RemoveCustomerFromQueue(){
        return customerWaitingQueue.Dequeue();
    }
    

    
}



