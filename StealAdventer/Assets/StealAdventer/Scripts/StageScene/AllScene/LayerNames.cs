using UnityEngine;
using System.Collections;

public class LayerNames : MonoBehaviour
{
	#region Character
	public static string Player = "Character(Player)";
	public static string MutekiPlayer = "Character(MutekiPlayer)";
	public static string NormalEnemy = "Character(Enemy)";
	public static string BossEnemy = "Character(Enemy)";
	#endregion

	#region Skill
	public static string PlayerSkill = "Skill(Player)";
	public static string EnemySkill = "Skill(Enemy)";
	#endregion

	#region Stage
	public static string Stage_FloorObject = "Stage(FloorObject)";
	public static string Stage_BossFloorObject = "Stage(BossFloorObject)";
    public static string Stage_WoodObject = "Stage(WoodObject)";
    public static string Stage_SlashableObject = "Stage(SlashableObject)";
    public static string Stage_BreakableObject = "Stage(BreakableObject)";
    public static string Stage_FlammableObject = "Stage(FlammableObject)";
	public static string Stage_TrapObject = "Stage(TrapObject)";
    #endregion
}
