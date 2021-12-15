using UnityEngine;

namespace DiceyPokeProto {
    public class God : MonoBehaviour {
        void Awake() {
            gameObject.AddComponent<MouseAndKeyboard>();
            gameObject.AddComponent<Datastore>();
            gameObject.AddComponent<Prefabs>();
            gameObject.AddComponent<Battlefield>();
        }
    }
    
}
