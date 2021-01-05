using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleSystem : MonoBehaviour
{
    public List<GameObject> partyObjects;
    public List<MonEntity> partyMons;
    public MonEntity activeMon;

    BattleMenuUI battleMenu;


    // Start is called before the first frame update
    void Start() {
        // UI setup
        battleMenu = new BattleMenuUI();
        battleMenu.populateMenu();
        partyMons = partyObjects.Select(monDef => monDef.GetComponent<MonEntity>()).ToList();
        partyMons.ForEach(mon => mon.constructMoves());
        activeMon = partyMons.First();
        battleMenu.updatePlayerMon(activeMon);
    }

    // Update is called once per frame
    void Update() {

    }

    public class BattleMenuUI {
        public GameObject parent;

        public GameObject playerMon;
        public List<GameObject> moveButtons;

        public void populateMenu() {
            parent = GameObject.Find("BattleMenu");
            playerMon = GameObject.Find("PlayerMon").gameObject;
            moveButtons = Enumerable.Range(1, 4).Select(number => GameObject.Find($"Move{number}Button").gameObject).ToList();
        }

        public void updatePlayerMon(MonEntity activeMon) {
            playerMon.GetComponent<Image>().sprite = Sprite.Create(
                activeMon.sprite,
                new Rect(0.0f, 0.0f, activeMon.sprite.width, activeMon.sprite.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );

            for (var i = 0; i < activeMon.activeMoves.Count; i++) {
                moveButtons[i].transform.Find("MoveName").GetComponent<Text>().text = activeMon.activeMoves[i].moveName;
                moveButtons[i].transform.Find("EnergyCost").GetComponent<Text>().text = $"Cost: {activeMon.activeMoves[i].cost}";
            }

        }
    }
}
