public abstract  class Structure : Entity {
    protected internal override bool MovePrecondition(Square newSquare) {
        return base.MovePrecondition(newSquare) && Square == null;
    }

    protected override void OnMove(Square from, Square to) { }
    public override bool IsAllowed(Entity entity) { throw new System.NotImplementedException(); }
}