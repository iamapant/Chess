using System;
using UnityEngine;

[Serializable]
public abstract class MoveSpeedCalculator {
    public float Speed;
    public abstract Vector3 CalculateSpeed(Vector3 position, Vector3 destination, float speed = 1);
}

[Serializable]
public class LerpSpeedCalculator : MoveSpeedCalculator {
    Vector3? initialPosition;

    public override Vector3 CalculateSpeed(Vector3 position, Vector3 target, float speed = 1) {
        if (initialPosition is null ||
            Vector3.Distance(position, target) > Vector3.Distance((Vector3)initialPosition, target))
            initialPosition = position;

        var t =
            (Vector3.Distance(position, initialPosition ?? Vector3.zero)
             / Vector3.Distance(initialPosition ?? Vector3.zero, target)
             + Time.fixedDeltaTime * speed
            );
        var lerp = Vector3.Lerp((Vector3)initialPosition, target, t);
        return lerp - position;
    }
}

[Serializable]
public class ConstantSpeedCalculator : MoveSpeedCalculator {
    public override Vector3 CalculateSpeed(Vector3 position, Vector3 destination, float speed = 1) {
        return Vector3.MoveTowards(position, destination, speed * Time.fixedDeltaTime) - position;
    }
}