using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    // UI Elements
    public GameObject rootUI;

    void instantiateRootUI() {
        rootUI = GameObject.Find("BattleMenu");
    }

    public GameObject playerMonUIGroup;
        public List<GameObject> playerMoveButtons;
        public Dictionary<GameObject, List<GameObject>> playerMoveOverloadGroups = new Dictionary<GameObject, List<GameObject>>();

    void instantiatePlayerMonUI() {
        playerMonUIGroup = GameObject.Find("PlayerMon").gameObject;
        playerMoveButtons =
            Enumerable.Range(1, 4)
            .Select(number => GameObject.Find($"Move{number}Button").gameObject).ToList();
        playerMoveButtons.ForEach(moveButton => playerMoveOverloadGroups.Add(
            moveButton.transform.Find("Overload").gameObject,
            new List<GameObject>() {
                moveButton.transform.Find("OverloadUp").gameObject,
                moveButton.transform.Find("OverloadDown").gameObject
            }
        ));
    }

    public GameObject playerTeamLineup;
        public List<GameObject> playerPartySlots;

    void instantiateTeamLineup() {
        playerTeamLineup = GameObject.Find("Lineup");
        playerPartySlots = Enumerable.Range(1,6).Select(number =>
            playerTeamLineup.transform.Find($"TeamMember{number}").gameObject
        ).ToList();
    }

    public GameObject enemyMonUIGroup;

    void instantiateEnemyMonUI() {
        enemyMonUIGroup = GameObject.Find("EnemyMon").gameObject;
    }

    // Underlying Model
    public List<MonEntity> playerPartyMons;
    public MonEntity activePlayerMon;
    public MonEntity activeEnemyMon;

    void Start() {
        Debug.Log("Starting BattleMenu");
        instantiateRootUI();
        instantiatePlayerMonUI();
        instantiateEnemyMonUI();
        instantiateTeamLineup();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (activePlayerMon != null) { updatePlayerMon(); }
        if (activeEnemyMon != null) { updateEnemyMon(); }
        if (playerPartyMons != null) { updateLineup(); }
    }

    void updatePlayerMon() {
        playerMonUIGroup.GetComponent<Image>().sprite = Sprite.Create(
            activePlayerMon.sprite,
            new Rect(0.0f, 0.0f, activePlayerMon.sprite.width, activePlayerMon.sprite.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );

        for (var i = 0; i < activePlayerMon.activeMoves.Count; i++) {
            playerMoveButtons[i].transform.Find("MoveName").GetComponent<Text>().text = activePlayerMon.activeMoves[i].name;
            playerMoveButtons[i].transform.Find("EnergyCost").GetComponent<Text>().text = $"Cost: {activePlayerMon.activeMoves[i].cost}";
            playerMoveButtons[i].transform.Find("OverloadCost").Find("Value").GetComponent<Text>().text = $"{activePlayerMon.activeMoves[i].overloadCost}";
            playerMoveButtons[i].transform.Find("MoveDescToolTip").Find("Value").GetComponent<Text>().text = activePlayerMon.activeMoves[i].desc;
            playerMoveButtons[i].SetActive(true);
        }

        for (var i = activePlayerMon.activeMoves.Count; i < 4; i++) {
            playerMoveButtons[i].SetActive(false);
        }

        var healthBar = playerMonUIGroup.transform.Find("HealthBar").gameObject;
        var healthNumber = healthBar.transform.Find("NumberDisplay").gameObject;
        healthNumber.GetComponent<Text>().text = $"Health: {activePlayerMon.currentHealth}";
        healthBar.GetComponent<Image>().fillAmount = (float) activePlayerMon.currentHealth / (float) activePlayerMon.maxHealth;

        var energyBar = playerMonUIGroup.transform.Find("EnergyBar").gameObject;
        var energyNumber = energyBar.transform.Find("NumberDisplay").gameObject;
        energyNumber.GetComponent<Text>().text = $"Energy: {activePlayerMon.currentEnergy}";
        energyBar.GetComponent<Image>().fillAmount = (float) activePlayerMon.currentEnergy / (float) activePlayerMon.maxEnergy;

        var overloadBar = energyBar.transform.Find("OverloadBar").gameObject;
        if (activePlayerMon.currentEnergy > activePlayerMon.maxEnergy) {
            overloadBar.GetComponent<Image>().fillAmount =
                (float) (activePlayerMon.currentEnergy - activePlayerMon.maxEnergy) / (float) 100;
        } else {
            overloadBar.GetComponent<Image>().fillAmount = 0;
        }
    }

    void updateEnemyMon() {
        enemyMonUIGroup.GetComponent<Image>().sprite = Sprite.Create(
            activeEnemyMon.sprite,
            new Rect(0.0f, 0.0f, activeEnemyMon.sprite.width, activeEnemyMon.sprite.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );

        var healthBar = enemyMonUIGroup.transform.Find("HealthBar").gameObject;
        var healthNumber = healthBar.transform.Find("NumberDisplay").gameObject;
        healthNumber.GetComponent<Text>().text = $"Health: {activeEnemyMon.currentHealth}";
        healthBar.GetComponent<Image>().fillAmount = (float) activeEnemyMon.currentHealth / (float) activeEnemyMon.maxHealth;

        var energyBar = enemyMonUIGroup.transform.Find("EnergyBar").gameObject;
        var energyNumber = energyBar.transform.Find("NumberDisplay").gameObject;
        energyNumber.GetComponent<Text>().text = $"Energy: {activeEnemyMon.currentEnergy}";
        energyBar.GetComponent<Image>().fillAmount = (float) activeEnemyMon.currentEnergy / (float) activeEnemyMon.maxEnergy;

        var overloadBar = energyBar.transform.Find("OverloadBar").gameObject;
        if (activeEnemyMon.currentEnergy > activeEnemyMon.maxEnergy) {
            overloadBar.GetComponent<Image>().fillAmount =
                (float) (activeEnemyMon.currentEnergy - activeEnemyMon.maxEnergy) / (float) 100;
        } else {
            overloadBar.GetComponent<Image>().fillAmount = 0;
        }
    }

    void updateLineup() {
        for (int i = 0; i < playerPartyMons.Count; i++ ) {
            playerPartySlots[i].GetComponent<Image>().sprite = Sprite.Create(
                playerPartyMons[i].sprite,
                new Rect(0.0f, 0.0f, playerPartyMons[i].sprite.width, playerPartyMons[i].sprite.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );
        }
        for (var i = playerPartyMons.Count; i < 6; i++) {
            playerPartySlots[i].SetActive(false);
        }
    }
}
