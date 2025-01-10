using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
  /*  private static Player instance;
    public static Player Instance {
        get { return instance; }
        set { instance = value; }
    }*/
    // Or
    public static Player Instance { get; private set; }
    // Or
    /* public static Player instanceField;
     public static Player GetInstanceField() {
         return instanceField;
     }
     public static void SetInstanceField(Player instanceField) {
         Player.instanceField = instanceField;
     }*/

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f; // Скорость движения
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null) { Debug.LogError("There is more than one Player instance!"); }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null) {
        selectedCounter.Interact();
        }
        
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;

    }
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero) {
            lastInteractDirection = moveDirection;
        }

        float interractDistance = 2f;
        if (Physics.Raycast(transform.position, moveDirection, out RaycastHit raycastHit, interractDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // has clearCounter
                // clearCounter.Interact();
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                    
                }
            }
            else { 
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
       // Debug.Log(selectedCounter);
               
        /*
         transform.position

    Это начальная точка, откуда начинается луч.
    В данном случае луч начинается из положения объекта, к которому прикреплен этот скрипт (позиция объекта в мировых координатах).

moveDirection

    Направление, в котором пускается луч.
    В данном случае это направление движения игрока, вычисляемое на основе ввода (inputVector).

out RaycastHit raycastHit

    Переменная raycastHit передается по ссылке и заполняется информацией о первом объекте, с которым столкнулся луч.
    Это объект типа RaycastHit, который содержит данные о столкновении, например:
        Позицию точки столкновения (raycastHit.point).
        Нормаль поверхности столкновения (raycastHit.normal).
        Ссылку на объект, с которым был контакт (raycastHit.collider).

interractDistance

    Максимальная длина луча.
    В данном случае луч проверяет объекты в пределах 2 единиц расстояния от позиции объекта.
         */
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        // Проверка основного направления
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            // Попытка движения только по X
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirX;
            }
            else
            {
                // Попытка движения только по Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDirection = moveDirZ;
                }
                else
                {
                    // Если нельзя двигаться вообще, остановить объект
                    moveDirection = Vector3.zero;
                }
            }
        }

        // Если движение разрешено, обновляем позицию
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveDistance;
        }

        isWalking = moveDirection != Vector3.zero;

        // Обновление направления взгляда игрока
        if (isWalking)
        {
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }
    }
    private void SetSelectedCounter(ClearCounter selectedCounter) {
    this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

}

/*
 Raycast:

    Physics.Raycast проверяет, есть ли препятствие по направлению движения.
    Оно "выстреливает" луч (Ray) из позиции игрока (transform.position) в направлении moveDirection.
    Длина луча ограничивается значением playerSize (в данном случае, 0.7f).

Логика проверки:

    !Physics.Raycast(...) означает "отсутствие столкновения".
    Если луч не находит препятствий в пределах указанного расстояния (playerSize), то canMove будет true.
    Если же есть столкновение, то canMove будет false.

Условие движения:

    Если canMove равно true, игроку разрешается двигаться: transform.position += moveDirection * moveSpeed * Time.deltaTime;.
 */