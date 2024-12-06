using System.Net.Cache;

public struct Effect
{
    public StatResource statResource;
    public bool permanent;

    public Effect(Stat stat, float amount, bool permanent = false)
    {
        statResource = new StatResource(stat, amount);
        this.permanent = permanent;
    }
}