using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class bullet : MonoBehaviour
{
    //[ClientCallback] <---- This one is only called on the clients
    [ServerCallback] //Only the server calls this function
    
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
