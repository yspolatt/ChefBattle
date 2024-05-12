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

    Transform steak;
    [SerializeField] private ServiceTable serviceTable;
    void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void Update()
    {
        if (HasArrived())
        {
            switch (state)
            {
                case WaiterStateEnum.GoingtoSteak:
                    Debug.Log("Arrived to Steak");
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
                    customer.MoveToExit();
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
        Debug.Log("Task Assigned");
        Debug.Log(steak.position);
        Debug.Log(customerToServe);
        goGetSteak(steak_serve);
    }
    public void goGetSteak(Transform steak_serve)
    {
        Debug.Log("Going to get steak");
        state = WaiterStateEnum.GoingtoSteak;
        Debug.Log(state);
        steak = steak_serve;
        Debug.Log(steak.position);

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
    public enum WaiterStateEnum
    {
        Idle,
        GoingtoSteak,
        ServingCustomer
    }
}
