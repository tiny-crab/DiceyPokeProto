using UniRx;
using System.Linq;
using UnityEngine;

namespace DiceyPokeProto {
    public class Battlefield : MonoBehaviour {
        private Prefabs _prefabs;
        private Datastore _datastore;

        private GameObject battlefield;
        private GameObject leftFormation;

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
            leftFormation = battlefield.transform.Find("Left").gameObject;
            _datastore.leftFormation.battleNodes = Enumerable.Range(0, 6)
                .Select(i => leftFormation.transform.Find($"BattleNode{i}").gameObject).ToList();

            MockStartBattle();
        }

        public void MockStartBattle() {
            _datastore.inputEvents.Receive<KeyEvent>()
                .Where(e => e.keyCode == KeyCode.N)
                .Subscribe(_ => {
                    var charmander = new Mon {
                        name = "Ash",
                        prefab = _prefabs.charmander,
                        instance = Instantiate(_prefabs.charmander)
                    };
                    _datastore.activeTeam.Add(charmander);
                    _datastore.leftFormation.AddToFormation(charmander);
                });
        }
    }
}