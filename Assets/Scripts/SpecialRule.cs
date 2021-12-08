public class SpecialRule {
    public string name;
    public string desc;
    public int playerTeamSize;
    public int enemyTeamSize;

    public SpecialRule(
        string name="",
        string desc="",
        int playerTeamSize=3,
        int enemyTeamSize=3
    ) {
        this.name = name;
        this.desc = desc;
        this.playerTeamSize = playerTeamSize;
        this.enemyTeamSize = enemyTeamSize;
    }

    public static SpecialRule MaxedOutEnemy = new SpecialRule(
        name:"Maxed Out Enemy",
        desc:"Opponent Pokemon has max energy on every turn.",
        playerTeamSize: 4
    );
}