using System;
using UnityEngine;

public abstract class EntityPower {
    private Entity _entity;
    private int _cooldown = 0;

    public EntityPower(Entity entity) {
        _entity = entity;
    }

    public abstract void OnUpdateTurn(Turn turn);
}