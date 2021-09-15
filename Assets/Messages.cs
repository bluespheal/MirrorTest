using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Messages : MonoBehaviour
{

    public struct MessageStructure : NetworkMessage
    {
        public int aNumber;
        public float aFloat;
        public Color aColor;
        public string aString;
    }

    public void SendMessage(int _i, float _f, Color _c, string _s)
    {
        MessageStructure message = new MessageStructure()
        {
            aNumber = _i,
            aFloat = _f,
            aColor = _c,
            aString = _s
        };

        NetworkServer.SendToAll(message);
    }

    public void ReceiveMessage(MessageStructure _message)
    {
        print(_message.aNumber);
    }

    void Start()
    {
        NetworkClient.RegisterHandler<MessageStructure>(ReceiveMessage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SendMessage(10, 1.5f, Color.red, "Hello World");
        }
    }
   
}
