using UnityEngine;

public class PlayerAminator : MonoBehaviour{
    [SerializeField] private Player player;

    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
       
    }
    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

}
