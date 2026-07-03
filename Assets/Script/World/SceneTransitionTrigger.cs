using UnityEngine;
using UnityEngine.SceneManagement;


/// Memicu transisi ke scene pertempuran saat player memasuki trigger.

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "BattleScene";

    public void TriggerBattle()
    {
        Debug.Log("Memuat BattleScene...");
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerBattle();
        }
    }
}