using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private bool wantspick = false;
    private bool wantsdrop = false;
    public bool Wantspick() { 
        return wantspick;
     }
    public void disableWantspick() {
         wantspick = false; 
         }
    public bool Wantsdrop() {
        return wantsdrop;
     }
    public void disableWantsdrop() {
         wantsdrop = false;
         }
    public Vector2 GetMovementVectorNormalized()
    {

        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            wantspick = true;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            wantsdrop = true;
        }
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
