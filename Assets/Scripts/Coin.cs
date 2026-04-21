using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        EventManager.RaiseEvent(new OnCoinCollected());
        Destroy(gameObject);
    }
}