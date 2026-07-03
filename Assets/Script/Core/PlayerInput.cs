using UnityEngine;

/// Membaca input pemain (WASD) dan menyediakan vektor gerakan.
/// Dapat dikunci dari luar untuk cutscene.
public class PlayerInput : MonoBehaviour
{
    public Vector3 MovementInput { get; private set; }
    private bool isInputLocked = false;

    void Update()
    {
        if (isInputLocked)
        {
            MovementInput = Vector3.zero;
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        MovementInput = new Vector3(horizontal, 0, vertical).normalized;
    }

    public void LockInput() { isInputLocked = true; }
    public void UnlockInput() { isInputLocked = false; }
}