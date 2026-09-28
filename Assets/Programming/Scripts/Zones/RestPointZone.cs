using UnityEngine;

public class RestPointZone : MonoBehaviour
{

    private bool hasTriggered = false;
    private DialogueData dialogueCoord;
    private DialogueData dialogueZone;
    private DrillShip currentShip;
    private float temperatureReduction;
    private bool isLastZone = false;

    public void Initialize( DialogueData coord, DialogueData zone, float tempReduction, bool isLast = false)
    {

        dialogueCoord = coord;
        dialogueZone = zone;
        temperatureReduction = tempReduction;
        isLastZone = isLast;

        if (isLastZone)
        {
            GameOverScreen.Instance.OnDialogueEnded += ExitGame;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.TryGetComponent<DrillShip>(out DrillShip ship))
        {
            hasTriggered = true;
            currentShip = ship;

            ship.ReduceTemperature(temperatureReduction);
            ship.enabled = false;
            GameOverScreen.Instance.PlayDialogue(dialogueZone);
        }

        if (other.tag == "Detector")
        {
            if (dialogueCoord != null)
            {
                dialogueCoord.lines[0].text = $"Zone détéctée aux coordonées :\nX: {(int)transform.position.x}\nY: {(int)transform.position.y}\nZ: {(int)transform.position.z}";
                DialogueManager.Instance.PlayDialogue(dialogueCoord);
            }
        }
    }

    private void ExitGame()
    {
        if (isLastZone)
        {
            GameOverScreen.Instance.OnDialogueEnded -= ExitGame;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }

    private void OnDestroy()
    {
        GameOverScreen.Instance.OnDialogueEnded -= ExitGame;
    }
}