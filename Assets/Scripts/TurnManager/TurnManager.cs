using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager {
    public int TurnNumber { get; private set; } = 0;
    public Turn CurrentTurn { get; private set; }

    public Action<Turn> UpdateTurn;

    private List<Turn> _turns;

    public TurnManager(params Turn[] turns) { _turns = turns.ToList(); }

    public void NextTurn() {
        CurrentTurn?.EndTurn();
        if (CurrentTurn == _turns.First()) ++TurnNumber;

        CurrentTurn = CurrentTurn == _turns.Last() ? _turns.First() : _turns[_turns.IndexOf(CurrentTurn) + 1];

        CurrentTurn.StartTurn();

        UpdateTurn?.Invoke(CurrentTurn);
    }

    private IEnumerator SkipThisTurn(Turn turn) {
        if (turn == CurrentTurn) return new WaitWhile(() => turn == CurrentTurn);
        return new WaitWhile(() => turn != CurrentTurn);
        NextTurn();
    }

    /// <summary>
    /// Register to skip the next turn on the list
    /// </summary>
    public void SkipTurn() {
        SkipTurn(CurrentTurn == _turns.Last() ? _turns.First() : _turns[_turns.IndexOf(CurrentTurn) + 1]);
    }

    /// <summary>
    /// Register to skip a faction turn
    /// </summary>
    /// <param name="faction">Faction of the skipping turn</param>
    public void SkipTurn(Faction faction) { SkipTurn(_turns.Find(e => e is FactionTurn ft && ft.Faction == faction)); }

    public void SkipTurn(Turn turn) => GameController.Instance.StartCoroutine(SkipThisTurn(turn));

    public void RevertTurn() {
        throw new NotImplementedException();
        //Use memento to play the captured snapshot of the last turn
        //The list should capture the snapshot of the whole game at the start of each turn
    }

    #region Modify Turn List

    public void Add(Turn turn) {
        if (TurnNumber != 0) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (!_turns.Any(e => e.GetType() == turn.GetType())) _turns.Add(turn);
        else throw new Exception($"Turn {turn.ToString()} is already in the list");
    }

    public void Remove(Turn turn) {
        if (TurnNumber != 0) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (_turns.Contains(turn)) _turns.Remove(turn);
    }

    public void AddFirst(Turn turn) {
        if (TurnNumber != 0) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (!_turns.Any(e => e.GetType() == turn.GetType())) {
            if (_turns.Count > 0) _turns.AddBefore(turn, _turns.First());
            else _turns.Add(turn);
        }
        else throw new Exception($"Turn {turn.ToString()} is already in the Collection");
    }

    public void AddAfter(Turn turn, Turn after) {
        if (TurnNumber != 0) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (_turns.All(e => e != after)) throw new Exception($"The Collection does not contain {after.ToString()}");
        if (_turns.All(e => e.GetType() != turn.GetType())) {
            if (_turns.Count > 0) _turns.AddAfter(turn, after);
            else _turns.Add(turn);
        }
        else throw new Exception($"Turn {turn.ToString()} is already in the list");
    }

    public void AddBefore(Turn turn, Turn before) {
        if (TurnNumber != 0) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (_turns.All(e => e != before)) throw new Exception($"The Collection does not contain {before.ToString()}");
        if (_turns.All(e => e.GetType() != turn.GetType())) {
            if (_turns.Count > 0) _turns.AddAfter(turn, before);
            else _turns.Add(turn);
        }
        else throw new Exception($"Turn {turn.ToString()} is already in the list");
    }

    #endregion
}