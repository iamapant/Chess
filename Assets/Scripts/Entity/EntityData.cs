using System;
using UnityEngine;

public interface IEntityData {
    
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Data/EntityData")]
public class ValueEntityData : ScriptableObject, IEntityData { }

public abstract class EntityDataDecorator : IEntityData {
    IEntityData data;

    public virtual IEntityData Decorate(IEntityData data) {
        this.data = data;
        return this;
    }

    public virtual IEntityData Undecorate(Type decoratorType) {
        if (decoratorType.IsSubclassOf(typeof(EntityDataDecorator))) {
            if (GetType() == decoratorType) { return this.GetData(); }

            var decorator = this;
            while (decorator.GetData() is EntityDataDecorator childDecorator) {
                if (childDecorator.GetType() == decoratorType) {
                    this.data = childDecorator.GetData();
                    return this;
                }

                decorator = childDecorator;
            }
        }

        return this;
    }

    public IEntityData GetData() => data;
}