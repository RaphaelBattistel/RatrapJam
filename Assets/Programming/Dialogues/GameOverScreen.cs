using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private CanvasGroup panelIMG; 
    [SerializeField] private DrillShip Ship;


    private DialogueData _currentDialogue;
    private int _currentLineIndex = 0;
    private Coroutine _typingCoroutine;
    private bool _isDialogueActive = false;
    private bool _isTypingComplete = false;

    public delegate void DialogueEndedDelegate();
    public event DialogueEndedDelegate OnDialogueEnded;

    public bool IsDialogueActive => _isDialogueActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        panelIMG.alpha = 0f;
    }

    private void Update()
    {
        if (_isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (_isTypingComplete)
            {
                NextLine();
            }
            else
            {
                // Complète le texte immédiatement
                StopCoroutine(_typingCoroutine);
                dialogueText.text = _currentDialogue.lines[_currentLineIndex].text;
                _isTypingComplete = true;
            }
        }
    }

    public void PlayDialogue(DialogueData dialogue)
    {
        if (_isDialogueActive) return; // Empêche de jouer deux dialogues en même temps

        _currentDialogue = dialogue;
        _currentLineIndex = 0;
        _isDialogueActive = true;

        dialoguePanel.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float duration = 0.3f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelIMG.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        panelIMG.alpha = 1f;
        DisplayLine(_currentLineIndex);
    }

    private void DisplayLine(int lineIndex)
    {
        if (lineIndex >= _currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        var line = _currentDialogue.lines[lineIndex];
        _isTypingComplete = false;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";

        foreach (char character in text)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(_currentDialogue.TextSpeed);
        }

        _isTypingComplete = true;
    }

    private void NextLine()
    {
        _currentLineIndex++;
        DisplayLine(_currentLineIndex);
    }

    private void EndDialogue()
    {
        _isDialogueActive = false;
        OnDialogueEnded?.Invoke(); // Alerte tous les abonnés
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float duration = 0.3f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelIMG.alpha = Mathf.Clamp01(1f - (elapsed / duration));
            yield return null;
        }

        panelIMG.alpha = 0f;
        dialoguePanel.SetActive(false);
    }
}