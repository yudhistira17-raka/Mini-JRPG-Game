using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public enum BattleState { Idle, PlayerTurn, EnemyTurn, Victory, GameOver, Finished }


/// Mengatur seluruh alur pertempuran: giliran, AI musuh, UI, dan akhir battle.

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [SerializeField] private BaseCharacter player;
    [SerializeField] private BaseCharacter enemy;
    [SerializeField] private GameObject actionsPanel;
    public Text resultText;

    private BattleState currentState;
    private List<BaseCharacter> turnQueue = new List<BaseCharacter>();
    private int currentTurnIndex = 0;
    private IBattleAction currentAction;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        StartBattle();
    }

    void StartBattle()
    {
        if (player == null || enemy == null)
        {
            Debug.LogError("Player atau Enemy belum diisi di Inspector!");
            return;
        }

        if (resultText != null) resultText.gameObject.SetActive(false);

        turnQueue = new List<BaseCharacter> { player, enemy }
            .Where(c => c != null)
            .OrderByDescending(c => c.stats.speed)
            .ToList();

        currentTurnIndex = 0;
        currentState = BattleState.Idle;

        UpdateAllUI();
        NextTurn();
    }

    void NextTurn()
    {
        if (turnQueue[currentTurnIndex].IsDead)
        {
            currentTurnIndex = (currentTurnIndex + 1) % turnQueue.Count;
            CheckBattleEnd();
            return;
        }

        BaseCharacter currentActor = turnQueue[currentTurnIndex];

        if (currentActor == player)
        {
            currentState = BattleState.PlayerTurn;
            if (actionsPanel != null) actionsPanel.SetActive(true);
        }
        else if (currentActor == enemy)
        {
            currentState = BattleState.EnemyTurn;
            if (actionsPanel != null) actionsPanel.SetActive(false);
            EnemyAI(currentActor);
        }
    }

    public void PlayerSelectAction(string actionType)
    {
        if (currentState != BattleState.PlayerTurn) return;

        if (actionsPanel != null) actionsPanel.SetActive(false);

        BaseCharacter attacker = player;
        BaseCharacter target = enemy;

        if (attacker == null || target == null) return;

        switch (actionType)
        {
            case "Attack": currentAction = new AttackAction(); break;
            case "Skill":
                if (attacker.stats.currentMP < 5)
                {
                    Debug.Log("⚠️ MP tidak cukup! Skill tidak bisa digunakan.");
                    if (actionsPanel != null) actionsPanel.SetActive(true);
                    return;
                }
                currentAction = new SkillAction("Fire Slash", 5);
                break;
            case "Guard": currentAction = new GuardAction(); break;
            default: return;
        }

        currentAction.Execute(attacker, target);
        EndPlayerTurn();
    }

    void EndPlayerTurn()
    {
        currentState = BattleState.Idle;
        currentTurnIndex = (currentTurnIndex + 1) % turnQueue.Count;
        CheckBattleEnd();
        Invoke(nameof(NextTurn), 1.5f);
    }

    void EnemyAI(BaseCharacter enemyActor)
    {
        int randomChance = Random.Range(0, 100);
        IBattleAction enemyAction;

        bool canUseSkill = enemyActor.stats.currentMP >= 5;

        if (randomChance < 60) enemyAction = new AttackAction();
        else if (randomChance < 90 && canUseSkill) enemyAction = new SkillAction("Enemy Fire Slash", 5);
        else enemyAction = new GuardAction();

        enemyAction.Execute(enemyActor, player);

        currentTurnIndex = (currentTurnIndex + 1) % turnQueue.Count;
        CheckBattleEnd();
        Invoke(nameof(NextTurn), 1.5f);
    }

    void UpdateAllUI()
    {
        CharacterUI playerUI = player.GetComponent<CharacterUI>();
        if (playerUI != null) playerUI.UpdateHP(player.stats.currentHP, player.stats.maxHP);

        CharacterUI enemyUI = enemy.GetComponent<CharacterUI>();
        if (enemyUI != null) enemyUI.UpdateHP(enemy.stats.currentHP, enemy.stats.maxHP);
    }

    void CheckBattleEnd()
    {
        if (player.IsDead)
        {
            currentState = BattleState.GameOver;
            FinishBattle("💀 Game Over...");
        }
        else if (enemy.IsDead)
        {
            currentState = BattleState.Victory;
            FinishBattle("🎉 Victory!");
        }
    }

    void FinishBattle(string message)
    {
        if (actionsPanel != null) actionsPanel.SetActive(false);

        if (resultText != null)
        {
            resultText.text = message + "\n\nPress 'E' to return to town...";
            resultText.gameObject.SetActive(true);
        }

        currentState = BattleState.Finished;
    }

    void Update()
    {
        if (currentState == BattleState.Finished && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoadScene("MainScene");
            else
                Debug.LogError("GameManager tidak ditemukan! Pastikan ada di scene ini.");
        }
    }
}