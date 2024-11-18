using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    private bool canInteract = false; // Can interact flag
    private GameObject interactableObj; // Current interactable object

    private void Update()
    {
        if (canInteract && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (interactableObj != null)
            {
                IInteractable interactable = interactableObj.GetComponent<IInteractable>();
                interactable?.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable") || collision.CompareTag("Item") || collision.CompareTag("NPC"))
        {
            canInteract = true; 
            interactableObj = collision.gameObject; // Store interactable object
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == interactableObj)
        {
            canInteract = false;
            interactableObj = null; 
        }
    }
}
