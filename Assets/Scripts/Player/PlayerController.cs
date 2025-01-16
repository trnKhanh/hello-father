using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    HashSet<GameObject> interactableObjects = new HashSet<GameObject>();

    void Update()
    {
        UpdateInteract();
    }

    void UpdateInteract()
    {
        FilterObject();
        if (interactableObjects.Count > 0)
        {
            GameObject interactable = ClosestInteractableObject();

            KeyCode interactKey = InputManager.Instance.InteractKey();
            string description = interactable.GetComponent<IInteractable>().GetDescription();
            if (description == null)
                description = "Interact";

            if (interactKey != KeyCode.None)
                HintManager.Instance.ShowHint(interactKey, description);

            if (InputManager.Instance.interact)
            {
                if (interactable != null)
                {
                    interactable.GetComponent<IInteractable>().Interact();
                }
            }
        }
    }

    void FilterObject()
    {
        interactableObjects.RemoveWhere((gameObject) =>
        {
            return gameObject == null;
        });
    }

    GameObject ClosestInteractableObject()
    {
        GameObject closestGameObject = null;


        foreach (GameObject obj in interactableObjects)
        {
            if (closestGameObject == null)
                closestGameObject = obj;
            else
            {
                float closestDistance = (transform.position - closestGameObject.transform.position).magnitude;
                float currentDistance = (transform.position - obj.transform.position).magnitude;
                if (currentDistance < closestDistance)
                    closestGameObject = obj;
            }
        }
        return closestGameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable))
            interactableObjects.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable))
            interactableObjects.Remove(other.gameObject);
    }
}
