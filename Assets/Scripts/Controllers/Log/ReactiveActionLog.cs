using UnityEditor;
using UnityEngine;

public abstract class ReactiveActionLog : GameActionLog {
}

public abstract class EntityReactiveAction : ReactiveActionLog {
    public Entity Entity { get; init; }

    public EntityReactiveAction(Entity entity) : base() {
        Entity = entity;
    }
}

public class EntityDestroyedAction : EntityReactiveAction {
    GameObject go;

    public EntityDestroyedAction(Entity entity) : base(entity) {
        go = Object.Instantiate(entity.gameObject);
        go.SetActive(false);
    }

    public override string ToString() => $"Entity {go.GetComponent<Entity>().name} is destroyed";
    protected override void OnRevert() { go.SetActive(true); }
}

public class EntitySpawnedAction : EntityReactiveAction {
    GameObject go;
    public EntitySpawnedAction(Entity entity) : base(entity) => go = entity.gameObject;


    public override string ToString() => $"Entity {go.GetComponent<Entity>().name} is spawned";

    protected override void OnRevert() {
        Object.DestroyImmediate(go);
        if (Logger.TryPeek(out var log) && log is EntityDestroyedAction destroyLog && destroyLog.Entity == Entity) {
            Logger.Pop();
        }
    }
}

public abstract class SquareReactiveAction : ReactiveActionLog {
    protected Square Square { get; init; }
    public SquareReactiveAction(Square square) : base() => Square = square;
}

public class SquareDestroyedAction : SquareReactiveAction {
    private GameObject go;

    public SquareDestroyedAction(Square square) : base(square) {
        go = GameObject.Instantiate(square.gameObject);
        go.SetActive(false);
    }

    public override string ToString() => $"Square {go.GetComponent<Square>().name} is destroyed";
    protected override void OnRevert() => go.SetActive(true);
}

public class SquareSpawnedAction : SquareReactiveAction {
    GameObject go;
    public SquareSpawnedAction(Square square) : base(square) => go = square.gameObject;


    public override string ToString() => $"Entity {go.GetComponent<Entity>().name} is spawned";

    protected override void OnRevert() {
        Object.DestroyImmediate(go);
        if (Logger.TryPeek(out var log) && log is SquareSpawnedAction destroyLog && destroyLog.Square == Square) {
            Logger.Pop();
        }
    }
}

