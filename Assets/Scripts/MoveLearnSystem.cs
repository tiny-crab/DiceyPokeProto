using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLearnSystem : MonoBehaviour
{

    public GameObject rootUI;

    public MoveLearnMenu moveLearnMenu;

    public List<MonEntity> party;
    public List<MonEntity> remaining;
    public MonEntity activeMon;
    public List<Move> movePool;
    public List<Move> offeredMoves;

    public bool completed = true;

    void Start() {
        Debug.Log("Starting MoveLearnSystem");
        moveLearnMenu = rootUI.GetComponent<MoveLearnMenu>();
        this.gameObject.SetActive(false);
    }

    public void startMoveLearn() {
        completed = false;
        rootUI.SetActive(true);

        offerMoves();

        if (party != null) {
            remaining = party;

            for (var i = 0; i < moveLearnMenu.moveLearnButtons.Count; i++) {
                var copyvar = i; // needed to save i index in lambda delegation
                moveLearnMenu.moveLearnButtons[i].GetComponent<Button>()
                    .onClick
                    .AddListener(
                        delegate {learnMove(activeMon, offeredMoves[copyvar]);}
                    );
                moveLearnMenu.moveLearnButtons[i].GetComponent<Button>()
                    .onClick
                    .AddListener(
                        delegate {offerMoves();}
                    );
            }
        }
    }

    public void offerMoves() {
        activeMon = party.First();
        moveLearnMenu.activeMon = activeMon;
        movePool = activeMon.learnableMoves
            .Where(moveConfig => moveConfig.Key == activeMon.currentLevel)
            .Select(pair => pair.Value).ToList();
        offeredMoves = movePool.getManyRandomElements(2);
        offeredMoves.ForEach(offeredMove => movePool.Remove(offeredMove));
        moveLearnMenu.offeredMoves = offeredMoves;
        Debug.Log($"Offering {offeredMoves[0].name} and {offeredMoves[1].name}");
    }

    public void learnMove(MonEntity activeMon, Move move) {
        activeMon.activeMoves.Add(move);
    }

    void Update() {
        if (remaining.Count == 0) {
            completed = true;
        }
    }
}
