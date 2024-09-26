using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]    
public class MovableSquare {
        public SerializedDictionary<Vector2Int, bool> MovableDirections = new ();

        public List<Square> QueryValidSquares(Entity entity) {
                throw new NotImplementedException();
        }
}
