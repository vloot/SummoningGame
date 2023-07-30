using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool acceptInputs;

    private Camera _mainCamera;
    private LevelTiles _levelTiles;

    // character events
    public delegate void OnCharacterClickedDelegate(BaseCharacter character);
    public OnCharacterClickedDelegate OnCharacterClicked;

    // tile events
    public delegate void OnTileClickedDelegate(Vector3Int pos);
    public event OnTileClickedDelegate OnTileClicked;

    public Vector3Int ClickPosition
    {
        get; private set;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void Setup(LevelTiles levelTiles)
    {
        _levelTiles = levelTiles;
    }

    void Update()
    {
        if (!acceptInputs)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            CheckTileClicks(mousePos);
        }
    }

    private void CheckTileClicks(Vector3 clickPos)
    {
        if (UITools.ClickedOnUI()) return;

        var clickedTile = _levelTiles.GetTileByWorldPosition(clickPos);

        if (clickedTile != null)
        {
            ConsoleLogger.Log("Tile clicked at: " + clickedTile.tilePosition);
            ClickPosition = clickedTile.tilePosition;
            OnTileClicked?.Invoke(clickedTile.tilePosition);
        }
    }

    private void CheckCharacterClick()
    {
        // TODO do a raycast here intead of character being responsible for clicks?
    }
}
