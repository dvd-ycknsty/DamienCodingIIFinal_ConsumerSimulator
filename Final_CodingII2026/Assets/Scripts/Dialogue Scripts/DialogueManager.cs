using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI lineText;
    public Transform choicesContainer;
    public Button choicesButtonPrefab;

    private NPCData currentNode;
    private int lineIndex;
    private bool isActive;

    private Player player;

    private void Awake()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        ClearChoices();

        player = FindFirstObjectByType<Player>();
    }

    void OnEnable()
    {
        Player.OnDialogueRequested += StartDialogue;
    }

    void OnDisable()
    {
        Player.OnDialogueRequested -= StartDialogue;
    }
    void StartDialogue(NPCData npcData)
    {
        if (npcData == null)
        {
            Debug.Log("NPC Data is null");
            return;
        }

        currentNode = npcData;
        lineIndex = 0;
        isActive = true;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        ShowLine();
    }

    bool HasChoices(NPCData node)
    {
        return node != null && node.choices != null && node.choices.Length > 0;
    }

    void Advance()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        lineIndex++;

        if (currentNode.lines != null && lineIndex < currentNode.lines.Length)
        {
            if (lineText != null)
            {
                lineText.text = currentNode.lines[lineIndex];
                return;
            }
        }

        FinishNode();
    }

    void ShowChoices(DialogueChoices[] choices)
    {
        ClearChoices();
        if (choicesContainer == null || choicesButtonPrefab == null)
        {
            Debug.Log("choices are not wired");
            return;
        }

        foreach (DialogueChoices choice in choices)
        {
            Button bttn = Instantiate(choicesButtonPrefab, choicesContainer);
            TextMeshProUGUI tmp = bttn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = choice.choiceText;

            NPCData next = choice.nextNode;

            bttn.onClick.AddListener(() =>
            {
                Choose(next);
            });
        }
    }

    void FinishNode()
    {
        if (HasChoices(currentNode))
        {
            ShowChoices(currentNode.choices);
            return;
        }

        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            lineIndex = 0;
            ShowLine();
            return;
        }

        EndDialogue();
    }

    void ShowLine()
    {
        ClearChoices();

        if (currentNode == null)
        {
            EndDialogue();
        }

        if (displayName != null) displayName.text = currentNode.displayName;

        if (currentNode.lines == null || currentNode.lines.Length == 0)
        {
            FinishNode();
            return;
        }

        lineIndex = Mathf.Clamp(lineIndex, 0, currentNode.lines.Length - 1);

        if (lineText != null) lineText.text = currentNode.lines[lineIndex];
    }

    void Choose(NPCData nextNode)
    {
        ClearChoices();

        if (nextNode == null)
        {
            EndDialogue();
            return;
        }
    }

    bool ChoicesAreShowing()
    {
        return choicesContainer != null && choicesContainer.childCount > 0;
    }

    void ClearChoices()
    {
        if (choicesContainer == null) return;

        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }

    void EndDialogue()
    {
        isActive = false;
        currentNode = null;
        lineIndex = 0;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }
}