using UnityEngine;


/// Mengontrol pemutaran musik latar (BGM) secara global di seluruh scene.

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        // Pastikan script ini menempel pada objek yang sudah DontDestroyOnLoad (GameManager)
        // Jika terjadi duplikasi, hancurkan objek yang baru.
        if (FindObjectsOfType<BGMManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.volume = 0.5f; // <--- Tambahkan baris ini!
            audioSource.Play();
        }
    }

    /// <summary>Mengatur volume musik (0.0 - 1.0).</summary>
    public void SetVolume(float volume)
    {
        if (audioSource != null) audioSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>Menghentikan musik.</summary>
    public void StopMusic()
    {
        if (audioSource != null) audioSource.Stop();
    }
}