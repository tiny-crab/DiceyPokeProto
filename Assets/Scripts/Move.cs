using System;
using System.Reflection;
using System.Linq;

public class Move : MoveTemplate {
    public Move(
        string name="",
        int damage=0,
        int cost=0,
        int overloadCost=0,
        int overloadDamage=0
    ): base(name, damage, cost, overloadCost, overloadDamage){}

    public static Move Tackle = new Move(
        name:"Tackle", damage:1, cost:1, overloadCost:1, overloadDamage:1
    );
    public static Move Ember = new Move(
        name:"Ember", damage:2, cost:2, overloadCost:2, overloadDamage:2
    );
    public static Move PoisonSting = new Move(
        name:"PoisonSting", damage:1, cost:2, overloadCost:2
    );

    public Move getMoveByName(string moveName) {
        // get all static fields of current class and return a constructed move that matches
        return this.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(Move))
            .Select(field => (Move) field.GetValue(null))
            .Where(move => move.name == moveName).First();
    }
}

public class MoveTemplate : IComparable {
    public string name;
    public int damage;
    public int cost;
    public int overloadCost;
    public int overloadDamage;

    protected MoveTemplate(
        string name="",
        int damage=0,
        int cost=0,
        int overloadCost=0,
        int overloadDamage=0
    ) {
        this.name = name;
        this.damage = damage;
        this.cost = cost;
        this.overloadCost = overloadCost;
        this.overloadDamage = overloadDamage;
    }

    public int CompareTo(object other) { return name.CompareTo(((Move)other).name); }
}

