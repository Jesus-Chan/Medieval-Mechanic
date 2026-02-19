using UnityEngine;

public class Fish_enemy : EnemyMovment
{
    [SerializeField] private Transform Eyes;
    [SerializeField] private float RayDistance = 6f;
    [SerializeField] private LayerMask Player;
    [SerializeField] private Transform InLineOfSight;
    [SerializeField] private float chaseDuration = 6f; // hur lång tid fisken jagar spelaren

    private float chaseTimer = 0f; // countdown till att sluta jaga
    private bool isChasing = false; // trakar om fisken jagar spelaren 
    void Start()
    {
        Rend = GetComponent<SpriteRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == false)
        {
            
        }

        if (LineOfSight() == true)
        {
            isChasing = true;
            chaseTimer = chaseDuration; // resetar timer för chasen
            
        }
        else if (isChasing) // spelaren inte i lineofsight räknare ner timern
        {
            chaseTimer -= Time.deltaTime;
        }
        if (chaseTimer <= 0f) // timer tar slut slutar jaga spelaren
        {
            isChasing = false;
            canMove = true;
        }

        if (isChasing)
        {
            lockOn();
        }
    }




    private bool LineOfSight()
    {
        // Om flipx är san, alstå trunken riktas höger, så siktar raycasten höger
        Vector2 direction = Rend.flipX ? Vector2.right : Vector2.left;

        RaycastHit2D centerHit = Physics2D.Raycast(Eyes.position, direction, RayDistance, Player);
        RaycastHit2D upHit = Physics2D.Raycast(Eyes.position, Quaternion.Euler(0, 0, 25f) * direction, RayDistance, Player);
        RaycastHit2D downHit = Physics2D.Raycast(Eyes.position, Quaternion.Euler(0, 0, -25f) * direction, RayDistance, Player);

        Debug.DrawRay(Eyes.position, direction * RayDistance, Color.red, 0.25f);
        Debug.DrawRay(Eyes.position, (Quaternion.Euler(0, 0, 25f) * direction) * RayDistance, Color.yellow, 0.25f);
        Debug.DrawRay(Eyes.position, (Quaternion.Euler(0, 0, -25f) * direction) * RayDistance, Color.yellow, 0.25f);

        if ((centerHit.collider != null && centerHit.collider.CompareTag("Player")) ||
        (upHit.collider != null && upHit.collider.CompareTag("Player")) ||
        (downHit.collider != null && downHit.collider.CompareTag("Player")))

        {
            return true;

        }
        else
        {
            return false;
        }
        
    }

    private void lockOn()
    {
        canMove = false;
        // gör så att fisken kollar mot spelaren medeans den jagar 
        if (InLineOfSight.position.x > transform.position.x)
        {
            Rend.flipX = true;     // player is to the right
        }
        else
        {
            Rend.flipX = false;    // player is to the left
        }

        // math abs ger absulute värdet och gör så att fisken in backar bort från spelaren i bland
        transform.position = Vector2.MoveTowards(transform.position,InLineOfSight.position,Mathf.Abs(moveSpeed) * Time.deltaTime);
    }
}
