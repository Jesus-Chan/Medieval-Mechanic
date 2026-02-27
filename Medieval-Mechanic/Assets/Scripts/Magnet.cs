using UnityEngine;

public class Magnet : MonoBehaviour
{


    [SerializeField] private float magnetStrength = 10f; // Strength of the magnetic force
    // Choose direction in Inspector
    public Vector2 forceDirection = Vector2.right;
  /*Right  1, 0

    Left  -1, 0

    Up     0, 1

    Down   0,-1*/


    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(forceDirection.normalized * magnetStrength);
        }
    }


}
