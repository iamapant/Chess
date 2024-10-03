using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EntityMovementManager {
    public abstract void FixedUpdate(Entity entity);

    public static bool Peek(Entity entity, Square square) {
        if (!IsSquareAllowEntry(entity, square)) return false;

        if (square?.Entities?.All(e => IsEntityAllowEntry(entity, e)) ?? false) return false;

        return true;
    }

    private static bool IsSquareAllowEntry(Entity entity, Square square) {
        throw new NotImplementedException();
        // Type entityType = entity.GetType();
        //
        // switch (entity) {
        //     case Unit unit:
        //         break;
        //     case Piece piece:
        //         break;
        //     case Structure structure:
        //         break;
        //     default:
        //         throw new NotImplementedException();
        // }
        //
        // return true;
    }

    private static bool IsEntityAllowEntry(Entity checker, Entity checkee) { throw new NotImplementedException(); }
}

public class NoMovement : EntityMovementManager {
    public override void FixedUpdate(Entity entity) { }
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

    public StayInSquare(Square square, float movementSpeed, MoveSpeedCalculator moveSpeedCalculator) : base(
        movementSpeed, moveSpeedCalculator) {
        this.square = square;
    }

    public override void FixedUpdate(Entity entity) {
        if (entity.transform.position != square.transform.position) {
            entity.transform.position
                += SpeedCalculator.CalculateSpeed(entity.transform.position, square.transform.position, MovementSpeed);
        }
    }
}