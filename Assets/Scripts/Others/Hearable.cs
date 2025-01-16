using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearable : MonoBehaviour
{
    [Header("Sound settings")]
    public float soundRadius;

    string k_playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(k_playerTag))
        {
            other.gameObject.GetComponent<PlayerMovement>().MakeSound(soundRadius);
        }
    }
}
