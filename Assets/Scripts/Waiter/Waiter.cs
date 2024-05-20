using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waiter : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private ShopManager shopManager;
    private Customer customer;
    public WaiterStateEnum state;
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform plate;
    private Transform plateAtHand;

    [SerializeField] private Transform waiterDefaultPosition;

    private CustomerManager customerManager;



    Transform steak;
    [SerializeField] private ServiceTable serviceTable;
    void Start()
    {
        shopManager = ShopManager.Instance;
        navMeshAgent = GetComponent<NavMeshAgent>();
        customerManager = CustomerManager.Instance;

    }
    void Update()
    {
        if (HasArrived())
        {
            switch (state)
            {
                case WaiterStateEnum.GoingtoSteak:
                    steak.parent = carryPoint;
                    steak.localPosition = new Vector3(0, 0, 0);
                    plateAtHand = Instantiate(plate, carryPoint);
                    plateAtHand.localPosition = Vector3.zero;
                    plateAtHand.localScale = new Vector3(3, 3, 3);
                    serviceTable.incrementEmptyPlaceCount();
                    serveCustomer(customer);
                    break;
                case WaiterStateEnum.ServingCustomer:
                    state = WaiterStateEnum.Idle;
                    steak.parent = customer.transform;
                    steak.localPosition = new Vector3(0, 0, 0);
                    plateAtHand = Instantiate(plate, steak);
                    plateAtHand.localPosition = Vector3.zero;
                    plateAtHand.localScale = new Vector3(3, 3, 3);
                    shopManager.addWaiter(this);
                    customer.UpdateCustomerState(CustomerStateEnum.Eating);
                    MoveDefaultPosition();
                    
                    
                    //MoveDefaultPosition();
                    // customer.MoveToExit();
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
    public void assignTask(Transform steak_serve, Customer customerToServe)
    {
        state = WaiterStateEnum.GoingtoSteak;
        customer = customerToServe;
        steak = steak_serve;
        goGetSteak(steak_serve);
    }
    public void goGetSteak(Transform steak_serve)
    {
        
        state = WaiterStateEnum.GoingtoSteak;
        steak = steak_serve;
        Vector3 target = new Vector3(steak.position.x - 0.5f, steak.position.y, steak.position.z);
        navMeshAgent.SetDestination(target);

    }
    public void serveCustomer(Customer customer)
    {
        Debug.Log($"Going to serve customer {customer}");
        state = WaiterStateEnum.ServingCustomer;
        if (customer != null && navMeshAgent.remainingDistance < 0.1f)
        {
            Vector3 target = new Vector3(customer.transform.position.x, customer.transform.position.y, customer.transform.position.z);
            navMeshAgent.SetDestination(target);
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

    private void MoveDefaultPosition()
    {
        navMeshAgent.SetDestination(waiterDefaultPosition.position);
    }
    public enum WaiterStateEnum
    {
        Idle,
        GoingtoSteak,
        ServingCustomer
    }
}
