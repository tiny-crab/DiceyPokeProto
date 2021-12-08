using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRulesMenu : MonoBehaviour
{
    // UI Elements
    public GameObject rootUI;
    public List<GameObject> ruleButtons;
    public List<GameObject> partySlots;


    // Underlying Model
    public List<MonEntity> party;
    public List<SpecialRule> offeredRules;

    // Start is called before the first frame update
    void Start() {
        rootUI = GameObject.Find("SpecialRulesMenu");
        ruleButtons = Enumerable.Range(1, 2)
            .Select(number => GameObject.Find($"SpecialRule{number}").gameObject)
            .ToList();
        partySlots = Enumerable.Range(1, 6)
            .Select(number => GameObject.Find($"Mon{number}").gameObject)
            .ToList();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
