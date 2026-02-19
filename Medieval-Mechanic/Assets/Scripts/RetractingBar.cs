using UnityEngine;
using UnityEngine.Audio;

public class RetractingBar : MonoBehaviour
{

    [SerializeField] private GameObject button;
    [SerializeField] private AudioClip UnlockSound;

    private Animator anim;
    private AudioSource audioSource;
    private bool hasPlayedAnimation = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayedAnimation)
        {
            button.SetActive(true);
            hasPlayedAnimation = true;
            anim.SetTrigger("ClickedBu");
            audioSource.PlayOneShot(UnlockSound, 0.5f);
        }
    }

}
