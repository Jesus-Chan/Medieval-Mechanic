using UnityEngine;

public class CheckpointToolbox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color activatedColor = Color.green;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            PlayerMovment player = other.GetComponent<PlayerMovment>();

            if (player != null)
            {
                player.SetSpawnPoint(transform);

                
                spriteRenderer.color = activatedColor;

                activated = true;

                Debug.Log("Checkpoint Activated!");
            }
        }
    }
}
