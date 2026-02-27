using UnityEngine;
using UnityEngine.Audio;

public class Wall_Fall : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private AudioClip UnlockSound;


    private Animator anim;
    private AudioSource audioSource;

    private bool hasPlayedAnimation = false;


    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !hasPlayedAnimation)
        {
            button.SetActive(false);   
            hasPlayedAnimation = true;
            anim.SetTrigger("Move");
            audioSource.PlayOneShot(UnlockSound, 0.5f);

        }
    }
}

