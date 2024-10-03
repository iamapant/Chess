public class Structure : Entity {
    protected override bool MovePrecondition(Square newSquare) {
        return base.MovePrecondition(newSquare) && Square == null;
    }

    public override void OnMove(Square square) { }
    public override bool IsAllowed(Entity entity) { throw new System.NotImplementedException(); }
}