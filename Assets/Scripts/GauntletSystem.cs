using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GauntletSystem : MonoBehaviour {
    public List<string> randomMonPool =
        new List<string>() {"Beldum", "Charmander", "Croagunk", "Sewaddle", "Shinx", "Tympole"};

    public GameObject battleSystemObj;
    public BattleSystem battleSystem;
    public List<MonEntity> playerParty = new List<MonEntity>();
    public List<MonEntity> enemyParty;

    public GameObject moveLearnSystemObj;
    public MoveLearnSystem moveLearnSystem;

    public GameObject chooseMonSystemObj;
    public ChooseMonSystem chooseMonSystem;

    public enum State {
        BATTLE,
        MOVE_LEARN,
        CHOOSE_MON
    }
    public State currentState;

    void Start() {
        battleSystem = battleSystemObj.GetComponent<BattleSystem>();
        moveLearnSystem = moveLearnSystemObj.GetComponent<MoveLearnSystem>();
        chooseMonSystem = chooseMonSystemObj.GetComponent<ChooseMonSystem>();

        enemyParty = new List<MonEntity>() { instantiateMon(randomMonPool.getRandomElement()) };
        playerParty.Concat(enemyParty).ToList().ForEach(mon => mon.constructBaseMoves());
        startChooseMon();
        currentState = State.CHOOSE_MON;
    }

    void Update() {
        if (chooseMonSystem.completed && currentState == State.CHOOSE_MON) {
            // barf
            chooseMonSystem.gameObject.SetActive(false);
            chooseMonSystem.rootUI.gameObject.SetActive(false);

            playerParty = chooseMonSystem.party;

            startBattle();
            currentState = State.BATTLE;
        }
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

            startChooseMon();
            currentState = State.CHOOSE_MON;
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

    public void startChooseMon() {
        Debug.Log("Calling startChooseMon() in GauntletSystem");
        chooseMonSystem.party = playerParty;
        chooseMonSystem.gameObject.SetActive(true);
        chooseMonSystem.rootUI.gameObject.SetActive(true);
        chooseMonSystem.startChooseMon();
    }

    public static MonEntity instantiateMon(string monName) {
        return Instantiate(Resources.Load<GameObject>($"MonPrefabs/{monName}")).GetComponent<MonEntity>();
    }

    IEnumerator WaitASecond() {
        yield return new WaitForSeconds(1f);
    }
}
