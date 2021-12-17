using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace DiceyPokeProto {
    public class Datastore : MonoBehaviour {
        
        // .88b  d88.  .d88b.  d8b   db .d8888. 
        // 88'YbdP`88 .8P  Y8. 888o  88 88'  YP 
        // 88  88  88 88    88 88V8o 88 `8bo.   
        // 88  88  88 88    88 88 V8o88   `Y8b. 
        // 88  88  88 `8b  d8' 88  V888 db   8D 
        // YP  YP  YP  `Y88P'  VP   V8P `8888Y' 
        public ReactiveCollection<Mon> activeTeam = new ReactiveCollection<Mon>();
        

        // d8888b.  .d8b.  d888888b d888888b db      d88888b d88888b d888888b d88888b db      d8888b. 
        // 88  `8D d8' `8b `~~88~~' `~~88~~' 88      88'     88'       `88'   88'     88      88  `8D 
        // 88oooY' 88ooo88    88       88    88      88ooooo 88ooo      88    88ooooo 88      88   88 
        // 88~~~b. 88~~~88    88       88    88      88~~~~~ 88~~~      88    88~~~~~ 88      88   88 
        // 88   8D 88   88    88       88    88booo. 88.     88        .88.   88.     88booo. 88  .8D 
        // Y8888P' YP   YP    YP       YP    Y88888P Y88888P YP      Y888888P Y88888P Y88888P Y8888D' 
        public BattleFormation leftFormation;
        public BattleFormation rightFormation;
        public MessageBroker battlefieldEvents = new MessageBroker();
        
        
        // d888888b d8b   db d8888b. db    db d888888b 
        //   `88'   888o  88 88  `8D 88    88 `~~88~~' 
        //    88    88V8o 88 88oodD' 88    88    88    
        //    88    88 V8o88 88~~~   88    88    88    
        //   .88.   88  V888 88      88b  d88    88    
        // Y888888P VP   V8P 88      ~Y8888P'    YP    
        public MessageBroker inputEvents = new MessageBroker();
    }
}