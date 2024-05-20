using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Customer yemegi aldiktan sonra biraz bekleyip cikacak + 
// customer cikinca queue'dan adam gelicek eger varsa + 
// custemer normal rengine donucek zamanla +
// tabak customer in onune konucak, kafasina degil -
// oyuncu tabagi service table a biraktiktan sonra eline tabak kaliyor -
// garson tabagi elinde kaliyor -
// sadece 4 tane steak koyabiliyoruz servis masasina -

public class ShopManager : Singleton<ShopManager>
{
    //Here I leave the rough logic to implement later.
    //First, we will need to implement a waitingCustomers queue, where we add customers once they
    //arrive, and remove them once they get their food served. We will also need to implement the
    //waiters' queue, where we add waiters once they become idle, and remove when a task is assigned.
    //Waiters might choose the ready steaks randomly or in a queue, this will be handled in waiters and
    //servicetable classes as I did in fridge, stove or servicetable interactions.
    private CustomerManager customerManager;
    [SerializeField] private ServiceTable serviceTable;
    private Queue<Waiter> waitersQueue = new Queue<Waiter>();

    public GameObject[] tables;

    private List<Seat> seats;

    private List<int> availableSeats;
    
    [SerializeField] private Waiter[] waiters;

    protected override void Awake()
    {
        StartTables();
        base.Awake();
    }
    private void Start()
    {   
        
        customerManager = CustomerManager.Instance;
        foreach (Waiter waiter in waiters)
        {
            waitersQueue.Enqueue(waiter);
        }


        
    }
    private void Update()
    {
       //Debug.Log("---------------");
       //Debug.Log($"Is there any Idle Waiter? {this.isAnyIdleWaiter()}");
       //Debug.Log($"Is there any customer waiting? {customerManager.isAnyCustomerWaiting()}");
       //Debug.Log($"Is there any plate in the table? {serviceTable.isPlateThere()}");
       //Debug.Log("---------------");
        if (customerManager.isAnyCustomerSeated()&& serviceTable.isPlateThere()&& this.isAnyIdleWaiter()){
            //Debug.Log("Serving customer");
            Customer customertoserve = customerManager.RemoveCustomerFromServiceQueue();
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


    public Seat FindAvailableSeats()
    {
        if (checkIfAllSeatsAreOccupied())
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, availableSeats.Count);
        int seatIndex = availableSeats[randomIndex]; // error
        availableSeats.RemoveAt(randomIndex);

        return seats[seatIndex];

    }

    private bool checkIfAllSeatsAreOccupied()
    {   
        
        foreach (Seat seat in seats)
        {   
            if (seat.isAvailable)
            {
                return false;
            }
        }
        return true;
    }

    public void StartTables()
    {
        tables = GameObject.FindGameObjectsWithTag("Table");
        seats = new List<Seat>();
        availableSeats = new List<int>();

        int i = 0;
        foreach (GameObject table in tables)
        {   

            Table newTable = table.GetComponent<Table>();
            newTable.seats[0].seatNumber = 2 * i;
            newTable.seats[1].seatNumber = 2* i + 1;
            seats.Add(newTable.seats[0]);
            seats.Add(newTable.seats[1]);
            availableSeats.Add(newTable.seats[0].seatNumber);
            availableSeats.Add(newTable.seats[1].seatNumber);
        
            i += 1;
        }
        
    }

    public void ReleaseSeat(Seat seat)
    {
        seat.isAvailable = true;
        availableSeats.Add(seat.seatNumber);
    }

}