using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePlaneCollider : MonoBehaviour
{

    [SerializeField] private Transform targetPlayer;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = targetPlayer.position.x;
        temp.z = targetPlayer.position.z;
        transform.position = temp;
    }
}
