using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Customer : MonoBehaviour
{


    private NavMeshAgent navMeshAgent;
    private Transform target;

    private Seat seat;
    public CustomerStateEnum state;
    private CustomerManager customerManager;
    private ShopManager shopManager;

    
    public static event Action<Customer> OnArrivedExit;

    
    [SerializeField] private float maxWaitOrderTime = 7f;

    [SerializeField] private float eatingTime = 10f;



    private Renderer customerRenderer;
    private Color originalColor;


    private void Awake()
    {
        customerRenderer = findRenderer();
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalColor = customerRenderer.material.color;
        customerManager = CustomerManager.Instance;
        shopManager = ShopManager.Instance;

    }

    private void Start()
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
                    customerManager.AddCustomerToServiceQueue(this);
                    StartCoroutine(WaitingOrderCoroutine());
                    break;
                case CustomerStateEnum.MovingToQueue:
                    transform.LookAt(Vector3.forward);
                    state = CustomerStateEnum.InQueue;
                    break;
                case CustomerStateEnum.MovingToExit:
                    OnArrivedExit?.Invoke(this);
                    break;
                case CustomerStateEnum.Leaving:
                    OnArrivedExit?.Invoke(this);
                    break;
                default:
                    break;
            }

        }

    }


    // // state is the new state that the customer will be in
    public void UpdateCustomerState(CustomerStateEnum state){

        switch(state){
            case CustomerStateEnum.Eating:
                StartEating();
                break;
            case CustomerStateEnum.Leaving:
                MoveToExit();
                shopManager.ReleaseSeat(seat);
                customerManager.HandleCustomerLeavingShop();
                break;
    

        }
    }



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



    private void MoveToAvailableTable()
    {
        if (target != null && navMeshAgent.remainingDistance < 0.1f)
        {
            navMeshAgent.SetDestination(target.position);

        }

        state = CustomerStateEnum.MovingToTable;


    }


    private void MoveToTheQueue()
    {
        Transform queuePoint = customerManager.queuePoint;
        if (customerManager.isQueueFull())
        {
            MoveToExit();
            return;
        }
        navMeshAgent.SetDestination(queuePoint.position);
        customerManager.customerWaitingQueue.Enqueue(this);
        customerManager.queuePoint.position += new Vector3(0, 0, 1.2f);
        
        state = CustomerStateEnum.MovingToQueue;


    }

    public void MoveToExit()
    {
        Transform exitPoint = customerManager.customerDisappearPoint;
        navMeshAgent.SetDestination(exitPoint.position);
        state = CustomerStateEnum.MovingToExit;
    }

    public void MoveToDestination(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    private IEnumerator WaitingOrderCoroutine()
    {
        
        float elapsedTime = 0f;
        Color targetColor = Color.red;
        while (elapsedTime < maxWaitOrderTime)
        {
            elapsedTime += 0.1f;
            customerRenderer.material.color = Color.Lerp(originalColor, targetColor, elapsedTime / maxWaitOrderTime);
            yield return new WaitForSeconds(0.1f);
        }

    }
    private Renderer findRenderer()
    {
        GameObject baseCharacter = transform.Find("BaseCharacter").gameObject;
        GameObject body = baseCharacter.transform.Find("Body").gameObject;
        return body.GetComponent<Renderer>();
    }

    private void OnDestroy()
    {
        if (state == CustomerStateEnum.MovingToQueue)
        {
            //CustomerManager.Instance.customerQueue.Remove(this);  burda niye bu var bilmiyorum
        }
    }

    
    public void StartMove()
    {   
        
        seat = null;
        customerRenderer.material.color = originalColor;
        seat = shopManager.FindAvailableSeats();

        target = seat?.transform;

        if (target != null)
        {
            seat.isAvailable = false;
            MoveToAvailableTable();
        }
        else
        {
            if (!customerManager.isQueueFull())
            {
                MoveToTheQueue();
            }
            else
            {
                MoveToExit();
            }
        }
    }

    public void StartEating(){
        StartCoroutine(EatingCoroutine());

    }

    private IEnumerator EatingCoroutine()
    {
        float elapsedTime = 0f;
        while(elapsedTime < eatingTime){
            elapsedTime += 0.1f;
            customerRenderer.material.color = Color.Lerp(customerRenderer.material.color, originalColor, elapsedTime / eatingTime);
            yield return new WaitForSeconds(0.1f);
        }
        UpdateCustomerState(CustomerStateEnum.Leaving);
    }

}

public enum CustomerStateEnum
{
    MovingToTable,
    InQueue,
    MovingToQueue,
    WaitingOrder,
    MovingToExit, // used for customers who does not enter
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