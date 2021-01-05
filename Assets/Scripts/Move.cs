public abstract class Move {
    public abstract string moveName {get;}
    public abstract int damage {get;}
    public abstract int cost {get;}
    public abstract int overloadCost {get;}
    public abstract int overloadDamage {get;}
}

public class Tackle :  Move {
    public override string moveName { get { return "Tackle"; } }
    public override int damage { get { return 1; } }
    public override int cost { get { return 1; } }
    public override int overloadCost { get { return 1; } }
    public override int overloadDamage { get { return 1; } }
}