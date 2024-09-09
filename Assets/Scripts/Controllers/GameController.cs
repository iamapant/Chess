using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : PersistentSingleton<GameController> {
    public TurnManager TurnManager { get; private set; }

    private void Start() {
        //OnTurnChange?.Invoke();   
    }
}
