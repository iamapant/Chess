using System;
using System.Linq;
using JetBrains.Annotations;


public class EntityMediator : Mediator<Entity> {
    Square _square;

    private void Awake() {
        _square = gameObject.GetComponent<Square>();
    }

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
        
        var squarePayloads = _square.OnRegistered();
        foreach (var key in squarePayloads.Keys) {
            if (key(entity)) Message(entity, squarePayloads[key]);
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
        
        var squarePayloads = _square.OnDeregistered();
        foreach (var key in squarePayloads.Keys) {
            if (key(entity)) Message(entity, squarePayloads[key]);
        }
    }

    protected internal override void Broadcast(Entity source, IVisitor message, Func<Entity, bool> predicate = null) {
        base.Broadcast(source, message, predicate);

        _square?.Accept(message);
    }

    protected internal void Broadcast(Square source, IVisitor message, Func<Entity, bool> predicate = null) {
        if (source != _square) return;
        
        entities.Where(e => SenderConditionMet(e, predicate)
                            && MediatorConditionMet(e))
            .ForEach(e => e.Accept(message));
    }
    
    protected void Message(Entity target, EntityPayload message) {
        entities.FirstOrDefault(e => e.Equals(target))?.Accept(message);
    }

    protected override bool MediatorConditionMet(Entity component) => true;
}