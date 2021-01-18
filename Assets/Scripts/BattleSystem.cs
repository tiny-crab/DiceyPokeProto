using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleSystem : MonoBehaviour{
    public List<MonEntity> partyMons;
    public MonEntity activeMon;

    public List<MonEntity> enemyParty;
    public MonEntity enemyMon;

    public GameObject battleMenuRootUI;
    public BattleMenu battleMenu;

    public bool battleComplete = true;

    void Start() {
        Debug.Log("Starting BattleSystem");
        battleMenu = battleMenuRootUI.GetComponent<BattleMenu>();
        this.gameObject.SetActive(false);
    }

    void Update() {
        if (
            activeMon.remainingActions <= 0 ||
            activeMon.currentEnergy < activeMon.activeMoves.Select(move => move.cost).Min()
        ) {
            endPlayerTurn();
        }

        if (enemyMon.currentHealth <= 0) {
            // battleMenuRootUI.SetActive(false);
            battleComplete = true;
        }
    }

    public void startBattle() {
        Debug.Log("Calling startBattle() in BattleSystem");
        battleComplete = false;

        battleMenuRootUI.SetActive(true);

        partyMons.Concat(enemyParty).ToList().ForEach(mon => {
            mon.constructMoves();
            mon.currentHealth = mon.maxHealth;
        });
        battleMenu.playerPartyMons = partyMons;
        activeMon = partyMons.First();
        enemyMon = enemyParty.First();

        battleMenu.activePlayerMon = activeMon;
        battleMenu.activeEnemyMon = enemyMon;

        // ready mons
        activeMon.currentEnergy = activeMon.generateEnergy();
        enemyMon.currentEnergy = enemyMon.generateEnergy();

        for (var i = 0; i < activeMon.activeMoves.Count; i++) {
            var copyvar = i; // needed to save i index in lambda delegation
            battleMenu.playerMoveButtons[i].GetComponent<Button>()
                .onClick.AddListener(
                    delegate {doMove(activeMon, enemyMon, activeMon.activeMoves[copyvar]);}
                );

            var overloadGroup = battleMenu.playerMoveOverloadGroups.ToList()[copyvar];
            overloadGroup.Key.transform.Find("Value").GetComponent<Text>().text = "0";
            overloadGroup.Value[0].GetComponent<Button>().onClick.AddListener(
                delegate{updateOverloadValue(activeMon, overloadGroup.Key, true, copyvar);}
            );
            overloadGroup.Value[1].GetComponent<Button>().onClick.AddListener(
                delegate{updateOverloadValue(activeMon, overloadGroup.Key, false, copyvar);}
            );
        }

        for (var i = 0; i < partyMons.Count; i++) {
            var copyvar = i; // needed to save i index in lambda delegation
            battleMenu.playerPartySlots[copyvar].GetComponent<Button>()
                .onClick.AddListener(
                    delegate {switchMon(partyMons[copyvar]);}
                );
        }

        // battleMenu.updatePlayerMon(activeMon);
        // battleMenu.updateEnemyMon(enemyMon);
    }

    void doMove(MonEntity attacker, MonEntity target, Move moveToExecute) {
        if (moveToExecute.cost <= attacker.currentEnergy){
            var moveIndex = attacker.activeMoves.IndexOf(moveToExecute);
            var overloadValue = int.Parse(battleMenu.playerMoveOverloadGroups.ToList()[moveIndex].Key.transform.Find("Value").GetComponent<Text>().text);

            if (target.dodgeStack == 0) {
                target.currentHealth -= moveToExecute.damage + moveToExecute.overloadDamage * overloadValue;
                // TODO if a move does negative effects to target, and positive effects to an attacker simultaneously,
                // should the positive effects still be applied if the target dodges the attack?
                if (moveToExecute.extraEffects != null) {
                    moveToExecute.extraEffects(attacker, target, overloadValue);
                }
                // don't evolve move if it doesn't hit
                if (overloadValue >= moveToExecute.evolveThreshold && moveToExecute.evolvedMoveName != "") {
                    attacker.activeMoves[moveIndex] = new Move().getMoveByName(moveToExecute.evolvedMoveName);
                }
            } else {
                target.dodgeStack--;
            }

            attacker.currentEnergy -= moveToExecute.cost + (overloadValue * moveToExecute.overloadCost);
            attacker.remainingActions -= moveToExecute.actionCost;
        }
    }

    public void updateOverloadValue(MonEntity activeMon, GameObject overload, bool up, int moveIndex) {
            var overloadValueText = overload.transform.Find("Value").GetComponent<Text>();
            var overloadValueInt = int.Parse(overloadValueText.text);

            var selectedMove = activeMon.activeMoves[moveIndex];
            var moveOverloadCost = selectedMove.overloadCost;

            if (up && activeMon.currentEnergy >= moveOverloadCost * (overloadValueInt+1) + selectedMove.cost) { overloadValueInt++; }
            if (!up && overloadValueInt > 0) { overloadValueInt--; }

            overloadValueText.text = overloadValueInt.ToString();
        }

    void switchMon(MonEntity toSwitch) {
        if (activeMon != toSwitch) {
            activeMon = toSwitch;
            toSwitch.switchRefresh();
            endPlayerTurn();
        }
    }

    void endPlayerTurn() {
        // do AI turn here
        enemyMon.refreshTurn();

        // prep for player turn again
        battleMenu.playerMoveOverloadGroups.ToList().ForEach(pair => pair.Key.transform.Find("Value").GetComponent<Text>().text = "0");
        activeMon.refreshTurn();
    }
}
