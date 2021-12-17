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
        public GameObject shinx;
        public GameObject beldum;
        public GameObject sewaddle;
        public GameObject tympole;
        
        
        // d88888b  .d88b.  d8888b. .88b  d88.  .d8b.  d888888b d888888b  .d88b.  d8b   db .d8888. 
        // 88'     .8P  Y8. 88  `8D 88'YbdP`88 d8' `8b `~~88~~'   `88'   .8P  Y8. 888o  88 88'  YP 
        // 88ooo   88    88 88oobY' 88  88  88 88ooo88    88       88    88    88 88V8o 88 `8bo.   
        // 88~~~   88    88 88`8b   88  88  88 88~~~88    88       88    88    88 88 V8o88   `Y8b. 
        // 88      `8b  d8' 88 `88. 88  88  88 88   88    88      .88.   `8b  d8' 88  V888 db   8D 
        // YP       `Y88P'  88   YD YP  YP  YP YP   YP    YP    Y888888P  `Y88P'  VP   V8P `8888Y'
        public GameObject duoFormation;
        public GameObject trioFormation;
        public GameObject quartetFormation;
        public GameObject quintetFormation;

        public void Awake() {
            charmander = Resources.Load<GameObject>("MonPrefabs/Charmander");
            shinx = Resources.Load<GameObject>("MonPrefabs/Shinx");
            beldum = Resources.Load<GameObject>("MonPrefabs/Beldum");
            sewaddle = Resources.Load<GameObject>("MonPrefabs/Sewaddle");
            tympole = Resources.Load<GameObject>("MonPrefabs/Tympole");

            duoFormation = Resources.Load<GameObject>("GamePrefabs/DuoFormation");
            trioFormation = Resources.Load<GameObject>("GamePrefabs/TrioFormation");
            quartetFormation = Resources.Load<GameObject>("GamePrefabs/QuartetFormation");
            quintetFormation = Resources.Load<GameObject>("GamePrefabs/QuintetFormation");
        }
    }
}