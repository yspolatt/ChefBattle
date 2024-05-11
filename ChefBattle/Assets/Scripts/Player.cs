using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameInput gameInput;
    private float interactDistance = 2f;
    private bool isWalking;
    private bool isHoldingRaw = false;
    private bool isHoldingCooked = false;
    private bool isHolding = false;
    private Vector3 lastInteractDir;
    private float cookingDuration;
    private Transform steakathand;
    private StoveManager stoveManager = new StoveManager();
    private void Update()
    {
        gameInput.disableWantsdrop();
        gameInput.disableWantspick();
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
                string inputStove = stove.name;
                char lastChar = inputStove[inputStove.Length - 1];
                int num = int.Parse(lastChar.ToString());
                //Stove
                if (gameInput.Wantspick() && !isHolding && !stoveManager.IsStoveEmpty(num))
                {

                    steakathand = stove.pick(num, stoveManager);
                    cookingDuration = stove.GetCookingDuration();
                    Debug.Log($"Now I am holding a steak that cooked {cookingDuration} seconds");
                    stoveManager.SetStoveEmpty(num, true);
                    gameInput.disableWantspick();
                    isHoldingCooked = true;
                    isHolding = true;

                }
                else if (gameInput.Wantsdrop() && isHoldingRaw && stoveManager.IsStoveEmpty(num))
                {
                    Destroy(steakathand.gameObject);
                    stove.drop(num, stoveManager);
                    stoveManager.SetStoveEmpty(num, false);
                    isHoldingRaw = false;
                    isHolding = false;
                    gameInput.disableWantsdrop();
                }
            }
            else if (raycasthit.transform.TryGetComponent(out Fridge fridge) && gameInput.Wantspick() && !isHolding && stoveManager.EmptyStoveCount() > 0)
            {
                steakathand = fridge.getSteak();
                Debug.Log(steakathand);
                isHolding = true;
                isHoldingRaw = true;
                gameInput.disableWantspick();
            }
            else if (raycasthit.transform.TryGetComponent(out ServiceTable servicetable) && gameInput.Wantsdrop() && isHoldingCooked && servicetable.getEmptyPlaceCount()>0 )
            {
                servicetable.drop(cookingDuration);
                Destroy(steakathand.gameObject);
                gameInput.disableWantsdrop();
                isHolding = false;
                isHoldingCooked = false;

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
