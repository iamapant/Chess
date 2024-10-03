public class Piece : Entity {
    
    public override void OnMove(Square square) {
    }

    public override bool IsAllowed(Entity entity) { throw new System.NotImplementedException(); }
}