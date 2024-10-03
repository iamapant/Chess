using System;

[Serializable]
public class Faction {
    public string Name {
        get => _name;
    }

    string _name { get; set; }
    
    private Faction(string name) { _name = name; }

    public static Faction Create(string name) {
        return new Faction(name);
    }
}