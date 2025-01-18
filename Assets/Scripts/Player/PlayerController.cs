using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable, IGameData
{
    [Serializable]
    public class PlayerData
    {
        public Vector3 position;
    }

    [Header("References")]
    public ThirdPersonCameraController cameraController;

    HashSet<GameObject> interactableObjects = new HashSet<GameObject>();

    PlayerMovement playerMovement;
    PlayerAudioManager audioManager;
    Animator animator;

    string k_die = "Die";

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        audioManager = GetComponent<PlayerAudioManager>();
    }

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

    public void Hit()
    {
        Die();
    }

    void Die()
    {
        cameraController.ChangeCameraStyle(ThirdPersonCameraController.CameraStyle.Basic);
        InputManager.Instance.controlActive = false;
        animator.SetTrigger(k_die);
        audioManager.Die();
        Invoke(nameof(Gameover), 1);
    }

    void Gameover()
    {
        GameoverTabManager.Instance.GameOver();
    }

    public void Save(string root)
    {
        string savePath = Path.Join(root, "player.json");
        PlayerData playerData = new PlayerData();

        playerData.position = transform.position;

        Debug.Log(String.Format("Save player to {0}", savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(playerData));
    }

    public void Load(string root)
    {
        try
        {
            string savePath = Path.Join(root, "player.json");
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(savePath));
            transform.position = playerData.position;
            Debug.Log(transform.position);
            Debug.Log(playerData.position);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
