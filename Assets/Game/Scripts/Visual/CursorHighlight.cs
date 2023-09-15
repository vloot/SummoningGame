using UnityEngine;

public class CursorHighlight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cursorSprite;
    [SerializeField] private LevelTiles levelTiles;

    private bool _isEnabled;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _isEnabled = true;
    }

    private void Update()
    {
        if (!_isEnabled) return;

        var mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var tile = levelTiles.GetTileByWorldPosition(mouseWorldPos);

        // TODO check if the cursor is over a character. If it is, highlight the tile of the character, not the tile that is under the cursor
        // TODO check if cursor is over UI

        if (tile != null)
        {
            EnableGraphic(true);
            cursorSprite.transform.position = tile.worldPosition;
        }
        else
        {
            EnableGraphic(false);
        }
    }

    public void EnableCursor(bool enabled)
    {
        _isEnabled = enabled;
        EnableGraphic(enabled);
    }

    private void EnableGraphic(bool enabled)
    {
        if (cursorSprite.enabled != enabled)
        {
            cursorSprite.enabled = enabled;
        }
    }
}
