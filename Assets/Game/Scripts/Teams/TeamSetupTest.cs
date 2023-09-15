using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class TeamSetupTest : MonoBehaviour
{
    [Header("Characters")]
    public BaseCharacter character1;
    public Vector3Int character1StartPosition;

    public BaseCharacter character2;
    public Vector3Int character2StartPosition;

    public BaseCharacter character3;
    public Vector3Int character3StartPosition;

    [Header("AI")]
    [SerializeField] private AIController aiController;

    [Header("Team stuff")]
    public TeamController teamController;

    private void Start()
    {
        var lt = FindObjectOfType<LevelTiles>();

        // Create teams
        var t1 = new Team(TeamSide.Player, true);
        var t2 = new Team(TeamSide.Enemy, false);

        // Register teams
        teamController.AddTeam(t1);
        teamController.AddTeam(t2);

        // Create characters
        character1.CreateCharacter(lt.GetTile(character1StartPosition), t1);
        character2.CreateCharacter(lt.GetTile(character2StartPosition), t2);
        character3.CreateCharacter(lt.GetTile(character3StartPosition), t1);

        // Register characters
        t1.AddCharacter(character1);
        t1.AddCharacter(character3);
        t2.AddCharacter(character2);

        teamController.ProcessMove();

        // AI setup
        aiController.InitAI(t2);
    }
}
