using UnityEngine;

public class CogEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed at which the cog moves
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = pointB; // Start moving towards pointB
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
