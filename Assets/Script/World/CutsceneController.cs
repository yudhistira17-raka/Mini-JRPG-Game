using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    [Header("References")]
    private PlayerInput playerInput;
    private CharacterMovement playerMovement;
    private CharacterAnimator playerAnimator;

    [Header("Cutscene Settings")]
    public float cutsceneWalkSpeed = 2f;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<CharacterMovement>();
        playerAnimator = GetComponent<CharacterAnimator>();
    }

    // --- Fungsi cutscene biasa (dialog tunggal) ---
    public void StartCutscene(Vector3 targetDestination, System.Action onComplete = null)
    {
        StartCoroutine(RunCutscene(targetDestination, onComplete));
    }

    IEnumerator RunCutscene(Vector3 target, System.Action onComplete)
    {
        playerInput.LockInput();
        if (playerAnimator != null) playerAnimator.SetCutsceneMode(true);

        yield return StartCoroutine(MoveToTargetWithAnim(target));

        if (playerAnimator != null) playerAnimator.SetCutsceneMode(false);
        playerInput.UnlockInput();
        onComplete?.Invoke();
    }

    // --- Fungsi duel (Robert -> Yes) ---
    public void StartDuelCutscene(Vector3 targetPoint1, NPCInteractable robertNPC)
    {
        // --- PERBAIKAN: Cari Enemy berdasarkan nama GameObject, bukan FindObjectOfType ---
        GameObject enemyObj = GameObject.Find("Enemy");
        NPCInteractable enemy = null;
        if (enemyObj != null)
        {
            enemy = enemyObj.GetComponentInChildren<NPCInteractable>();
        }
        // -----------------------------------------------------------------------------

        if (enemy == null)
        {
            Debug.LogError("Tidak ada objek bernama 'Enemy' yang ditemukan di scene! Pastikan musuh di arena memiliki nama GameObject 'Enemy'.");
            return;
        }
        StartCoroutine(RunDuelCutscene(targetPoint1, enemy));
    }

    IEnumerator RunDuelCutscene(Vector3 target1, NPCInteractable enemy)
    {
        playerInput.LockInput();
        if (playerAnimator != null) playerAnimator.SetCutsceneMode(true);

        // --- BUKA PINTU ARENA (Gate) saat duel dimulai ---
        GameObject gate = GameObject.Find("Arena_Gate");
        if (gate != null)
        {
            gate.SetActive(false); // Matikan Collider agar player bisa masuk
            Debug.Log("Pintu Arena terbuka!");
        }
        else
        {
            Debug.LogWarning("Objek 'Arena_Gate' tidak ditemukan. Pastikan namanya persis!");
        }
        // ----------------------------------------------------

        // 1. Player berjalan ke Target 1 (depan Enemy)
        yield return StartCoroutine(MoveToTargetWithAnim(target1));
        if (playerAnimator != null) playerAnimator.SetMoving(false);

        // 2. Minta Enemy berbicara (menggunakan Sequence)
        SimpleDialogueManager enemyDm = enemy.dialogueManager;
        if (enemyDm != null && enemy.duelDialogueSequence != null && enemy.duelDialogueSequence.lines.Count > 0)
        {
            bool enemyDone = false;
            enemyDm.OnClosed = () =>
            {
                enemyDone = true;
                Debug.Log("Percakapan duel selesai!");
            };

            enemyDm.ShowDialogue(enemy.duelDialogueSequence);
            yield return new WaitUntil(() => enemyDone == true);
        }
        else
        {
            Debug.LogWarning("Enemy tidak memiliki Sequence dialog atau DialogueManager belum diseret!");
        }

        // 3. Player berjalan ke Target 2 (tepat di depan Enemy, bersentuhan)
        Vector3 target2 = enemy.transform.position + (transform.position - enemy.transform.position).normalized * 1.5f;
        yield return StartCoroutine(MoveToTargetWithAnim(target2));

        // 4. Trigger Battle Scene
        // Cari trigger di dalam Enemy (misal anak bernama "Battle_Trigger" atau langsung di Enemy)
        SceneTransitionTrigger trigger = enemy.GetComponentInChildren<SceneTransitionTrigger>();
        if (trigger != null)
        {
            trigger.TriggerBattle();
        }
        else
        {
            Debug.LogError("Enemy belum punya SceneTransitionTrigger!");
        }

        if (playerAnimator != null) playerAnimator.SetCutsceneMode(false);
        playerInput.UnlockInput();
    }

    // --- Fungsi bantu: bergerak dengan animasi ---
    IEnumerator MoveToTargetWithAnim(Vector3 target)
    {
        Vector3 startPos = transform.position;
        float journeyLength = Vector3.Distance(startPos, target);
        float startTime = Time.time;

        if (playerAnimator != null) playerAnimator.SetMoving(true);

        Vector3 direction = (target - startPos).normalized;
        int targetDirection = 0;
        if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
            targetDirection = direction.z > 0 ? 3 : 0;
        else if (Mathf.Abs(direction.x) > 0.1f)
            targetDirection = direction.x > 0 ? 2 : 1;

        if (playerAnimator != null) playerAnimator.SetDirection(targetDirection);

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * cutsceneWalkSpeed;
            float fraction = Mathf.Clamp01(distCovered / journeyLength);
            transform.position = Vector3.Lerp(startPos, target, fraction);
            yield return null;
        }

        transform.position = target;
        if (playerAnimator != null) playerAnimator.SetMoving(false);
    }
}