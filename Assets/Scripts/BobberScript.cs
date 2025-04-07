using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BobberScript : NetworkBehaviour
{
    [SyncVar] public bool gameIsOver;  // เพิ่ม SyncVar สำหรับ gameIsOver
    public Animator bobberAnim;
    public float bobberTime;

    void Update()
    {
        if (!isServer) return;  // ตรวจสอบว่าเป็น server หรือไม่

        bobberTime += Time.deltaTime;

        if (bobberTime >= 3)
        {
            RpcPlayBobberAnimation();  // เรียกใช้การเล่นอนิเมชัน
        }

        if (gameIsOver)
        {
            NetworkServer.Destroy(gameObject);  // ลบ bobber เมื่อจบเกม
        }
    }

    [ClientRpc]
    void RpcPlayBobberAnimation()
    {
        if (bobberAnim != null)
            bobberAnim.Play("bobberFish");  // เล่นอนิเมชัน
    }

    public void GameOver()
    {
        gameIsOver = true;  // เซ็ตค่าให้ gameIsOver เป็น true
    }
}
