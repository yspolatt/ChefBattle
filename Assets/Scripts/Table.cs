using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    private int numOfSeats = 2;

    public List<Seat> seats = new List<Seat>();

    public Transform[] seatPositions = new Transform[2];

   private void Start()
   {    
        for (int i = 0; i< numOfSeats; i++){
            seats.Add(new Seat(seatPositions[i]));
            Debug.Log("Seat added" + seats[i].position);
        }
       
   }

   


}

public class Seat{
    public bool isAvailable;
    public Transform position;
    public Seat(Transform position){
        this.position = position;
        this.isAvailable = true;
    }
}
