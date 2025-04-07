using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayeScript : NetworkBehaviour
{
    public Animator playerAnim;
    public bool isFishing;
    public bool poleBack;
    public bool throwBobber;
    public Transform fishingPoint;
    public GameObject bobber;

    public float targetTime = 0.0f;
    public float savedTargetTime;
    public float extraBobberDistance;

    public GameObject fishGame;
    public float timeTillCatch = 0.0f;
    public bool winnerAnim;

    void Start()
    {
        if (!isLocalPlayer) return;

        isFishing = false;
        fishGame.SetActive(false);
        throwBobber = false;
        targetTime = 0.0f;
        savedTargetTime = 0.0f;
        extraBobberDistance = 0.0f;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space) && !isFishing && !winnerAnim)
        {
            poleBack = true;
        }

        if (isFishing)
        {
            timeTillCatch += Time.deltaTime;
            if (timeTillCatch >= 3)
            {
                fishGame.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && !isFishing && !winnerAnim)
        {
            poleBack = false;
            isFishing = true;
            throwBobber = true;

            extraBobberDistance += (targetTime >= 3) ? 3 : targetTime;
        }

        Vector3 temp = new Vector3(extraBobberDistance, 0, 0);
        fishingPoint.transform.position += temp;

        if (poleBack)
        {
            playerAnim.Play("playerSwingBack");
            savedTargetTime = targetTime;
            targetTime += Time.deltaTime;
        }

        if (isFishing)
        {
            if (throwBobber)
            {
                CmdThrowBobber(fishingPoint.position, fishingPoint.rotation);
                fishingPoint.transform.position -= temp;
                throwBobber = false;
                targetTime = 0.0f;
                savedTargetTime = 0.0f;
                extraBobberDistance = 0.0f;
            }

            playerAnim.Play("playerFishing");
        }

        if (Input.GetKeyDown(KeyCode.P) && timeTillCatch <= 3)
        {
            playerAnim.Play("playerStill");
            poleBack = false;
            throwBobber = false;
            isFishing = false;
            timeTillCatch = 0;
        }
    }

    [Command]
    void CmdThrowBobber(Vector3 position, Quaternion rotation)
    {
        GameObject bobberInstance = Instantiate(bobber, position, rotation);
        NetworkServer.Spawn(bobberInstance, connectionToClient);
        RpcSetBobber(bobberInstance);
    }

    [ClientRpc]
    void RpcSetBobber(GameObject instance)
    {
        bobber = instance;
    }

    public void fishGameWon()
    {
        playerAnim.Play("playerWonFish");
        fishGame.SetActive(false);
        ResetFishState();
    }

    public void fishGameLossed()
    {
        playerAnim.Play("playerStill");
        fishGame.SetActive(false);
        ResetFishState();
    }

    void ResetFishState()
    {
        poleBack = false;
        throwBobber = false;
        isFishing = false;
        timeTillCatch = 0;
    }
}
