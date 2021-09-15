using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorTransform : NetworkBehaviour
{

    public int packetsPerSecond = 5;
    float packetFrequency;
    float packetDelay;

    Vector3 predictionDestination;
    float predictionDestinyDistance;

    // Start is called before the first frame update
    void Start()
    {
        packetFrequency = 1f / packetsPerSecond;
        packetDelay = packetFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, predictionDestination, predictionDestinyDistance * (1f / packetFrequency) * Time.deltaTime);
            return;
        }

        packetDelay -= Time.deltaTime;
        if( packetDelay <= 0f)
        {
            Cmd_SyncPos(transform.position);
            packetDelay = packetFrequency;
        }
    }

    [Command]
    void Cmd_SyncPos(Vector3 _pos)
    {
        Rpc_SyncPos(_pos);
    }

    [ClientRpc]
    void Rpc_SyncPos(Vector3 _pos)
    {
        if (!isLocalPlayer)
        {
            predictionDestination = _pos;
            predictionDestinyDistance = Vector3.Distance(transform.position, predictionDestination);
            
            if(predictionDestinyDistance > 3f)
            {
                transform.position = _pos;
                predictionDestination = _pos;
                predictionDestinyDistance = 0f;
            }
        }
        //transform.position = _pos;
    }
}
