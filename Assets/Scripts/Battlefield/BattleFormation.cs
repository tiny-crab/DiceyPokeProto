using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace DiceyPokeProto {
    public class BattleFormation {
        public ReactiveProperty<FormationType> type = new ReactiveProperty<FormationType>(FormationTypes.DuoFormation);
        public ReactiveDictionary<Mon, int> mons = new ReactiveDictionary<Mon, int>();

        public GameObject gameObject;
        public List<GameObject> battleNodes = new List<GameObject>();

        public void AddToFormation(Mon mon) {
            switch (mons.Count + 1) {
                case 1:
                case 2:
                    type.Value = FormationTypes.DuoFormation;
                    break;
                case 3:
                    type.Value = FormationTypes.TrioFormation;
                    break;
                case 4:
                    type.Value = FormationTypes.QuartetFormation;
                    break;
                case 5:
                    type.Value = FormationTypes.QuintetFormation;
                    break;
            }

            if (mons.Count < 5) {
                mons[mon] = NextPlaceableBattleNodeIndex();
            }
        }

        public int NextPlaceableBattleNodeIndex() {
            var firstPosition = mons.Values.Count > 0 ? mons.Values.Min() : 0;
            var allPossibleFormRotations = Enumerable.Range(0, type.Value.diffRotations).Select(formationCoordsIndex => {
                return Enumerable.Range(0, type.Value.maxMons)
                    .Select(formationSpaceIndex => formationSpaceIndex * type.Value.diffRotations + formationCoordsIndex);
            });
            var currentFormRotation = allPossibleFormRotations.First(i => i.Contains(firstPosition));
            return currentFormRotation.First(i => !mons.Values.Contains(i));
        }

        public void RotateClockwise() {
            mons.Keys.ToList().ForEach(mon => {
                if (mons[mon] == type.Value.totalNodes - 1) {
                    mons[mon] = 0;
                }
                else {
                    mons[mon]++;
                }
            });
        }

        public void RotateCounterClockwise() {
            mons.Keys.ToList().ForEach(mon => {
                if (mons[mon] == 0) {
                    mons[mon] = type.Value.totalNodes - 1;
                }
                else {
                    mons[mon]--;
                }
            });
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