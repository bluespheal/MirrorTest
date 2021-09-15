using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class player : NetworkBehaviour 
{
    public float speed = 5f;
    public GameObject prefabBullet;

    //for an proper sync, sync must be done by the server
    // when a new player joins, thet are in sync with how the server is
    [SyncVar(hook = nameof(ColorChanged))]
    Color myColor = Color.white;

    SyncList<int> listOfInts = new SyncList<int>(); //Mirror class, no need for [Syncvar] instruction, it's built in.

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = myColor;
        listOfInts.Callback += ListChanged;

        if (base.isLocalPlayer)
            instance = this;

        Camera.main.GetComponent<CameraFollow>().InnitFollow();
        
    }

    private void OnDestroy()
    {
        listOfInts.Callback -= ListChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        Vector3 move = Vector3.zero;
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        transform.Translate(move * speed * Time.deltaTime);

        if ( Input.GetKeyDown(KeyCode.C))
        {
            Cmd_ColorChange();   
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cmd_AddNumber();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cmd_CreateBullet();
        }
    }


    [Command] // Executes server-side
    void Cmd_CreateBullet()
    {
        //Traditional intantiation of a GO using Network Manager
        GameObject go = NetworkManager.Instantiate(prefabBullet, transform.position + Vector3.up, Quaternion.identity);
        //We can alter the GO before spawning it
        go.GetComponent<Rigidbody>().velocity = Vector3.up * 3f;
        go.GetComponent<MeshRenderer>().material.color = myColor;
        //Spawns the GO
        NetworkServer.Spawn(go);
        //If destroyed, the server has to do it.
    }

    [Command] // Executes server-side
    void Cmd_AddNumber()
    {
        int playerNumber = Random.Range(1,50); //Server chooses color and everyone follows suit.
        listOfInts.Add(playerNumber);
    }

    [Command] // Executes server-side
    void Cmd_ColorChange()
    {
        myColor = Random.ColorHSV(); //Server chooses color and everyone follows suit.
        Rpc_ColorChange(myColor);
    }

    [ClientRpc]
    void Rpc_ColorChange(Color _c)
    {
        GetComponent<MeshRenderer>().material.color = _c;
    }

    void ColorChanged(Color _old_color, Color _newColor)
    {
        print("Changed color to: " + _newColor);
    }

    //This function is called whenever the List is changed.
    void ListChanged(SyncList<int>.Operation _op, int _index, int _oldValue, int _newValue)
    {
        switch (_op)
        {
            case SyncList<int>.Operation.OP_ADD:
                print("Added new number: " + _newValue + " in the index number: " + _index);
                break;
            case SyncList<int>.Operation.OP_CLEAR:
                break;
            case SyncList<int>.Operation.OP_INSERT:
                break;
            case SyncList<int>.Operation.OP_REMOVEAT:
                break;
            case SyncList<int>.Operation.OP_SET:
                break;
        }
    }

    static player instance = null;

    public static player Instance
    {
        get
        {
            return instance;
        }
    }

}
