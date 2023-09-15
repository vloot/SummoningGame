public class AICharacterMove
{
    public BaseCharacter character;
    public Tile tile;

    public bool canMove;

    public AICharacterMove()
    {
        canMove = false;
    }

    public AICharacterMove(BaseCharacter character, Tile tile)
    {
        this.character = character;
        this.tile = tile;
        canMove = true;
    }
}
