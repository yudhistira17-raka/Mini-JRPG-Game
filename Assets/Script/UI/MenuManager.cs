using UnityEngine;


/// Mengatur panel menu utama, transisi dari "press any button" ke menu.

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pressAnyButtonPanel;
    [SerializeField] private GameObject mainMenuPanel;

    private bool hasStarted = false;

    void Start()
    {
        mainMenuPanel.SetActive(false);
        pressAnyButtonPanel.SetActive(true);
    }

    void Update()
    {
        if (!hasStarted && Input.anyKeyDown)
        {
            hasStarted = true;
            ShowMainMenu();
        }
    }

    private void ShowMainMenu()
    {
        pressAnyButtonPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.Instance.LoadScene("MainScene");
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game dipilih (belum diimplementasikan)");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings dipilih (belum diimplementasikan)");
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}