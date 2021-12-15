using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace DiceyPokeProto {
    public class BattleFormation {
        public FormationType type = FormationTypes.TripleFormation;
        public IntReactiveProperty currentFormationIndex = new IntReactiveProperty(0);
        public List<int> currentFormationCoords;
        public ReactiveDictionary<Mon, int> mons = new ReactiveDictionary<Mon, int>();
        public List<GameObject> battleNodes = new List<GameObject>(); 

        public BattleFormation(FormationType type = null) {
            type ??= FormationTypes.TripleFormation;
            currentFormationIndex.AsObservable()
                .Subscribe(newIndex => currentFormationCoords = type.formationCoords[newIndex]);
            currentFormationCoords = type.formationCoords[currentFormationIndex.Value];
        }

        public void AddToFormation(Mon mon) {
            if (mons.Count < type.maxMons) {
                mons[mon] = currentFormationCoords.First(i => !mons.Values.Contains(i));
            }
        }

        public void RotateClockwise() {
            if (currentFormationIndex.Value == type.formationCoords.Count - 1) {
                currentFormationIndex.Value = 0;
            }
            else {
                currentFormationIndex.Value++;
            }
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
            if (currentFormationIndex.Value == 0) {
                currentFormationIndex.Value = type.formationCoords.Count - 1;
            }
            else {
                currentFormationIndex.Value--;
            }
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
                .ForEach(kvp => kvp.Key.instance.transform.position = battleNodes[kvp.Value].transform.position);
        }
    }

    public class FormationType {
        public int maxMons;
        public List<List<int>> formationCoords;
    }

    public static class FormationTypes {
        public static FormationType TripleFormation = new FormationType {
            maxMons = 3,
            formationCoords = new List<List<int>> {
                new List<int> {0, 2, 4},
                new List<int> {1, 3, 5},
            }
        };
    }
}