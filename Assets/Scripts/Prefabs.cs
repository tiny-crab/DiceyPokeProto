using System;
using UnityEngine;

namespace DiceyPokeProto {
    public class Prefabs : MonoBehaviour {
        
        // .88b  d88.  .d88b.  d8b   db .d8888. 
        // 88'YbdP`88 .8P  Y8. 888o  88 88'  YP 
        // 88  88  88 88    88 88V8o 88 `8bo.   
        // 88  88  88 88    88 88 V8o88   `Y8b. 
        // 88  88  88 `8b  d8' 88  V888 db   8D 
        // YP  YP  YP  `Y88P'  VP   V8P `8888Y' 
        public GameObject charmander;

        public void Awake() {
            charmander = Resources.Load<GameObject>("MonPrefabs/Charmander");
        }
    }
}