using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RestPointData
{
    public string pointName;
    public Vector3 position;
    public float pecentageTempMinus;

    public float triggerRadius = 5f;
    public DialogueData coordinateDialogue;
    public DialogueData zoneDialogue;
}