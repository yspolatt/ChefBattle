using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    public List<Seat> seats = new List<Seat>();

    public Transform[] seatTransforms = new Transform[2];


    private void Awake(){
        for(int i = 0; i < seatTransforms.Length; i++){
            seats.Add(new Seat(seatTransforms[i], i));
        }
    }
}

public class Seat{
    public bool isAvailable;
    public Transform transform;

    public int seatNumber;
    public Seat(Transform transform, int seatNumber){
        this.transform = transform;
        this.isAvailable = true;
        this.seatNumber = seatNumber;
    }
}
