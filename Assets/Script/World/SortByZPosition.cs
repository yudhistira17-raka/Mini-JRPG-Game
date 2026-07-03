using UnityEngine;


/// Mengatur urutan sorting sprite berdasarkan posisi Z untuk efek 2.5D.

public class SortByZPosition : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Objek dengan Z lebih kecil (lebih dekat ke kamera) digambar di depan.
        spriteRenderer.sortingOrder = (int)(-transform.position.z * 100);
    }
}