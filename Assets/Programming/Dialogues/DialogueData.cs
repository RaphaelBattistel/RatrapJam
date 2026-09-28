using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "Game/Dialogue")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 4)] public string text; // le texte
    }

    public List<DialogueLine> lines = new List<DialogueLine>();
    [SerializeField] private float textSpeed = 0.05f; // délai entre chaque caractère

    public float TextSpeed => textSpeed;
}