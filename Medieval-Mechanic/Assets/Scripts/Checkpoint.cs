using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform Spawnpoint;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            anim.SetBool("Checkpoint", true);
            Spawnpoint.position = transform.position;
        }
    }

}