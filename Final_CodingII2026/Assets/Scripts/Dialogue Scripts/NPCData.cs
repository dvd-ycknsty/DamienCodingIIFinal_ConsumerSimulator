using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Data")]

public class NPCData : ScriptableObject
{
    [Header("Speaker")]
    public string displayName;

    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string[] lines;

    [Header("if there are no choices, we show buttons after line ends")]
    public DialogueChoices[] choices;

    [Header("if no choices, auto continue to the next node")]
    public NPCData nextNode;
}

[System.Serializable]
public class DialogueChoices
{
    public string choiceText;
    public NPCData nextNode;
}