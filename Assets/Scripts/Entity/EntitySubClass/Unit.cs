using UnityEngine;

public class Unit : Entity {
    [SerializeField] MovableSquare movableSquare;
    public override void OnMove(Square square) { }
}