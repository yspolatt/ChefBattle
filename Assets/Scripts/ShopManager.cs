using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //Here I leave the rough logic to implement later.
    //First, we will need to implement a waitingCustomers queue, where we add customers once they
    //arrive, and remove them once they get their food served. We will also need to implement the
    //waiters' queue, where we add waiters once they become idle, and remove when a task is assigned.
    //Waiters might choose the ready steaks randomly or in a queue, this will be handled in waiters and
    //servicetable classes as I did in fridge, stove or servicetable interactions.
    [SerializeField] private CustomerManager customerManager;
    [SerializeField] private ServiceTable serviceTable;
    private Queue<Waiter> waitersQueue = new Queue<Waiter>();
    [SerializeField] private Waiter[] waiters;
    public void Start()
    {
        foreach (Waiter waiter in waiters)
        {
            waitersQueue.Enqueue(waiter);
        }
    }
    public void Update()
    {
       //Debug.Log("---------------");
       //Debug.Log($"Is there any Idle Waiter? {this.isAnyIdleWaiter()}");
       //Debug.Log($"Is there any customer waiting? {customerManager.isAnyCustomerWaiting()}");
       //Debug.Log($"Is there any plate in the table? {serviceTable.isPlateThere()}");
       //Debug.Log("---------------");
        if (customerManager.isAnyCustomerWaiting()&& serviceTable.isPlateThere()&& this.isAnyIdleWaiter()){
            //Debug.Log("Serving customer");
            Customer customertoserve = customerManager.RemoveFromWaitingQueue();
            Waiter waiter_serve = waitersQueue.Dequeue();
            Transform steak_serve = serviceTable.getNextSteak(); 
            waiter_serve.assignTask(steak_serve,customertoserve);
            //waiter_serve.serveCustomer(customertoserve);
            //, steak_serve7
        }
    }
    public bool isAnyIdleWaiter(){
        
        return waitersQueue.Count > 0;
    }
    public void addWaiter(Waiter waiter){
        waitersQueue.Enqueue(waiter);
    }

}