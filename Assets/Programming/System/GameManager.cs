using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<RestPointData> restPoints = new List<RestPointData>();
    [SerializeField] private DialogueData dialogue;

    public List<RestPointData> RestPoints => restPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(PlayIntroAfterDelay());
    }

    private System.Collections.IEnumerator PlayIntroAfterDelay()
    {
        yield return null; // Attend une frame
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.PlayDialogue(dialogue);
    }
    public void LoadMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}