using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    List<GameObject> interactableObjects = new List<GameObject>();

    void Update()
    {
        UpdateInteract();
    }

    void UpdateInteract()
    {
        if (interactableObjects.Count > 0)
        {
            KeyCode interactKey = InputManager.Instance.InteractKey();
            if (interactKey != KeyCode.None)
                HintManager.Instance.ShowHint(InputManager.Instance.interactKeys[0], "Interact");
        }

        if (InputManager.Instance.interact)
        {
            GameObject interactable = ClosestInteractableObject();
            if (interactable != null)
            {
                interactable.GetComponent<IInteractable>().Interact();
            }
        }
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
