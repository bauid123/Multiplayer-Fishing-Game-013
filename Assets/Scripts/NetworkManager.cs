using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{

    public Transform left;
    public Transform right;


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform start = numPlayers == 0 ? left : right;

        GameObject player = Instantiate(playerPrefab,start.position,start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
    }
}

