using System.Reflection;
using System.Linq;
using System;

public class Move {
    public static Move Tackle = new Move(
        name:"Tackle", damage:1, cost:1, overloadCost:1, overloadDamage:1
    );
    public static Move QuickAttack = new Move(
        name:"Quick Attack", damage:1, cost:2, actionCost:0,
        overloadCost:2, overloadDamage:1
    );
    public static Move Ember = new Move(
        name:"Ember", damage:2, cost:2, overloadCost:2, overloadDamage:2,
        evolveThreshold:0, evolvedMoveName:"Flamethrower"
    );
    public static Move Flamethrower = new Move(
        name:"Flamethrower", damage:4, cost:3, overloadCost:2, overloadDamage:1
    );
    public static Move PoisonSting = new Move(
        name:"Poison Sting", damage:1, cost:2, overloadCost:1, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            target.poisonStack += 1 + overloadValue;
        }
    );
    public static Move PoisonJab = new Move(
        name:"Poison Jab", damage:0, cost:3, overloadCost:2, extraEffects:
        delegate(MonEntity attacker, MonEntity target, int overloadValue) {
            target.currentHealth -= target.poisonStack;
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
