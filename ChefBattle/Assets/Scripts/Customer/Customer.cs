using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Customer : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;


    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindAvailableTargets();
        Debug.Log(target);
        
    }

    void  Update(){
        MoveToAvailableTable();
        
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


    

}
