using UnityEngine;

public class DestroyInteractable : Interactable
{
    public override void Interact(Player player)
    {
        Destroy(gameObject);
        Debug.Log("Destroyed:" + gameObject.name);
    }
}