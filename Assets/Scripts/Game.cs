using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : NetworkBehaviour
{
    public static Game Instance;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject playMode;
    [ClientRpc]
    private void RpcGameOver()
    {
        if (gameOverPanel != null)
        {
            playModeStop();
            gameOverPanel.SetActive(true);
        }

    }

    [Command(requiresAuthority = false)]
    public void CmdRestartGame()
    {
        Game.Instance.RpcRestartGame();
    }
    [ClientRpc]
    private void RpcRestartGame()
    {
        
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
        SceneManager.LoadScene(0);
    }
    [ClientRpc]
    private void RpcVictory()
    {
        if (victoryPanel != null)
        {
            playModeStop();
            victoryPanel.SetActive(true);
        }

    }

    void playModeStop()
    {
        if (playMode != null)
            playMode.SetActive(false);
    }
}
