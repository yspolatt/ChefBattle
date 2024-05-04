using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    private float moveSpeed_dyn;
    private bool isWalking;
    private bool isDancing = false;

    [SerializeField] private float runSpeed = 3.0f;
    private void Update()
    {
        moveSpeed_dyn = moveSpeed; 
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
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
            inputVector.x = +1;
        }
        if (Input.GetKey(KeyCode.LeftShift)){
            moveSpeed_dyn = runSpeed;
        }
        if (Input.GetKey(KeyCode.X))
        {
            isDancing= true;
        }else{
            isDancing=false;
        }
        inputVector = inputVector.normalized;
        Vector3 movedir = new Vector3(inputVector.x, 0f, inputVector.y);    
        float playersize = 0.7f;
        bool canmove = !Physics.Raycast(transform.position, movedir, playersize);
        if (canmove)
        {
            transform.position += movedir * Time.deltaTime * moveSpeed_dyn;

        }
        isWalking = movedir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    public bool IsDancing()
    {
        return isDancing;
    }
}
