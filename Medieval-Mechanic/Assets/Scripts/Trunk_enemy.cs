using UnityEngine;

public class Trunk_enemy : EnemyMovment
{
    [SerializeField] private Transform Eyes;
    [SerializeField] private LayerMask Player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float ShootCooldown = 0f;
    [SerializeField] private float RayDistance = 6f;


    private float TimeBetweenShots = 0f;

    private Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        anim = GetComponent<Animator>();
        Rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeBetweenShots -= Time.deltaTime;
        Shoot();
        anim.SetFloat("moveSpeed", Mathf.Abs(moveSpeed));
    }

    private void Shoot()
    {
        if (LineOfSight() == true)
        {
            GetComponent<Animator>().SetBool("Idle", true);
            canMove = false;
            if (TimeBetweenShots <= 0f)
            {
                GetComponent<Animator>().SetTrigger("Shoot");
                //start shoot animation
                TimeBetweenShots = ShootCooldown;
            }



        }
        else { canMove = true; GetComponent<Animator>().SetBool("Idle", false); }

    }
    // This function is called as an animation event
    public void FireBullet()
    {
        Vector2 direction = Rend.flipX ? Vector2.right : Vector2.left;
        GameObject bullet = Instantiate(bulletPrefab, Eyes.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().SetDirection(direction);

    }

    private bool LineOfSight()
    {
        // Om flipx är san, alstå trunken riktas höger, så siktar raycasten höger
        Vector2 direction = Rend.flipX ? Vector2.right : Vector2.left;

        RaycastHit2D EyeSight = Physics2D.Raycast(Eyes.position, direction, RayDistance, Player);

        Debug.DrawRay(Eyes.position, direction * RayDistance, Color.green, 0.25f);

        if (EyeSight.collider != null && EyeSight.collider.CompareTag("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
