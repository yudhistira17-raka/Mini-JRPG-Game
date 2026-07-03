using UnityEngine;


/// Menggerakkan karakter menggunakan Rigidbody 3D, dengan kecepatan yang dapat diatur.

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector3 moveDirection;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = playerInput.MovementInput;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }
}