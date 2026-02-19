using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [SerializeField] private GameObject Platform1, Platform2, Platform3;
    [SerializeField] private float AppearanceTime;
    [SerializeField] private GameObject Timer;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private AudioClip clockSound;

    private bool isActive = false;
    private AudioSource audiosource;
    private float appearanceTimeCurrent;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        Platform1.SetActive(false);
        Platform2.SetActive(false);
        Platform3.SetActive(false);
        Timer.SetActive(false);
        appearanceTimeCurrent = AppearanceTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isActive = true;
            Platform1.SetActive(true);
            Platform2.SetActive(true);
            Platform3.SetActive(true);
            Timer.SetActive(true);
            appearanceTimeCurrent = AppearanceTime;
            audiosource.PlayOneShot(clockSound, 0.5f);
        }

    }

    private void Update()
    {


        if (isActive == true)
        {
            appearanceTimeCurrent -= Time.deltaTime;
            TimerText.text = appearanceTimeCurrent.ToString("F2");
            if (appearanceTimeCurrent <= 0)
            {
                isActive = false;
                appearanceTimeCurrent = AppearanceTime;
                Disappear();
            }
        }
    }

    private void Disappear()
    {
        Timer.SetActive(false);
        Platform1.SetActive(false);
        Platform2.SetActive(false);
        Platform3.SetActive(false);
    }

    
}
