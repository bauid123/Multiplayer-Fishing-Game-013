using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;  // ใช้สำหรับ Text UI
using UnityEngine.SceneManagement;  // เพิ่มเพื่อโหลด scene

public class FishingBarScript : NetworkBehaviour
{
    [SyncVar] public float targetTime = 4.0f;
    public Rigidbody rb;
    public bool atTop;
    public float savedTargetTime;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;
    public GameObject p7;
    public GameObject p8;
    public GameObject p9;
    public GameObject p10;

    public bool onFish;
    public PlayeScript playerS;
    public GameObject bobber;
    void Start()
    {   
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }

    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (onFish)
        {
            targetTime += Time.deltaTime;
        }
        else
        {
            targetTime -= Time.deltaTime;
        }

        if (targetTime <= 0.0f)
        {
            onFish = false;
            SceneManager.LoadScene(1);
            playerS.fishGameLossed();
            targetTime = 4.0f;
        }
        else if (targetTime >= 10.0f)
        {
            onFish = false;
            SceneManager.LoadScene(2);
            playerS.fishGameWon();
            targetTime = 4.0f;

        }

        // Fishing bar visual logic
        GameObject[] segments = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 };
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].SetActive(targetTime >= i + 1);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up, ForceMode.Impulse);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer) return;

        if (other.gameObject.CompareTag("fish"))
        {
            onFish = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer) return;

        if (other.gameObject.CompareTag("fish"))
        {
            onFish = false;
        }
    }
}
