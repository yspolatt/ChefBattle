using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactDistance = 2f;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
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
                if (gameInput.Wantspick())
                {
                    stove.pick();
                    gameInput.disableWantspick();
                }
                else if (gameInput.Wantsdrop())
                    stove.drop();
                gameInput.disableWantsdrop();
            }
            else if (raycasthit.transform.TryGetComponent(out Fridge fridge) && gameInput.Wantspick())
            {
                fridge.interact();
                gameInput.disableWantspick();

            }
            else if (raycasthit.transform.TryGetComponent(out ServiceTable servicetable) && gameInput.Wantsdrop())
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
