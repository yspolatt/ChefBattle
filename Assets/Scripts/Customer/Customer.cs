using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Customer : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    public CustomerStateEnum state;
    private CustomerManager customerManager;
    public static event Action<Customer> OnArrivedExit;
    public static event Action<Customer> OnSeat;
    public static event Action<CustomerState> OnCustomerStateChanged; // not used for now

    [SerializeField] private float waitTime = 15f;

    private Renderer customerRenderer;
    private Color originalColor;


    private void Awake()
    {
        customerRenderer = findRenderer();
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalColor = customerRenderer.material.color;
    }

    private void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
    }
    private void OnEnable()
    {
        StartMove();
    }



    void Update()
    {

        if (HasArrived())
        {
            switch (state)
            {
                case CustomerStateEnum.MovingToTable:
                    state = CustomerStateEnum.WaitingOrder;
                    customerManager.AddToWaitingQueue(this);
                    StartCoroutine(WaitingOrderCoroutine());
                    break;
                case CustomerStateEnum.MovingToQueue:
                    state = CustomerStateEnum.InQueue;
                    break;
                case CustomerStateEnum.MovingToExit:
                    OnArrivedExit?.Invoke(this);
                    break;
                //     state = CustomerState.WaitingOrder;
                //     break;
                // case CustomerState.WaitingOrder:
                //     state = CustomerState.Eating;
                //     break;
                // case CustomerState.Eating:
                //     state = CustomerState.Leaving;
                //     break;
                //case CustomerStateEnum.Leaving:

                //break;
                default:
                    break;
            }

        }

    }

    //NOT USED FOR NOW
    // state is the new state that the customer will be in
    // private void UpdateCustomerState(CustomerStateEnum state, Customer customer){
    //     CustomerState customerState = new CustomerState(state, customer);
    //     switch(state){
    //         case CustomerStateEnum.InQueue:
    //             //if queue is full, customer leaves
    //             //if queue is not full, customer waits
    //             //if customer is at the front of the queue and one customer leaves, customer moves to the table
    //             //if customer is not at the front of the queue and one customer leaves, customer moves one step forward
    //             break;
    //         case CustomerStateEnum.WaitingOrder:
    //             //customer waits for steak to arrive
    //             //if customer waits for too long, customer leaves
    //             //if steak arrives, customer starts eating

    //             break;
    //         case CustomerStateEnum.Eating:
    //             //customer eats steak, after eating, customer leaves, 

    //             break;
    //         case CustomerStateEnum.Leaving:
    //             //customer pays for the steak, leaves the restaurant
    //             //if queue is not empty, customer at the front of the queue moves to the table

    //             break;
    //     }
    //     OnCustomerStateChanged?.Invoke(customerState);

    // }

    private bool HasArrived()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {

                    return true;
                }
            }
        }
        return false;
    }

    private Transform FindAvailableTargets()
    {

        GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");

        List<Seat> availableSeats = new List<Seat>();

        foreach (GameObject table in tables)
        {
            Table tableScript = table.GetComponent<Table>();
            foreach (Seat seat in tableScript.seats)
            {
                if (seat.isAvailable)
                {
                    availableSeats.Add(seat);
                }
            }
        }

        if (availableSeats.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableSeats.Count);
            availableSeats[randomIndex].isAvailable = false;
            return availableSeats[randomIndex].position;

        }
        return null;

    }

    private void MoveToAvailableTable()
    {
        //Debug.Log(navMeshAgent);
        if (target != null && navMeshAgent.remainingDistance < 0.1f)
        {
            navMeshAgent.SetDestination(target.position);

        }
        state = CustomerStateEnum.MovingToTable;

    }


    private void MoveToTheQueue()
    {
        Transform queuePoint = CustomerManager.Instance.queuePoint;
        if (CustomerManager.Instance.customerQueue.Count >= CustomerManager.Instance.queueCapacity)
        {
            MoveToExit();
            return;
        }
        navMeshAgent.SetDestination(queuePoint.position);
        CustomerManager.Instance.customerQueue.Add(this);
        CustomerManager.Instance.queuePoint.position += new Vector3(0, 0, 1);
        transform.LookAt(Vector3.forward);
        state = CustomerStateEnum.MovingToQueue;


    }

    public void MoveToExit()
    {
        Transform exitPoint = CustomerManager.Instance.customerDisappearPoint;
        navMeshAgent.SetDestination(exitPoint.position);
        state = CustomerStateEnum.MovingToExit;
    }

    private IEnumerator WaitingOrderCoroutine()
    {
        float duration = 3f;
        float elapsedTime = 0f;
        Color targetColor = Color.red;

        while (elapsedTime < duration)
        {
            elapsedTime += 0.1f;
            customerRenderer.material.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
            yield return new WaitForSeconds(0.1f);
        }

    }
    private Renderer findRenderer()
    {
        GameObject baseCharacter = transform.Find("BaseCharacter").gameObject;
        GameObject body = baseCharacter.transform.Find("Body").gameObject;
        return body.GetComponent<Renderer>();
        //Debug.Log(body.GetComponent<Renderer>());
    }

    private void OnDestroy()
    {
        if (state == CustomerStateEnum.MovingToQueue)
        {
            CustomerManager.Instance.customerQueue.Remove(this);
        }
    }
    public void StartMove()
    {
        //renderer.material.color = originalColor;
        target = FindAvailableTargets();
        if (target != null)
        {
            MoveToAvailableTable();
        }
        else
        {
            if (!CustomerManager.Instance.isQueueFull())
            {
                MoveToTheQueue();
            }
            else
            {
                MoveToExit();
            }
        }
    }






}

public enum CustomerStateEnum
{
    MovingToTable,
    InQueue,
    MovingToQueue,
    WaitingOrder,
    MovingToExit,
    Eating,
    Leaving
}

public class CustomerState
{
    public CustomerStateEnum state;
    public Customer customer;
    public CustomerState(CustomerStateEnum state, Customer customer)
    {
        this.state = state;
        this.customer = customer;
    }

}
