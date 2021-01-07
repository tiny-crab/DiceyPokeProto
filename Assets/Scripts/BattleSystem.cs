using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleSystem : MonoBehaviour{
    public List<GameObject> partyObjects;
    public List<MonEntity> partyMons;
    public MonEntity activeMon;

    public List<GameObject> enemyPartyObjects;
    public List<MonEntity> enemyParty;
    public MonEntity enemyMon;

    BattleMenuUI battleMenu;

    void Start() {
        // UI setup
        battleMenu = new BattleMenuUI();
        battleMenu.populateMenu();
        partyMons = partyObjects.Select(monDef => monDef.GetComponent<MonEntity>()).ToList();
        partyMons.ForEach(mon => {
            mon.constructMoves();
            mon.currentHealth = mon.maxHealth;
        });
        battleMenu.updateLineup(partyMons);
        activeMon = partyMons.First();
        var randomMonPool = new List<string>() {"Beldum", "Charmander", "Croagunk", "Sewaddle", "Shinx", "Tympole"};
        enemyParty.Add(Resources
            .Load<GameObject>(
                $"MonPrefabs/{randomMonPool[new System.Random().Next(0, randomMonPool.Count)]}"
            )
            .GetComponent<MonEntity>()
        );
        enemyMon = enemyParty.First();

        // ready mons
        activeMon.currentEnergy = generateEnergy(activeMon);

        enemyMon.currentHealth = enemyMon.maxHealth;

        for (var i = 0; i < activeMon.activeMoves.Count; i++) {
            var copyvar = i; // needed to save i index in lambda delegation
            battleMenu.moveButtons[i].GetComponent<Button>()
                .onClick.AddListener(
                    delegate {doMove(activeMon, enemyMon, activeMon.activeMoves[copyvar]);}
                );

            var overloadGroup = battleMenu.overloadGroups.ToList()[copyvar];
            overloadGroup.Key.transform.Find("Value").GetComponent<Text>().text = "0";
            overloadGroup.Value[0].GetComponent<Button>().onClick.AddListener(
                delegate{battleMenu.updateOverloadValue(activeMon, overloadGroup.Key, true, copyvar);}
            );
            overloadGroup.Value[1].GetComponent<Button>().onClick.AddListener(
                delegate{battleMenu.updateOverloadValue(activeMon, overloadGroup.Key, false, copyvar);}
            );
        }

        for (var i = 0; i < partyMons.Count; i++) {
            var copyvar = i; // needed to save i index in lambda delegation
            battleMenu.partySlots[copyvar].GetComponent<Button>()
                .onClick.AddListener(
                    delegate {switchMon(partyMons[copyvar]);}
                );
        }
    }

    void Update() {
        battleMenu.updatePlayerMon(activeMon);
        battleMenu.updateEnemyMon(enemyMon);

        if (activeMon.remainingActions <= 0) {
            endPlayerTurn();
        }

        if (enemyMon.currentHealth <= 0) {
            Application.Quit();
        }
    }

    int generateEnergy(MonEntity mon) {
        var bottomBound = Mathf.FloorToInt(mon.maxEnergy / 2);
        return new System.Random().Next(bottomBound, mon.maxEnergy + 1);
    }

    void doMove(MonEntity attacker, MonEntity target, Move moveToExecute) {
        var moveIndex = attacker.activeMoves.IndexOf(moveToExecute);
        var overloadValue = int.Parse(battleMenu.overloadGroups.ToList()[moveIndex].Key.transform.Find("Value").GetComponent<Text>().text);
        target.currentHealth -= moveToExecute.damage + moveToExecute.overloadDamage * overloadValue;
        attacker.currentEnergy -= moveToExecute.cost;
        attacker.remainingActions -= 1;
    }

    void switchMon(MonEntity toSwitch) {
        if (activeMon != toSwitch) {
            activeMon = toSwitch;
            endPlayerTurn();
        }
    }

    void endPlayerTurn() {

        // do AI turn here

        // prep for player turn again
        activeMon.currentEnergy = generateEnergy(activeMon);
        battleMenu.overloadGroups.ToList().ForEach(pair => pair.Key.transform.Find("Value").GetComponent<Text>().text = "0");
        activeMon.remainingActions = 1;
    }

    public class BattleMenuUI {
        public GameObject parent;

        public GameObject playerMon;
        public List<GameObject> moveButtons;
        public Dictionary<GameObject, List<GameObject>> overloadGroups = new Dictionary<GameObject, List<GameObject>>();
        public GameObject lineup;
        public List<GameObject> partySlots;

        public GameObject enemyMon;

        public void populateMenu() {
            parent = GameObject.Find("BattleMenu");
            playerMon = GameObject.Find("PlayerMon").gameObject;
            enemyMon = GameObject.Find("EnemyMon").gameObject;
            moveButtons = Enumerable.Range(1, 4).Select(number => GameObject.Find($"Move{number}Button").gameObject).ToList();
            moveButtons.ForEach(moveButton => overloadGroups.Add(
                moveButton.transform.Find("Overload").gameObject,
                new List<GameObject>() {
                    moveButton.transform.Find("OverloadUp").gameObject,
                    moveButton.transform.Find("OverloadDown").gameObject
                }
            ));
            lineup = GameObject.Find("Lineup");
            partySlots = Enumerable.Range(1,6).Select(number =>
                lineup.transform.Find($"TeamMember{number}").gameObject
            ).ToList();
        }

        public void updatePlayerMon(MonEntity activeMon) {
            playerMon.GetComponent<Image>().sprite = Sprite.Create(
                activeMon.sprite,
                new Rect(0.0f, 0.0f, activeMon.sprite.width, activeMon.sprite.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );

            for (var i = 0; i < activeMon.activeMoves.Count; i++) {
                moveButtons[i].transform.Find("MoveName").GetComponent<Text>().text = activeMon.activeMoves[i].name;
                moveButtons[i].transform.Find("EnergyCost").GetComponent<Text>().text = $"Cost: {activeMon.activeMoves[i].cost}";
                moveButtons[i].SetActive(true);
            }

            for (var i = activeMon.activeMoves.Count; i < 4; i++) {
                moveButtons[i].SetActive(false);
            }

            var healthBar = playerMon.transform.Find("HealthBar").gameObject;
            var healthNumber = healthBar.transform.Find("NumberDisplay").gameObject;
            healthNumber.GetComponent<Text>().text = $"Health: {activeMon.currentHealth}";
            healthBar.GetComponent<Image>().fillAmount = activeMon.currentHealth / activeMon.maxHealth;

            var energyBar = playerMon.transform.Find("EnergyBar").gameObject;
            var energyNumber = energyBar.transform.Find("NumberDisplay").gameObject;
            energyNumber.GetComponent<Text>().text = $"Energy: {activeMon.currentEnergy}";
            energyBar.GetComponent<Image>().fillAmount = (float) activeMon.currentEnergy / (float) activeMon.maxEnergy;
        }

        public void updateEnemyMon(MonEntity enemy) {
            enemyMon.GetComponent<Image>().sprite = Sprite.Create(
                enemy.sprite,
                new Rect(0.0f, 0.0f, enemy.sprite.width, enemy.sprite.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );

            var healthBar = enemyMon.transform.Find("HealthBar").gameObject;
            var healthNumber = healthBar.transform.Find("NumberDisplay").gameObject;
            healthNumber.GetComponent<Text>().text = $"Health: {enemy.currentHealth}";
            healthBar.GetComponent<Image>().fillAmount = (float) enemy.currentHealth / (float) enemy.maxHealth;
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

        public void updateLineup(List<MonEntity> party) {
            for (int i = 0; i < party.Count; i++ ) {
                partySlots[i].GetComponent<Image>().sprite = Sprite.Create(
                    party[i].sprite,
                    new Rect(0.0f, 0.0f, party[i].sprite.width, party[i].sprite.height),
                    new Vector2(0.5f, 0.5f),
                    100.0f
                );
            }

            for (var i = party.Count; i < 6; i++) {
                partySlots[i].SetActive(false);
            }
        }
    }
}
