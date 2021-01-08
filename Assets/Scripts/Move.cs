using System.Reflection;
using System.Linq;
using System;

public class Move {
    public static Move Tackle = new Move(
        name:"Tackle", desc:"1 DMG\nOverload: +1 DMG",
        damage:1, cost:1,
        overloadCost:1, overloadDamage:1
    );
    public static Move QuickAttack = new Move(
        name:"Quick Attack", desc:"1 DMG, +1 ACT\nOverload: +1 DMG",
        damage:1, cost:2, actionCost:0,
        overloadCost:2, overloadDamage:1
    );
    public static Move Roar = new Move(
        name:"Roar", desc:"+2 ATK\nOverload: +1 ATK",
        cost: 3,
        overloadCost: 5, overloadDamage:0, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            attacker.attackStack += 1 + overloadValue;
        }
    );
    public static Move Ember = new Move(
        name:"Ember", desc:"2 DMG \nOverload: +2 DMG",
        damage:2, cost:2,
        overloadCost:2, overloadDamage:2,
        evolveThreshold:0, evolvedMoveName:"Flamethrower"
    );
    public static Move Flamethrower = new Move(
        name:"Flamethrower", desc:"4 DMG \nOverload: -1 Enemy ATK",
        damage:4, cost:3, overloadCost:8, overloadDamage:0,
        extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            target.attackStack -= overloadValue;
        }
    );
    public static Move DragonDance = new Move(
        name:"Dragon Dance", desc: "+1 ATK\nOverload: +1 ACT",
        cost:4, overloadCost:4, overloadDamage:0, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            attacker.attackStack += 1;
            attacker.remainingActions += overloadValue;
        }
    );
    public static Move ThunderWave = new Move(
        name:"Thunder Wave", desc: "Enemy +1 PAR\nOverload: +1 PAR",
        cost: 3,
        overloadCost: 5, overloadDamage:0, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            target.paralysisStack = 1 + overloadValue;
        }
    );
    public static Move PoisonSting = new Move(
        name:"Poison Sting", desc: "1 DMG & 1 POI\nOverload: +1 POI",
        damage:1, cost:2, overloadCost:1, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            target.poisonStack += 1 + overloadValue;
        }
    );
    public static Move PoisonJab = new Move(
        name:"Poison Jab", desc: "DMG = POI\nOverload: +1 POI",
        damage:0, cost:3, overloadCost:2, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            target.poisonStack += overloadValue;
            target.currentHealth -= target.poisonStack;
        }
    );
    public static Move IronDefense = new Move(
        name:"Iron Defense", desc: "+1 DEF\nOverload: +1 DEF",
        damage:0, cost:3, overloadCost:3, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            attacker.defenseStack += 1 + overloadValue;
        }
    );


    public Move getMoveByName(string moveName) {
        // get all static fields of current class and return a constructed move that matches
        return this.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(Move))
            .Select(field => (Move) field.GetValue(null))
            .Where(move => move.name == moveName).First();
    }

    public string name;
    public string desc;
    public int damage;
    public int cost;
    public int actionCost;
    public int overloadCost;
    public int overloadDamage;
    public int evolveThreshold;
    public string evolvedMoveName;
    public Action<MonEntity, MonEntity, int> extraEffects;


    public Move(
        string name="",
        string desc="",
        int damage=0,
        int cost=0,
        int actionCost=1,
        int overloadCost=0,
        int overloadDamage=0,
        int evolveThreshold=0,
        string evolvedMoveName="",
        Action<MonEntity, MonEntity, int> extraEffects=null
    ) {
        this.name = name;
        this.desc = desc;
        this.damage = damage;
        this.cost = cost;
        this.actionCost = actionCost;
        this.overloadCost = overloadCost;
        this.overloadDamage = overloadDamage;
        this.evolveThreshold = evolveThreshold;
        this.evolvedMoveName = evolvedMoveName;
        this.extraEffects = extraEffects;
    }
}
