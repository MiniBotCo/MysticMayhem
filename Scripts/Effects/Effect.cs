using System.Net.Cache;

public struct Effect
{
    public string name;
    public float amount;
    public bool permanent;

    public Effect(string name, float amount, bool permanent = false)
    {
        this.name = name;
        this.amount = amount;
        this.permanent = permanent;
    }
}