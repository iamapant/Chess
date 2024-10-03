using UnityEngine;

public class Unit : Entity {
    [SerializeField] MovableSquare movableSquare;
    public override void OnMove(Square square) { }
    public override bool IsAllowed(Entity entity) { throw new System.NotImplementedException(); }
}