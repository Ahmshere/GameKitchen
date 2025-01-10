using System;
using UnityEngine;

public class GameInput : MonoBehaviour{

    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        /*
         * Разница между использованием Interact_performed и Interact_performed() заключается в том, что Interact_performed ссылается на метод как объект, а Interact_performed() вызывает этот метод.
         */
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       /* if (OnInteractAction != null) {
            OnInteractAction(this, EventArgs.Empty);
        }  
       The same! */
        OnInteractAction?.Invoke(this, EventArgs.Empty);

        /*
         Знак ? в строке OnInteractAction?.Invoke(this, EventArgs.Empty); — это null-условный оператор (null-conditional operator). Он используется для проверки, существует ли объект перед его использованием, чтобы избежать исключения NullReferenceException.
        Использование ?. в вызове событий — это безопасный способ проверить, есть ли подписчики на событие, и вызов события только в том случае, если подписчики существуют.
         */
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = 
        playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized; // чтоб объект не двигался в 2 раза быстрее при нажатии двух клавиш для движения по диоганали.
        return inputVector;
        
    }
}
/*
 Чтобы добавить управление с разных устройств используй PlayerInputActions (двойное нажатие)
и добавь Действие.
 */
