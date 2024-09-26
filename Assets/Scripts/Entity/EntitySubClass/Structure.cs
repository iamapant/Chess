public class Structure : Entity {
    protected override bool MovePrecondition(Square newSquare) {
        return base.MovePrecondition(newSquare) && Square == null;
    }

    public override void OnMove(Square square) { }
}