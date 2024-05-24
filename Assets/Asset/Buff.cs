using Cysharp.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class Buff
{
    public string BuffName { get; set; }
    public float EffectAmount { get; set; }
    public int Duration { get; set; }
    public Stat stat;
    public abstract UniTask ApplyEffect(Stats stats);
    public abstract void RemoveEffect(Stats stats);
}

public class TimedBuff : Buff
{
    public override async UniTask ApplyEffect(Stats stats)
    {
        stats.ChangeCurrent(EffectAmount);
        await UniTask.WaitForSeconds(Duration);
        RemoveEffect(stats);
        await UniTask.CompletedTask;
    }

    public override void RemoveEffect(Stats stats)
    {
        stats.ChangeCurrent(EffectAmount);
    }
}

public class PermanentBuff : Buff
{
    public override async UniTask ApplyEffect(Stats stats)
    {
        stats.ChangeCurrent(EffectAmount);
        await UniTask.CompletedTask;
    }

    public override void RemoveEffect(Stats stats)
    {
        // Permanent buffs do not remove their effect
    }
}

public class PeriodicBuff : Buff
{
    public override async UniTask ApplyEffect(Stats stats)
    {
        float startTime = Time.time;

        while (Time.time < startTime + Duration)
        {
            stats.ChangeCurrent(EffectAmount);
            await UniTask.Delay(1000);
        }

        RemoveEffect(stats);
        await UniTask.CompletedTask;
    }

    public override void RemoveEffect(Stats stats)
    {
        // Permanent buffs do not remove their effect
    }
}