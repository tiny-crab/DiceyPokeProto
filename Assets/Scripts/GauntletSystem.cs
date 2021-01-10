using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GauntletSystem : MonoBehaviour {
    public List<string> randomMonPool =
        new List<string>() {"Beldum", "Charmander", "Croagunk", "Sewaddle", "Shinx", "Tympole"};

    public BattleSystem currentBattle;
    public List<MonEntity> playerParty;
    public List<MonEntity> enemyParty;

    // Start is called before the first frame update
    void Start() {
        currentBattle = Instantiate(
            (GameObject) Resources.Load("ScriptObjects/BattleSystem")
        ).GetComponent<BattleSystem>();

        playerParty = new List<MonEntity>() { instantiateMon(randomMonPool.getRandomElement()) };
        currentBattle.partyMons = playerParty;

        enemyParty = new List<MonEntity>() { instantiateMon(randomMonPool.getRandomElement()) };
        currentBattle.enemyParty = enemyParty;

    }

    // Update is called once per frame
    void Update() {

    }

    public static MonEntity instantiateMon(string monName) {
        return Instantiate(Resources.Load<GameObject>($"MonPrefabs/{monName}")).GetComponent<MonEntity>();
    }
}
