using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 7f; // поле не public, но с сипользованием SerializeField можно до него добираться в редакторе!
    private bool isWalking;
    private void Update(){

        Vector2 inputVector = new Vector2(0,0);

        if (Input.GetKey(KeyCode.W)){
            inputVector.y = +1f;
            // Debug.Log("Pressing");
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1f;
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1f;
        }
        inputVector = inputVector.normalized; // чтоб объект не двигался в 2 раза быстрее при нажатии двух клавиш для движения по диоганали.
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        isWalking = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
        

    }
    public bool IsWalking() { 
        return isWalking;
    }
}
