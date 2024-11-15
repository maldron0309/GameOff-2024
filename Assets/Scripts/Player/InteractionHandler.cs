using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI; // UI for interaction
    [SerializeField] private DialogGraph dialogGraph;

    private bool canInteract = false; // Can interact flag
    private GameObject interactableObj; // Current interactable object

    private void Start()
    {
        interactionUI?.SetActive(false);
    }

    private void Update()
    {
        if (canInteract && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (interactableObj != null)
            {
                IInteractable interactable = interactableObj.GetComponent<IInteractable>();
                if (interactable is BaseNPC)
                {
                    DialogSystem.instance?.StartDialog(dialogGraph); // Start dialog with NPC
                }
                else
                {
                    interactable?.Interact(); // Interact with other objects
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canInteract = true; 
            interactableObj = collision.gameObject; // Store interactable object

            interactionUI?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canInteract = false;
            interactableObj = null; 

            interactionUI?.SetActive(false); 
        }
    }
}
