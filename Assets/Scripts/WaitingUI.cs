using UnityEngine;

public class WaitingUI : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
    }

    private void GameManager_OnGameStarted(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }
}
