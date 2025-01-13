using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent father;

    void Start()
    {
        father = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        father.destination = player.position;
    }

}
