using System;
using Godot;

public partial class StatResource : Resource
{
    public Action OnZero;
    [Export] public Stat StatType { get; private set; }

    private float _statValue;
    [Export]
    public float StatValue
    {
        get => _statValue;
        set
        {
            _statValue = Mathf.Clamp(value, 0, Mathf.Inf);

            if (_statValue == 0)
            {
                OnZero?.Invoke();
            }
        }
    }

    // Some constructors to be used when adding stat resources, default is health with a value of 100
    public StatResource()
    {
        StatType = Stat.Health;
        _statValue = 100;
    }

    public StatResource(Stat statType, float statValue)
    {
        StatType = statType;
        _statValue = statValue;
    }
}