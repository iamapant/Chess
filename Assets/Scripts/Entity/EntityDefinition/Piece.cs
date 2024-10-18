public abstract class Piece : Entity {
    
    protected override void OnMove(Square from, Square to) {
    }

    public override bool IsAllowed(Entity entity) { throw new System.NotImplementedException(); }
}