using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Customer : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private bool hasReachedDestination = false; // Flag to check if the destination is reached


    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindAvailableTargets();
        Debug.Log(target);
        
    }

    void  Update(){
        MoveToAvailableTable();
        CheckIfReachedTarget();
    }

    private Transform FindAvailableTargets(){

        GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");

        List<Seat> availableSeats = new List<Seat>();

        foreach(GameObject table in tables){
            Table tableScript = table.GetComponent<Table>();
            foreach(Seat seat in tableScript.seats){
                if(seat.isAvailable){
                    Debug.Log("Seat is available");
                    availableSeats.Add(seat);
                }
            }
        }
       
        if(availableSeats.Count > 0){
            int randomIndex = Random.Range(0, availableSeats.Count);
            availableSeats[randomIndex].isAvailable = false;
            Debug.Log("Available position: " + availableSeats[randomIndex]);
            return availableSeats[randomIndex].position;

        }
        return null;

    }

    private void MoveToAvailableTable(){
        if (target != null && navMeshAgent.remainingDistance < 0.1f ){
            navMeshAgent.SetDestination(target.position);
        }

    }
     private void CheckIfReachedTarget()
    {
        if (target != null && !hasReachedDestination)
        {
            // Check if the customer has reached the target by measuring the distance to the target
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // I should add the isEnqueued bool to do this only once.
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    // Arrived
                    hasReachedDestination = true;
                    
                }
            }
        }
    }


    

}
