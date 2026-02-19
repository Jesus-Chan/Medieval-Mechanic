using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject dialougeBox, finishedText, unfinishedText;
    [SerializeField] private int questGoal = 10;
    [SerializeField] private int levelToLoad;

    private Animator anim;
    private bool levelisLoading = false;
    private string sceneName;
    
    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool bunnyBossDefeated = GameObject.Find("Bunny_boss") == null;
            if (other.GetComponent<PlayerMovment>().CherriesCollected >= questGoal && sceneName != "Level 4") 
            {
                ShowFinishedDialogue();


            }

            else if (bunnyBossDefeated && sceneName == "Level 4")
            {
                ShowFinishedDialogue();
            }
            else
            {
                dialougeBox.SetActive(true);
                unfinishedText.SetActive(true);
                finishedText.SetActive(false);
            }
        }

    }
    private void ShowFinishedDialogue()
    {
        dialougeBox.SetActive(true);
        finishedText.SetActive(true);
        unfinishedText.SetActive(false);
        anim.SetTrigger("Flag");
        Invoke("LoadNextLevel", 4.0f);
        levelisLoading = true;
        
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelisLoading)
        {
            dialougeBox.SetActive(false);
            
            unfinishedText.SetActive(false);

        }
    }
}
