using UnityEngine;
using UnityEngine.SceneManagement;


/// Singleton yang mengelola muatan scene dan siklus hidup game.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    ///Memuat scene berdasarkan nama
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    ///Keluar dari aplikasi
    public void QuitGame()
    {
        Debug.Log("Game Closed.");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}