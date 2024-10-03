using System;

public abstract class SquareDataDecorator : ISquareData {
    private ISquareData squareData;

    public virtual ISquareData Decorate(ISquareData data) {
        squareData = data;
        return this;
    }

    public virtual ISquareData Undecorate(Type decoratorType) {
        if (decoratorType.IsSubclassOf(typeof(SquareDataDecorator))) {
            if (GetType() == decoratorType) { return this.GetData(); }

            var decorator = this;
            while (decorator.GetData() is SquareDataDecorator childDecorator) {
                if (childDecorator.GetType() == decoratorType) {
                    this.squareData = childDecorator.GetData();
                    return this;
                }

                decorator = childDecorator;
            }
        }

        return this;
    }

    public ISquareData Undecorate(SquareDataDecorator decorator) => this.Undecorate(decorator.GetType());

    public ISquareData GetData() => squareData;

    public float _goldPerTurn { get; init; } = 0f;
    public float _goldMultiplier { get; init; } = 0f;
    public float _faithPerTurn { get; init; } = 0f;
    public float _faithMultiplier { get; init; } = 0f;
    public float _expPerTurn { get; init; } = 0f;
    public float _maxExp { get; init; } = 0f;
    public float _stability { get; init; } = 0f;
    public float _maxStability { get; init; } = 0f;
    public float _minStability { get; init; } = 0f;
    public float _stabilityPerTurn { get; init; } = 0f;


    public virtual float GoldPerTurn {
        get => squareData.GoldPerTurn + _goldPerTurn;
    }

    public virtual float GoldMultiplier {
        get => squareData.GoldMultiplier + _goldMultiplier;
    }

    public virtual float FaithPerTurn {
        get => squareData.FaithPerTurn + _faithPerTurn;
    }

    public virtual float FaithMultiplier {
        get => squareData.FaithMultiplier + _faithMultiplier;
    }

    public virtual float ExpPerTurn {
        get => squareData.ExpPerTurn + _expPerTurn;
    }

    public virtual float MaxExp {
        get => squareData.MaxExp + _maxExp;
    }

    public virtual float Stability {
        get => squareData.Stability + _stability;
    }

    public virtual float MaxStability {
        get => squareData.MaxStability + _maxStability;
    }

    public virtual float MinStability {
        get => squareData.MinStability + _minStability;
    }

    public virtual float StabilityPerTurn {
        get => squareData.StabilityPerTurn + _stabilityPerTurn;
    }
}

public class GoldSquareDecorator : SquareDataDecorator {
    public GoldSquareDecorator(float GoldPerTurn, float GoldMultiplier = 0) : base() {
        _goldPerTurn = GoldPerTurn;
        _goldMultiplier = GoldMultiplier;
    }
}

public class NonZeroDecorator : SquareDataDecorator {
    public NonZeroDecorator() { }

    public override float GoldPerTurn {
        get => base.GoldPerTurn < 0 ? 0 : base.GoldPerTurn;
    }

    public override float GoldMultiplier {
        get => base.GoldMultiplier < 0 ? 0 : base.GoldMultiplier;
    }

    public override float FaithPerTurn {
        get => base.FaithPerTurn < 0 ? 0 : base.FaithPerTurn;
    }

    public override float FaithMultiplier {
        get => base.FaithMultiplier < 0 ? 0 : base.FaithMultiplier;
    }

    public override float ExpPerTurn {
        get => base.ExpPerTurn < 0 ? 0 : base.ExpPerTurn;
    }

    public override float MaxExp {
        get => base.MaxExp < 0 ? 0 : base.MaxExp;
    }

    public override float Stability {
        get => base.Stability < 0 ? 0 : base.Stability;
    }

    public override float MaxStability {
        get => base.MaxStability < 0 ? 0 : base.MaxStability;
    }

    public override float MinStability {
        get => base.MinStability < 0 ? 0 : base.MinStability;
    }

    public override float StabilityPerTurn {
        get => base.StabilityPerTurn < 0 ? 0 : base.StabilityPerTurn;
    }
}