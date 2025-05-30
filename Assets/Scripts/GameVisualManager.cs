using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class GameVisualManager : NetworkBehaviour
{
    private const float GRID_SIZE = 3.2f;

    [SerializeField] private Transform crossPrefabs;
    [SerializeField] private Transform circlePrefabs;
    [SerializeField] private Transform lineCompletePrefab;

    private List<GameObject> visualGameObjectList;

    private void Awake()
    {
        visualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        GameManager.Instance.OnClickedOnGridPosition += Instance_OnClickedOnGridPosition;
        GameManager.Instance.OnGameWin += GameManager_OnGameWin;
        GameManager.Instance.OnRematch += GameManager_OnRematch;
    }

    private void GameManager_OnRematch(object sender, System.EventArgs e)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        foreach (GameObject go in visualGameObjectList) { 
            Destroy(go);
        }
        visualGameObjectList.Clear();
    }

    private void GameManager_OnGameWin(object sender, GameManager.OnGameWinEventArgs e)
    {
        if(!NetworkManager.Singleton.IsServer) return;

        float eulerZ = 0f;
        switch (e.line.orientation)
        {
            default:
            case GameManager.Orientation.Horizontal: eulerZ = 0f; break;
            case GameManager.Orientation.Vertical: eulerZ = 90f; break;
            case GameManager.Orientation.DiagonalA: eulerZ = 45f; break;
            case GameManager.Orientation.DiagonalB: eulerZ = -45f; break;
        }
        Transform lineCompleteTransform =
            Instantiate(
                lineCompletePrefab,
                GetGridWorldPos(e.line.centerGridPosition.x, e.line.centerGridPosition.y),
                Quaternion.Euler(0, 0, eulerZ)
            );
        lineCompleteTransform.GetComponent<NetworkObject>().Spawn(true);
        visualGameObjectList.Add(lineCompleteTransform.gameObject);
    }

    private void Instance_OnClickedOnGridPosition(object sender, GameManager.OnClickedOnGridPositionEventArgs e)
    {
        SpawnObjectRpc(e.x, e.y, e.playerType);
    }

    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(int x, int y, GameManager.PlayerType playerType)
    {
        Transform prefabs;
        switch(playerType)
        {
            default:
            case GameManager.PlayerType.Cross:
                prefabs = crossPrefabs;
                break;
            case GameManager.PlayerType.Circle:
                prefabs = circlePrefabs;
                break;
        }
        Transform spawnedCrossTransform = Instantiate(prefabs, GetGridWorldPos(x, y), Quaternion.identity);
        spawnedCrossTransform.GetComponent<NetworkObject>().Spawn(true);
        visualGameObjectList.Add(spawnedCrossTransform.gameObject);
    }

    private Vector2 GetGridWorldPos(int x, int y)
    {
        return new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
    }
}
