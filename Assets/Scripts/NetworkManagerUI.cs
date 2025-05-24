using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] private GameObject waitingUI;   

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            waitingUI.SetActive(true);
            Hide();
        });
        startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            waitingUI.SetActive(true);
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
