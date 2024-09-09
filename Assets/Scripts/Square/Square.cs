using System;
using Blackboard.ScriptableObj;
using Mediator;
using Mediator.EntityMediator;
using UnityEngine;

public class Square : Mediator<Entity>, IVisitable {
    [SerializeField] private BlackboardData squareData;
    private void OnEnable() {
        GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn;
    }

    private void OnDisable() {
        GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn;
    }

    #region Mediator

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

    protected override bool MediatorConditionMet(Entity component) => true;

    protected internal override void Broadcast(Entity source, IVisitor message, Func<Entity, bool> predicate = null) {
        base.Broadcast(source, message, predicate);

        this.Accept(message);
    }

    public void Accept(IVisitor visitor) {
        var payload = visitor as EntityPayload;
        payload?.Content(this);
    }

    #endregion

    protected virtual void OnUpdateTurn(Turn turn) { }
}