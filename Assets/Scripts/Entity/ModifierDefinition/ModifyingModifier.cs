using UnityEngine;

public class ModifyingModifier : Modifier {
    [SerializeField]
    Modifier _modifier;
    
    protected override void OnInitialize() {
        
    }

    protected override void OnDeregisteredAsEntity(Entity entity) {
    }

    protected override void OnDeregisteredAsSquare(Square square) {
    }

    protected override void OnRegisteredAsEntity(Entity entity) {
        entity.AddModifier(_modifier);
    }

    protected override void OnRegisteredAsSquare(Square square) {
    }
}