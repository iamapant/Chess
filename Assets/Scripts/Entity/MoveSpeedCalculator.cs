using UnityEngine;

public abstract class MoveSpeedCalculator {
    public abstract Vector2 CalculateSpeed(Vector2 position, Vector2 destination, float speed = 1);
}

public class LerpSpeedCalculator : MoveSpeedCalculator {
    Vector2 initialPosition;
    public override Vector2 CalculateSpeed(Vector2 position, Vector2 destination, float speed = 1) {
        if (initialPosition == null) initialPosition = position;
        
        return Vector2.zero;
    }
}