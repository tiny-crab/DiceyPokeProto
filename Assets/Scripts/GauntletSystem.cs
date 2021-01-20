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

    public GameObject moveLearnSystemObj;
    public MoveLearnSystem moveLearnSystem;

    public enum State {
        BATTLE,
        MOVE_LEARN
    }
    public State currentState;

    void Start() {
        battleSystem = battleSystemObj.GetComponent<BattleSystem>();
        moveLearnSystem = moveLearnSystemObj.GetComponent<MoveLearnSystem>();

        playerParty = new List<string>() {"Charmander", "Sewaddle"}.Select(mon => instantiateMon(mon)).ToList();
        enemyParty = new List<MonEntity>() { instantiateMon(randomMonPool.getRandomElement()) };
        playerParty.Concat(enemyParty).ToList().ForEach(mon => mon.constructMoves());
        startBattle();
    }

    void Update() {
        if (battleSystem.battleComplete && currentState == State.BATTLE) {
            battleSystem.gameObject.SetActive(false);
            battleSystem.battleMenuRootUI.gameObject.SetActive(false);

            startMoveLearn();
            currentState = State.MOVE_LEARN;
        }
        if (moveLearnSystem.completed && currentState == State.MOVE_LEARN) {
            // barf
            moveLearnSystem.gameObject.SetActive(false);
            moveLearnSystem.rootUI.gameObject.SetActive(false);

            startBattle();
            currentState = State.BATTLE;
        }
    }

    public void startBattle() {
        Debug.Log("Calling startBattle() in GauntletSystem");
        battleSystem.partyMons = playerParty;

        battleSystem.enemyParty = enemyParty;

        battleSystem.gameObject.SetActive(true);
        battleSystem.battleMenuRootUI.gameObject.SetActive(true);
        battleSystem.startBattle();
    }

    public void startMoveLearn() {
        Debug.Log("Calling startMoveLearn() in GauntletSystem");
        moveLearnSystem.party = playerParty;
        moveLearnSystem.gameObject.SetActive(true);
        moveLearnSystem.rootUI.gameObject.SetActive(true);
        moveLearnSystem.startMoveLearn();
    }

    public static MonEntity instantiateMon(string monName) {
        return Instantiate(Resources.Load<GameObject>($"MonPrefabs/{monName}")).GetComponent<MonEntity>();
    }

    IEnumerator WaitASecond() {
        yield return new WaitForSeconds(1f);
    }
}
