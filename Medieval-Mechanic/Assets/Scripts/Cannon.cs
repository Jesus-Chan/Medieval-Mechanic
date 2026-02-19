using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float DelayBetweenShots;
    [SerializeField] private Transform ProjectileTrail;
    [SerializeField] private GameObject cannonballPrefab;
    [SerializeField] private AudioClip ShootSound;
    private float timeSinceLastShot = 0f;
    private bool canShoot = true;

    private AudioSource audiosource;
    protected SpriteRenderer Rend;
    private void Start()
    {
        Rend = GetComponent<SpriteRenderer>();
        audiosource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (canShoot == true)
        {
            canShoot = false;
            shoot();
            timeSinceLastShot = 0f;
            audiosource.PlayOneShot(ShootSound, 0.2f);
        }
        else if (canShoot == false)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= DelayBetweenShots)
            {
                canShoot = true;
            }

        }
    }

//    private void OnTriggerEnter2D(Collider2D other)
  //  {
    //    if (canShoot == true)
      //  {
      //      canShoot = false;
        //}
       // else
        //{
        //    canShoot = true;
        //}

    //}

    private void shoot()
    {
        Vector2 direction = Rend.flipX ? Vector2.right : Vector2.left;
        GameObject bullet = Instantiate(cannonballPrefab, ProjectileTrail.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().SetDirection(direction);
    }

}
