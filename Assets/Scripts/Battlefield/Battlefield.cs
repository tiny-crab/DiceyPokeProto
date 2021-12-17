using UniRx;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace DiceyPokeProto {
    public class Battlefield : MonoBehaviour {
        private Prefabs _prefabs;
        private Datastore _datastore;

        private GameObject battlefield;
        private GameObject spawnArea;

        public void Start() {
            _prefabs = GetComponent<Prefabs>();
            _datastore = GetComponent<Datastore>();

            _datastore.inputEvents.Receive<KeyEvent>()
                .Where(e => e.keyCode == KeyCode.R || e.keyCode == KeyCode.E)
                .Subscribe(e => {
                    if (e.keyCode == KeyCode.R) {
                        _datastore.leftFormation.RotateClockwise(); 
                    }
                    else {
                        _datastore.leftFormation.RotateCounterClockwise();
                    }
                    _datastore.battlefieldEvents.Publish(new RotateFormationEvent {
                        formation = _datastore.leftFormation
                    });
                    UpdateMonPositions();
                });

            battlefield = GameObject.Find("Battlefield");
            spawnArea = battlefield.transform.Find("SpawnArea").gameObject;
            
            _datastore.leftFormation = new BattleFormation();
            InstantiateFormation(_prefabs.duoFormation, FormationTypes.DuoFormation);
            _datastore.leftFormation.type.AsObservable().Subscribe(newType => {
                if (newType == FormationTypes.DuoFormation) {
                    InstantiateFormation(_prefabs.duoFormation, newType);    
                } else if (newType == FormationTypes.TrioFormation) {
                    InstantiateFormation(_prefabs.trioFormation, newType);    
                } else if (newType == FormationTypes.QuartetFormation) {
                    InstantiateFormation(_prefabs.quartetFormation, newType);    
                } else if (newType == FormationTypes.QuintetFormation) {
                    InstantiateFormation(_prefabs.quintetFormation, newType);    
                }
            });
            
            _datastore.rightFormation = new BattleFormation();
            MockInstantiateOpposingFormation(_prefabs.trioFormation, FormationTypes.TrioFormation);
            
            MockStartBattle();
        }

        public void MockStartBattle() {
            _datastore.inputEvents.Receive<KeyEvent>()
                .Where(e => e.keyCode == KeyCode.N)
                .Subscribe(_ => {
                    switch (_datastore.activeTeam.Count) {
                        case 0:
                            var charmander = new Mon {
                                name = "Ash",
                                prefab = _prefabs.charmander,
                                instance = Instantiate(
                                    _prefabs.charmander,
                                    spawnArea.transform.position,
                                    Quaternion.identity)
                            };
                            _datastore.activeTeam.Add(charmander);
                            _datastore.leftFormation.AddToFormation(charmander);
                            break;
                        case 1:
                            var beldum = new Mon {
                                name = "Rivet",
                                prefab = _prefabs.beldum,
                                instance = Instantiate(
                                    _prefabs.beldum, 
                                    spawnArea.transform.position,
                                    Quaternion.identity)
                            };
                            _datastore.activeTeam.Add(beldum);
                            _datastore.leftFormation.AddToFormation(beldum);
                            break;
                        case 2:
                            var shinx = new Mon {
                                name = "Bolt",
                                prefab = _prefabs.shinx,
                                instance = Instantiate(
                                    _prefabs.shinx,
                                    spawnArea.transform.position, 
                                    Quaternion.identity)
                            };
                            _datastore.activeTeam.Add(shinx);
                            _datastore.leftFormation.AddToFormation(shinx);
                            break;
                        case 3:
                            var sewaddle = new Mon {
                                name = "Gucci",
                                prefab = _prefabs.sewaddle,
                                instance = Instantiate(
                                    _prefabs.sewaddle,
                                    spawnArea.transform.position, 
                                    Quaternion.identity)
                            };
                            _datastore.activeTeam.Add(sewaddle);
                            _datastore.leftFormation.AddToFormation(sewaddle);
                            break;
                        case 4:
                            var tympole = new Mon {
                                name = "Greg",
                                prefab = _prefabs.tympole,
                                instance = Instantiate(
                                    _prefabs.tympole,
                                    spawnArea.transform.position, 
                                    Quaternion.identity)
                            };
                            _datastore.activeTeam.Add(tympole);
                            _datastore.leftFormation.AddToFormation(tympole);
                            break;
                    }
                    UpdateMonPositions();
                });
        }

        private void InstantiateFormation(GameObject formationPrefab, FormationType type) {
            Destroy(_datastore.leftFormation.gameObject);
            _datastore.leftFormation.gameObject = Instantiate(formationPrefab, battlefield.transform);
            _datastore.leftFormation.gameObject.name = "Left";

            _datastore.leftFormation.battleNodes = 
                Enumerable.Range(0, type.totalNodes)
                    .Select(i => _datastore.leftFormation.gameObject.transform.Find($"BattleNode{i}").gameObject)
                    .ToList();
        }

        private void MockInstantiateOpposingFormation(GameObject formationPrefab, FormationType type) {
            _datastore.rightFormation.gameObject = Instantiate(formationPrefab, battlefield.transform);
            _datastore.rightFormation.gameObject.name = "Right";

            _datastore.rightFormation.battleNodes = 
                Enumerable.Range(0, type.totalNodes)
                    .Select(i => _datastore.rightFormation.gameObject.transform.Find($"BattleNode{i}").gameObject)
                    .ToList();
            
            _datastore.rightFormation.battleNodes.ForEach(node => {
                node.transform.position = Vector3.Scale(node.transform.position, new Vector3(-1, 1, 1));
            });
            
        }

        private void UpdateMonPositions() {
            _datastore.leftFormation.mons.ToList()
                .ForEach(kvp => 
                    kvp.Key.instance.transform.DOMove(
                        _datastore.leftFormation.battleNodes[kvp.Value].transform.position,
                        0.3f
                    )
                );
        }
    }
}