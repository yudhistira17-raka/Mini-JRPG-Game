using UnityEngine;


/// Mengatur animasi berjalan dan idle karakter di dunia luar.

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;
    private Rigidbody rb;

    private enum FacingDirection { Down = 0, Left = 1, Right = 2, Up = 3 }
    private int lastDirection = 0;
    private bool isInCutscene = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        lastDirection = Mathf.RoundToInt(animator.GetFloat("Direction"));
    }

    void Update()
    {
        if (playerInput == null) return;

        if (!isInCutscene)
        {
            Vector3 input = playerInput.MovementInput;
            bool isMoving = input.magnitude > 0.1f;
            animator.SetBool("IsMoving", isMoving);

            if (input.magnitude > 0.1f)
            {
                int directionValue = 0;
                if (Mathf.Abs(input.z) > Mathf.Abs(input.x))
                    directionValue = input.z > 0 ? (int)FacingDirection.Up : (int)FacingDirection.Down;
                else if (Mathf.Abs(input.x) > 0)
                    directionValue = input.x > 0 ? (int)FacingDirection.Right : (int)FacingDirection.Left;
                lastDirection = directionValue;
            }
            animator.SetFloat("Direction", lastDirection);
        }
    }

    public void SetCutsceneMode(bool inCutscene)
    {
        isInCutscene = inCutscene;
        if (inCutscene) animator.SetBool("IsMoving", false);
    }

    public void SetMoving(bool moving) { animator.SetBool("IsMoving", moving); }
    public void SetDirection(int direction)
    {
        lastDirection = direction;
        animator.SetFloat("Direction", direction);
    }
}