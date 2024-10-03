using System;
using UnityEngine;

public class HelloModifier : Modifier {
    protected override void OnInitialize() { }
    protected override void OnDestroy() { }

    protected override void OnDeregisteredAsEntity(Entity entity) { Debug.Log($"Goodbye {entity.name}!"); }

    protected override void OnDeregisteredAsSquare(Square square) { Debug.Log($"Goodbye {square.name}!"); }

    protected override void OnRegisteredAsEntity(Entity entity) {
        Debug.Log($"Hello {entity.name}, I'm {Entity.name}!");
    }

    protected override void OnRegisteredAsSquare(Square square) {
        Debug.Log($"Hello {square.name}, I'm {Entity.name}!");
    }
}