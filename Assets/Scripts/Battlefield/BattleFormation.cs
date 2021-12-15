using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace DiceyPokeProto {
    public class BattleFormation {
        public FormationType type = FormationTypes.TripleFormation;
        public ReactiveDictionary<Mon, int> mons = new ReactiveDictionary<Mon, int>();
        public List<GameObject> battleNodes = new List<GameObject>(); 

        public BattleFormation(FormationType type = null) {
            this.type = type ?? FormationTypes.TripleFormation;
        }

        public void AddToFormation(Mon mon) {
            if (mons.Count < type.maxMons) {
                var firstPosition = mons.Values.Count > 0 ? mons.Values.Min() : 0;
                var allPossibleFormRotations = Enumerable.Range(0, type.diffRotations).Select(formationCoordsIndex => {
                    return Enumerable.Range(0, type.maxMons)
                        .Select(formationSpaceIndex => formationSpaceIndex * type.diffRotations + formationCoordsIndex);
                });
                var currentFormRotation = allPossibleFormRotations.First(i => i.Contains(firstPosition));
                mons[mon] = currentFormRotation.First(i => !mons.Values.Contains(i));
            }
        }

        public void RotateClockwise() {
            mons.Keys.ToList().ForEach(mon => {
                if (mons[mon] == 5) {
                    mons[mon] = 0;
                }
                else {
                    mons[mon]++;
                }
            });
            UpdateMonPosition();
        }

        public void RotateCounterClockwise() {
            mons.Keys.ToList().ForEach(mon => {
                if (mons[mon] == 0) {
                    mons[mon] = 5;
                }
                else {
                    mons[mon]--;
                }
            });
            UpdateMonPosition();
        }

        public void UpdateMonPosition() {
            mons.ToList()
                .ForEach(kvp => kvp.Key.instance.transform.DOMove(battleNodes[kvp.Value].transform.position, 0.3f));
        }
    }

    public class FormationType {
        // the number of mons that can make up this formation
        public int maxMons {
            get { return totalNodes / diffRotations; }
        }
        // the number of total nodes in this formation, containing all rotation types
        public int totalNodes;
        public int diffRotations;
    }

    public static class FormationTypes {
        public static FormationType TripleFormation = new FormationType {
            totalNodes = 6,
            diffRotations = 2,
        };
    }
}