using UnityEngine;

public class FallingBridge : MonoBehaviour
{
    [SerializeField] private GameObject Axe;
    private Rigidbody2D Rig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rig.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
