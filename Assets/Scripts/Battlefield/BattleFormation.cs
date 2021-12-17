using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace DiceyPokeProto {
    public class BattleFormation {
        public FormationType type;
        public ReactiveDictionary<Mon, int> mons = new ReactiveDictionary<Mon, int>();
        public List<GameObject> battleNodes = new List<GameObject>(); 

        public BattleFormation(FormationType type = null) {
            this.type = type ?? FormationTypes.QuintetFormation;
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
                if (mons[mon] == type.totalNodes - 1) {
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
                    mons[mon] = type.totalNodes - 1;
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
        public int maxMons => totalNodes / diffRotations;
        // the number of total nodes in this formation, containing all rotation types
        public int totalNodes;
        public int diffRotations;
    }

    public static class FormationTypes {
        public static FormationType DuoFormation = new FormationType {
            totalNodes = 4,
            diffRotations = 2,
        };
        public static FormationType TrioFormation = new FormationType {
            totalNodes = 6,
            diffRotations = 2,
        };
        public static FormationType QuartetFormation = new FormationType {
            totalNodes = 8,
            diffRotations = 2,
        };
        public static FormationType QuintetFormation = new FormationType {
            totalNodes = 10,
            diffRotations = 2,
        };
        
        public static List<FormationType> allTypes = new List<FormationType> {
            DuoFormation,
            TrioFormation,
            QuartetFormation,
            QuintetFormation,
        };
    }
}