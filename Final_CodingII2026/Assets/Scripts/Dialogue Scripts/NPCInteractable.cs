using UnityEngine;

public class NPCInteractable : Interactable
{
    public NPCData npcData;

    public override void Interact(Player player)
    {
        if (npcData == null)
        {
            Debug.Log("Npc has no data: " + gameObject.name);
        }

        //if we are interacting with the npc and it has data then request dialogue
        player.RequestDialogue(npcData);
    }
}