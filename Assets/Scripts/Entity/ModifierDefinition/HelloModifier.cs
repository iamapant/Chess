using System;
using UnityEngine;

public class HelloModifier : Modifier {
    public override AttachedTo Attach { get; } = AttachedTo.Entity;

    protected override void OnInitialize() {
        // Debug.Log(Modifable.name);
    }
    protected override void OnDestroy() { }

    protected override void OnDeregisteredAsEntity(Entity entity) { Debug.Log($"Goodbye {entity.name}!"); }

    protected override void OnDeregisteredAsSquare(Square square) { Debug.Log($"Goodbye {square.name}!"); }

    protected override void OnRegisteredAsEntity(Entity entity) {
        Debug.Log($"Hello {entity.name}, I'm {Modifiable.name}!");
    }

    protected override void OnRegisteredAsSquare(Square square) {
        Debug.Log($"Hello {square.name}, I'm {Modifiable.name}!");
    }
}