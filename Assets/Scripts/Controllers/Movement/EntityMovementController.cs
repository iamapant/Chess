using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public delegate void EntityMoved(Entity entity, [CanBeNull] Square from, Square to);

public class EntityMovementController {
    public static event EntityMoved OnEntityMoved;
    public EntityMovementAnimation MovementAnimation { get; private set; }

    public EntityMovementController(EntityMovementAnimation movementAnimation) {
        MovementAnimation = movementAnimation;
    }

    public void MoveToSquare(Entity entity, Square square) {
        if (!entity.MovePrecondition(square)) return;

        var oldSquare = entity.GetSquare();

        entity.GetSquare()?.Mediator.Deregister(entity);
        entity.SetSquare(square);

        entity.GetSquare()?.Mediator.Register(entity);
        
        OnEntityMoved?.Invoke(entity, oldSquare, square);
    }
}

[Serializable]
public abstract class EntityMovementAnimation {
    public float Speed;

    private Action MovingUpdate;

    public void Hello() {
    }
    
    public EntityMovementAnimation(float speed) {
        GameController.Instance.OnUpdateCallback += () => MovingUpdate?.Invoke();
    }

    public abstract void Start(Entity entity, Square to);
    public abstract void End(Entity entity);
}

// public class EntityMovementCalculator {
//     public static List<Square> CalculateValidDestinations(Entity entity) {
//         return PathFinder.CalculatePath()
//         return new();
//     }
// }