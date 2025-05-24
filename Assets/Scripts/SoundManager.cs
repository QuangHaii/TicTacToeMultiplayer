using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Transform placeSfxPrefab;
    [SerializeField] private Transform winSfxPrefab;
    [SerializeField] private Transform loseSfxPrefab;
    [SerializeField] private GameObject backgroundMusic;
    [SerializeField] private Button muteButton;
    private bool isMuted = false;

    private void Start()
    {
        GameManager.Instance.OnPlacedObject += GameManager_OnPlacedObject;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        muteButton.onClick.AddListener(() =>
        {
            if (!isMuted)
            {
                isMuted = true;
                backgroundMusic.SetActive(false);
            }
            else
            {
                isMuted = false; 
                backgroundMusic.SetActive(true);
            }
        });
    }

    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinEventArgs e)
    {
        if (isMuted) return;
        if(GameManager.Instance.GetLocalPlayerType() == e.winPlayerType)
        {
            Transform sfxTransform = Instantiate(winSfxPrefab);
            Destroy(sfxTransform.gameObject, 5f);
        }
        else
        {
            Transform sfxTransform = Instantiate(loseSfxPrefab);
            Destroy(sfxTransform.gameObject, 5f);
        }
    }

    private void GameManager_OnPlacedObject(object sender, System.EventArgs e)
    {
        if (isMuted) return;
        Transform sfxTransform = Instantiate(placeSfxPrefab);
        Destroy(sfxTransform.gameObject,5f);
    }
}
