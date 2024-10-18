using UnityEngine;

public abstract class Unit : Entity {
    protected override void OnMove(Square from, Square to) { }
    public override bool IsAllowed(Entity entity) { throw new System.NotImplementedException(); }
}