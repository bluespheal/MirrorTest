using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Transform playerPos = null;
    
    public void InnitFollow()
    {
        if (player.Instance)
        {
            playerPos = player.Instance.transform;
            this.enabled = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = playerPos.position;
        pos.z = 5f;
        transform.position = pos;
    }
}
