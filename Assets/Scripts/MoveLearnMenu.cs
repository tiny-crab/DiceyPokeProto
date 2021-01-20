using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLearnMenu : MonoBehaviour
{
    // UI Elements
    public GameObject rootUI;

    public GameObject monSprite;
    public List<GameObject> moveLearnButtons;

    // Underlying Model
    public MonEntity activeMon;
    public List<Move> offeredMoves;

    void Start() {
        rootUI = GameObject.Find("MoveLearnMenu");
        monSprite = rootUI.transform.Find("MonSprite").gameObject;
        moveLearnButtons =
            Enumerable.Range(1, 2)
            .Select(number => GameObject.Find($"MoveLearnButton{number}").gameObject)
            .ToList();

        this.gameObject.SetActive(false);
    }

    void Update() {
        monSprite.GetComponent<Image>().sprite = Sprite.Create(
                activeMon.sprite,
                new Rect(0.0f, 0.0f, activeMon.sprite.width, activeMon.sprite.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );
        for (int i = 0; i < moveLearnButtons.Count; i++) {
            moveLearnButtons[i].transform.Find("MoveName").GetComponent<Text>().text = offeredMoves[i].name;
            moveLearnButtons[i].transform.Find("EnergyCost").GetComponent<Text>().text = $"Cost: {offeredMoves[i].cost}";
            moveLearnButtons[i].transform.Find("OverloadCost").Find("Value").GetComponent<Text>().text = $"{offeredMoves[i].overloadCost}";
            moveLearnButtons[i].transform.Find("MoveDescToolTip").Find("Value").GetComponent<Text>().text = offeredMoves[i].desc;
            var moveParent = new Move().getMoveParent(offeredMoves[i].name);
            var prevolution = moveLearnButtons[i].transform.Find($"MoveLearnPrevolution");
            prevolution.gameObject.SetActive(true);
            if (moveParent != null) {
                prevolution.transform.Find("MoveName").GetComponent<Text>().text = moveParent.name;
                prevolution.transform.Find("EnergyCost").GetComponent<Text>().text = $"Cost: {moveParent.cost}";
                prevolution.transform.Find("OverloadCost").Find("Value").GetComponent<Text>().text = $"{moveParent.overloadCost}";
            } else {
                prevolution.gameObject.SetActive(false);
            }
        }

    }
}
