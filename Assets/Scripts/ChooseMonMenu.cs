using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMonMenu : MonoBehaviour
{

    // UI Elements
    public GameObject rootUI;

    public List<GameObject> monSelectButtons;

    // Underlying Model
    public List<MonEntity> selectableMons;

    void Start() {
        rootUI = GameObject.Find("ChooseMonMenu");
        monSelectButtons =
            Enumerable.Range(1, 2)
            .Select(number => rootUI.transform.Find($"Mon{number}Button").gameObject)
            .ToList();

        this.gameObject.SetActive(false);
    }

    void Update() {
        for (int i = 0; i < monSelectButtons.Count; i++) {
            monSelectButtons[i].transform.Find("MonName").GetComponent<Text>().text = selectableMons[i].monName;
            monSelectButtons[i].transform.Find("MonSprite").GetComponent<Image>().sprite = Sprite.Create(
                selectableMons[i].sprite,
                new Rect(0.0f, 0.0f, selectableMons[i].sprite.width, selectableMons[i].sprite.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );
        }
    }
}
