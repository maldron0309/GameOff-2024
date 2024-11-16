using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    private List<GameObject> interactables;
    public GameObject indicatorPrefab;
    private Dictionary<GameObject, GameObject> interactionInstances;
    private void Awake()
    {
        interactables = new List<GameObject>();
        interactionInstances = new Dictionary<GameObject, GameObject>();
    }
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable") || collision.CompareTag("Item") || collision.CompareTag("NPC"))
        {
            if (!interactables.Contains(collision.gameObject))
            {
                interactables.Add(collision.gameObject);
                GameObject indicator = Instantiate(indicatorPrefab, collision.gameObject.transform.position, Quaternion.identity);
                interactionInstances.Add(collision.gameObject, indicator);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactables.Contains(collision.gameObject))
        {
            if (interactionInstances.ContainsKey(collision.gameObject))
            {
                Destroy(interactionInstances[collision.gameObject]);
                interactionInstances.Remove(collision.gameObject);
            }
            interactables.Remove(collision.gameObject);
        }
    }
}
