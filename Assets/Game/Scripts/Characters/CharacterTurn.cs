public struct CharacterTurn
{
    public bool moved;
    public bool attacked;
    public bool usedSpell;

    private bool isTurnOver;

    public bool IsTurnOver()
    {
        isTurnOver = attacked || usedSpell;
        return isTurnOver;
    }

    public void EndTurn()
    {
        isTurnOver = true;
    }
}
