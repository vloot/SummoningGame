using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseDebug : MonoBehaviour
{
    private const string mousePosLabel = "Mouse pos";
    private const string selectedTileLabel = "Selected tile";

    private Camera _mainCamera;

    public Tilemap map;
    public LevelTiles levelTiles;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        DebugTools.ScreenLogger.Instance.AddLine(mousePosLabel, "");
        DebugTools.ScreenLogger.Instance.AddLine(selectedTileLabel, "");
    }

    private void Update()
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var tilePos = map.WorldToCell(mousePos);
        tilePos.z = 0;

        DebugTools.ScreenLogger.Instance.UpdateLine(mousePosLabel, mousePos.ToString());
        DebugTools.ScreenLogger.Instance.UpdateLine(selectedTileLabel, tilePos.ToString());
    }
}
