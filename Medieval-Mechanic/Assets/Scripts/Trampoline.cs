using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce = 200f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, 0);
            playerRigidbody.AddForce(new Vector2 (0, jumpForce));
            GetComponent<Animator>().SetTrigger("Jump");
        }
    }
}
