using UniRx;
using System.Linq;
using UnityEngine;

namespace DiceyPokeProto {
    public class Battlefield : MonoBehaviour {
        private Prefabs _prefabs;
        private Datastore _datastore;

        private GameObject battlefield;
        private GameObject leftFormation;
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
                });

            battlefield = GameObject.Find("Battlefield");
            leftFormation = Instantiate(_prefabs.quintetFormation, battlefield.transform);
            leftFormation.name = "Left";
            _datastore.leftFormation.battleNodes = Enumerable.Range(0, FormationTypes.QuintetFormation.totalNodes)
                .Select(i => leftFormation.transform.Find($"BattleNode{i}").gameObject).ToList();
            _datastore.leftFormation.type = FormationTypes.QuintetFormation;
            spawnArea = battlefield.transform.Find("SpawnArea").gameObject;
                
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
                    /// bad bad bad bad bad dont do this
                    _datastore.leftFormation.UpdateMonPosition();
                });
        }
    }
}