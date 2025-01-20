using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Material daylight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            RenderSettings.skybox = daylight;
            CutsceneManager.Instance.LoadNextCutscene();
        }
    }
}
