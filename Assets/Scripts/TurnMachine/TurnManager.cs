using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class TurnManager {
    public int TurnNumber { get; private set; } = 0;
    public Turn CurrentTurn { get; private set; }

    private bool _isPlaying = false;
    
    public Action<Turn> UpdateTurn;
    
    private List<Turn> _turns;

    public TurnManager(params Turn[] turns) {
        _turns = turns.ToList();
    }

    public void NextTurn() {
        _isPlaying = true;
        CurrentTurn?.EndTurn();
        if (CurrentTurn == _turns.Last()) {
            TurnNumber++;
            CurrentTurn = _turns.First();
        }
        else CurrentTurn = _turns[_turns.IndexOf(CurrentTurn) + 1];
        CurrentTurn.StartTurn();
        
        UpdateTurn?.Invoke(CurrentTurn);
    }

    public void RevertTurn() {
        throw new NotImplementedException();
        //Use reflection to play the captured image of the last turn
        //The list should capture the image of the whole game at the start of each turn
    }

    public void Add(Turn turn) {
        if (_isPlaying) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }
        if(!_turns.Any(e => e.GetType() == turn.GetType())) _turns.Add(turn);
        else throw new Exception($"Turn {turn.name} is already in the list");
    }

    public void Remove(Turn turn) {
        if (_isPlaying) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }
        if (_turns.Contains(turn)) _turns.Remove(turn);
    }
    
    public void AddFirst(Turn turn) {
        if (_isPlaying) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (!_turns.Any(e => e.GetType() == turn.GetType())) {
            if (_turns.Count > 0) _turns.AddBefore(turn, _turns.First());
            else _turns.Add(turn);
        }
        else throw new Exception($"Turn {turn.name} is already in the Collection");
    }

    public void AddAfter(Turn turn, Turn after) {
        if (_isPlaying) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (_turns.All(e => e != after)) throw new Exception($"The Collection does not contain {after.name}");
        if (_turns.All(e => e.GetType() != turn.GetType())) {
            if (_turns.Count > 0) _turns.AddAfter(turn, after);
            else _turns.Add(turn);
        }
        else throw new Exception($"Turn {turn.name} is already in the list");
    }
    public void AddBefore(Turn turn, Turn before) { 
        if (_isPlaying) {
            Debug.LogWarning("Cannot modify turn order while game is playing.");
            return;
        }

        if (_turns.All(e => e != before)) throw new Exception($"The Collection does not contain {before.name}");
        if (_turns.All(e => e.GetType() != turn.GetType())) {
            if (_turns.Count > 0) _turns.AddAfter(turn, before);
            else _turns.Add(turn);
        }
        else throw new Exception($"Turn {turn.name} is already in the list");}


}