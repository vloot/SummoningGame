using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool acceptPlayerInputs;

    private Camera _mainCamera;
    private LevelTiles _levelTiles;

    // cached clicks
    public Tile Tile { get; private set; }
    public BaseCharacter ClickedCharacter { get; private set; }
    public Tile ClickedTile { get; private set; }

    // character events
    public delegate void OnCharacterClickedDelegate(BaseCharacter character);
    public OnCharacterClickedDelegate OnCharacterClicked;

    // tile events
    public delegate void OnTileClickedDelegate(Tile tile);
    public event OnTileClickedDelegate OnTileClicked;

    private const int CHARACTER_LAYER = 6;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void Setup(LevelTiles levelTiles)
    {
        _levelTiles = levelTiles;
        acceptPlayerInputs = true;
    }

    private void Update()
    {
        if (!acceptPlayerInputs)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ConsoleLogger.Log("Mouse click");
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            CheckTileClicks(mousePos);
            CheckCharacterClick(mousePos);
        }
    }

    private void CheckTileClicks(Vector3 clickPos)
    {
        if (UITools.ClickedOnUI()) return;

        var clickedTile = _levelTiles.GetTileByWorldPosition(clickPos);

        if (clickedTile != null)
        {
            ConsoleLogger.Log("Tile clicked at: " + clickedTile.position);
            ClickedTile = clickedTile;
            OnTileClicked?.Invoke(clickedTile);
        }
    }

    private void CheckCharacterClick(Vector3 clickPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.layer == CHARACTER_LAYER)
        {
            ClickedCharacter = hit.collider.gameObject.GetComponent<BaseCharacter>();

            if (ClickedCharacter != null)
            {
                ConsoleLogger.Log("Character hit: " + hit.collider.gameObject.name);
                OnCharacterClicked?.Invoke(ClickedCharacter);
            }
        }
    }

    public void SimulateClick(Tile tile)
    {
        ClickedTile = tile;
    }

    public void SimulateClick(BaseCharacter character)
    {
        ClickedCharacter = character;
    }
}
