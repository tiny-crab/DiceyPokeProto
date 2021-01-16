using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GauntletSystem : MonoBehaviour {
    public List<string> randomMonPool =
        new List<string>() {"Beldum", "Charmander", "Croagunk", "Sewaddle", "Shinx", "Tympole"};

    public GameObject battleSystemObj;
    public BattleSystem battleSystem;
    public List<MonEntity> playerParty;
    public List<MonEntity> enemyParty;

    void Start() {
        battleSystem = battleSystemObj.GetComponent<BattleSystem>();
        Debug.Log("Starting GauntletSystem");
    }

    void Update() {
        if (battleSystem.battleComplete) {
            battleSystem.gameObject.SetActive(false);
            // go to next state
            startBattle();
            battleSystem.gameObject.SetActive(true);
        }
    }

    public void startBattle() {
        Debug.Log("Calling startBattle() in GauntletSystem");
        playerParty = randomMonPool.getManyRandomElements(2).Select(mon => instantiateMon(mon)).ToList();
        battleSystem.partyMons = playerParty;

        enemyParty = new List<MonEntity>() { instantiateMon(randomMonPool.getRandomElement()) };
        battleSystem.enemyParty = enemyParty;

        battleSystem.startBattle();
    }

    public static MonEntity instantiateMon(string monName) {
        return Instantiate(Resources.Load<GameObject>($"MonPrefabs/{monName}")).GetComponent<MonEntity>();
    }

    IEnumerator WaitASecond() {
        yield return new WaitForSeconds(1f);
    }
}
