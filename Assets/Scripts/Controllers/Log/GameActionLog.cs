using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

public class GameActionLogger {
    public Stack<GameActionLog> Logs = default!;

    public bool TryPeakPlayerAction(out PlayerActionLog playerAction) {
        var list = Logs.ToList();
        list.Reverse();
        foreach (var log in list) {
            if (log is PlayerActionLog playerActionLog) {
                playerAction = playerActionLog;
                return true;
            }
        }

        playerAction = default;
        return false;
    }
    
    public static implicit operator Stack<GameActionLog>(GameActionLogger gameActionLogger) => gameActionLogger.Logs;
    public Stack<GameActionLog> ToStack() => Logs;
}

public delegate void OnRevertTrigger();

public abstract class GameActionLog {
    public event OnRevertTrigger OnRevertTriggered;
    public DateTime Timestamp { get; init; }
    public Turn Turn { get; init; }
    public int TurnNumber { get; init; }
    protected Stack<GameActionLog> Logger { get; init; }

    public GameActionLog() {
        Timestamp = DateTime.UtcNow;
        Turn = GameController.Instance.TurnManager.CurrentTurn;
        TurnNumber = GameController.Instance.TurnManager.TurnNumber;
        Logger = GameController.Instance.GameActionLogger;
    }

    public override abstract string ToString();

    public void Revert() {
        if (Logger.Contains(this) && Logger.TryPeek(out GameActionLog peek) && peek != this) {
            while (Logger.Count != 0 && (Logger.Pop() is { } log && log != this)) { log.Revert(); }
        }

        OnRevert();
        OnRevertTriggered?.Invoke();
    }

    protected abstract void OnRevert();
}