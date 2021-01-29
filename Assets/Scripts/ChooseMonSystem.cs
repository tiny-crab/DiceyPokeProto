using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMonSystem : MonoBehaviour
{

    public GameObject rootUI;
    public ChooseMonMenu chooseMonMenu;

    public bool completed = true;

    public List<MonEntity> party;
    public List<GameObject> monPool;
    public List<MonEntity> offeredMons;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("Starting ChooseMonSystem");
        chooseMonMenu = rootUI.GetComponent<ChooseMonMenu>();

        // monPool = Resources.LoadAll("MonPrefabs").ToList()
        //     .Select(obj => (GameObject) obj)
        //     .Where(obj => obj.name != "PrototypeMon").ToList();
        monPool = new List<string>() {"Beldum", "Charmander", "Croagunk", "Sewaddle", "Shinx", "Tympole"}.ToList()
            .Select(name => (GameObject) Resources.Load($"MonPrefabs/{name}")).ToList();
        this.gameObject.SetActive(false);
    }

    public bool readyToStart() {
        return party.Count < 6;
    }

    public void startChooseMon() {
        completed = false;
        rootUI.SetActive(true);

        offeredMons = monPool
            .getManyRandomElements(chooseMonMenu.monSelectButtons.Count)
            .Select(obj => {
                var mon = Instantiate(obj).GetComponent<MonEntity>();
                mon.constructBaseMoves();
                return mon;
            }).ToList();

        chooseMonMenu.selectableMons = offeredMons;

        for (var i = 0; i < chooseMonMenu.monSelectButtons.Count; i++) {
            var copyvar = i; // needed to save i index in lambda delegation
            chooseMonMenu.monSelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            chooseMonMenu.monSelectButtons[i].GetComponent<Button>().onClick
                .AddListener(
                    delegate {selectMon(offeredMons[copyvar]);}
                );
        }
    }

    void selectMon(MonEntity mon) {
        party.Add(mon);
        completed = true;
    }
}
