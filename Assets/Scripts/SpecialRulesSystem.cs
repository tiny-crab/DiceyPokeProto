using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRulesSystem : MonoBehaviour
{
    public GameObject rootUI;
    public SpecialRulesMenu specialRulesMenu;

    public List<MonEntity> party;

    public List<SpecialRule> offeredRules;

    public bool completed = true;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("Starting SpecialRulesSystem");
        specialRulesMenu = rootUI.GetComponent<SpecialRulesMenu>();
        this.gameObject.SetActive(false);
    }

    public void startSpecialRules() {
        completed = false;
        rootUI.SetActive(true);

        offeredRules = new List<SpecialRule>() {SpecialRule.MaxedOutEnemy, SpecialRule.MaxedOutEnemy};

        specialRulesMenu.party = party;
        specialRulesMenu.offeredRules = offeredRules;

        // for (var i = 0; i < chooseMonMenu.monSelectButtons.Count; i++) {
        //     var copyvar = i; // needed to save i index in lambda delegation
        //     chooseMonMenu.monSelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
        //     chooseMonMenu.monSelectButtons[i].GetComponent<Button>().onClick
        //         .AddListener(
        //             delegate {selectMon(offeredMons[copyvar]);}
        //         );
        // }
    }
}
