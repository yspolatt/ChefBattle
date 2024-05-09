using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactDistance = 2f;
    private bool isWalking;
    private bool isHolding = false;
    private Vector3 lastInteractDir;
    private Transform steakathand;
    private void Update()
    {
        gameInput.disableWantsdrop();
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    public Transform getSteak()
    {
        return steakathand;
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycasthit, interactDistance))
        {
            if (raycasthit.transform.TryGetComponent(out Stove stove))
            {
                //Stove
                if (gameInput.Wantspick() && !isHolding)
                {
                    stove.pick();
                    gameInput.disableWantspick();
                }
                else if (gameInput.Wantsdrop() && isHolding && stove.IsEmpty())
                {
                    Destroy(steakathand.gameObject);
                    stove.drop();
                    isHolding = false;
                    gameInput.disableWantsdrop();

                }
            }
            else if (raycasthit.transform.TryGetComponent(out Fridge fridge) && gameInput.Wantspick() && !isHolding)
            {
                fridge.interact();
                steakathand = fridge.getSteak();
                Debug.Log(steakathand);
                isHolding = true;
                gameInput.disableWantspick();
            }
            else if (raycasthit.transform.TryGetComponent(out ServiceTable servicetable) && gameInput.Wantsdrop() && isHolding)
            {
                servicetable.interact();
                gameInput.disableWantsdrop();

            }
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerSize = .7f;
        bool canMove = !Physics.Raycast(transform.position, moveDirection, playerSize);

        if (canMove)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }

        isWalking = moveDirection != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

    }
}
