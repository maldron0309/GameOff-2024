using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;

    private bool canInteract = false;
    private GameObject interactableObj;

    private void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (canInteract && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (interactableObj != null)
            {
                IInteractable interactable = interactableObj.GetComponent<IInteractable>();
                interactable?.Interact();

                if (interactableObj.CompareTag("Item"))
                {
                    Debug.Log($"Item added to Inventory: {interactableObj.name}");
                    Destroy(interactableObj);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canInteract = true;
            interactableObj = collision.gameObject;

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
