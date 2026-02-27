using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialougeBox;
    [SerializeField] private GameObject finishedText;
    [SerializeField] private GameObject unfinishedText;

    [Header("Quest Settings")]
    [SerializeField] private int questGoal = 5;
    [SerializeField] private string sceneToLoad;   // <-- Type scene name here

    [Header("Pump Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite activeSprite;

    private SpriteRenderer spriteRenderer;
    private bool levelIsLoading = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Make sure pump starts with idle sprite
        if (idleSprite != null)
            spriteRenderer.sprite = idleSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovment player = other.GetComponent<PlayerMovment>();

        if (player == null) return;

        if (player.CogsKilled >= questGoal)
        {
            ShowFinishedDialogue();
        }
        else
        {
            ShowUnfinishedDialogue();
        }
    }

    private void ShowFinishedDialogue()
    {
        if (levelIsLoading) return;

        dialougeBox.SetActive(true);
        finishedText.SetActive(true);
        unfinishedText.SetActive(false);

        // Switch pump sprite
        if (activeSprite != null)
            spriteRenderer.sprite = activeSprite;

        levelIsLoading = true;

        // Load next level after 3 seconds
        Invoke(nameof(LoadNextLevel), 3f);
    }

    private void ShowUnfinishedDialogue()
    {
        dialougeBox.SetActive(true);
        unfinishedText.SetActive(true);
        finishedText.SetActive(false);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!levelIsLoading)
        {
            dialougeBox.SetActive(false);
            unfinishedText.SetActive(false);
        }
    }
}
