public abstract class EntityMovementManager {
    public abstract void FixedUpdate(Entity entity);
    
}

public class NoMovement : EntityMovementManager {
    public override void FixedUpdate(Entity entity) {
        
    }
}

public abstract class EntityMoving : EntityMovementManager {
    protected float MovementSpeed;
    protected MoveSpeedCalculator SpeedCalculator;

    public EntityMoving(float movementSpeed, MoveSpeedCalculator moveSpeedCalculator) {
        MovementSpeed = movementSpeed;
        SpeedCalculator = moveSpeedCalculator;
    }
}

public class StayInSquare : EntityMoving {
    private Square square;

    public StayInSquare(Square square, float movementSpeed, MoveSpeedCalculator moveSpeedCalculator) : base(movementSpeed, moveSpeedCalculator) { this.square = square; }

    public override void FixedUpdate(Entity entity) {
        if (entity.transform.position != square.transform.position) {
            entity.transform.position
                += SpeedCalculator.CalculateSpeed(entity.transform.position, square.transform.position, MovementSpeed);
        }
    }
}