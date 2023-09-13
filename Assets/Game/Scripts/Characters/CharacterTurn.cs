public struct CharacterTurn
{
    public BaseCharacter character;

    public CharacterAction characterAction;
    private bool _isCharacterSelected;

    private bool _isTurnOver;

    public bool Moved { get; private set; }
    public bool Attacked { get; private set; }
    public bool CastedSpell { get; private set; }

    public CharacterTurn(BaseCharacter character)
    {
        this.character = character;
        characterAction = CharacterAction.None;
        Moved = false;
        Attacked = false;
        CastedSpell = false;
        _isCharacterSelected = true;
        _isTurnOver = false;
    }

    public void EndTurn()
    {
        _isTurnOver = true;
        character.EndTurn();
    }

    public bool IsOver()
    {
        if (!_isTurnOver)
        {
            // check if turn is over
            _isTurnOver = Attacked || CastedSpell;
            _isTurnOver = Moved; // TODO this line is for testing, remove later!!
        }

        return _isTurnOver;
    }

    public bool IsCharacterSelected()
    {
        return _isCharacterSelected && !_isTurnOver;
    }

    public void SelectAction(CharacterAction action)
    {
        // TODO add additional check here to see if the action can be selected
        characterAction = action;
    }

    public void CompleteAction()
    {
        switch (characterAction)
        {
            case CharacterAction.Move:
                Moved = true;
                break;
            case CharacterAction.Attack:
                Attacked = true;
                break;
            case CharacterAction.UseSpell:
                CastedSpell = true;
                break;
            default:
                return;
        }

        characterAction = CharacterAction.None;
    }
}