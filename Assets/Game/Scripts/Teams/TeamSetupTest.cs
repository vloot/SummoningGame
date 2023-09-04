using System.Threading.Tasks;
using UnityEngine;

public class TeamSetupTest : MonoBehaviour
{
    [Header("Characters")]
    public BaseCharacter character1;
    public BaseCharacter character2;
    public BaseCharacter character3;

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
        character1.CreateCharacter(lt.GetTile(new Vector3Int(-4, -2, 0)), t1);
        character3.CreateCharacter(lt.GetTile(new Vector3Int(-2, -2, 0)), t1);
        character2.CreateCharacter(lt.GetTile(new Vector3Int(1, -1, 0)), t2);

        // Register characters
        t1.AddCharacter(character1);
        t1.AddCharacter(character3);
        t2.AddCharacter(character2);
    }
}
