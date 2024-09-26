using System;
using JetBrains.Annotations;


public class EntityMediator : Mediator<Entity> {
    [CanBeNull] private Square square;

    private void Start() { square = GetComponent<Square>(); }

    protected override void OnRegistered(Entity entity) {
        var dict = entity.OnRegistered();
        foreach (var key in dict.Keys) {
            var payload = dict[key];
            if (payload != null) Broadcast(entity, payload, key);
        }

        foreach (var en in entities) {
            var dicts = en.OnRegistered();
            foreach (var key in dicts.Keys) {
                if (key(entity)) Message(en, entity, dicts[key]);
            }
        }
    }

    protected override void OnDeregistered(Entity entity) {
        foreach (var en in entities) {
            var dicts = en.OnDeregistered();
            foreach (var key in dicts.Keys) {
                if (key(entity)) Message(en, entity, dicts[key]);
            }
        }

        var dict = entity.OnDeregistered();
        foreach (var key in dict.Keys) {
            var payload = dict[key];
            if (payload != null) Broadcast(entity, payload, key);
        }
    }

    protected internal override void Broadcast(Entity source, IVisitor message, Func<Entity, bool> predicate = null) {
        base.Broadcast(source, message, predicate);

        square?.Accept(message);
    }

    protected override bool MediatorConditionMet(Entity component) => true;
}