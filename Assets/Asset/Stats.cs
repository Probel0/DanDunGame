using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Lukomor.Reactive;

public interface IStats
{
    IReactiveProperty<float> CurrentValue { get; }
    IReactiveProperty<float> MaxValue { get; }
    IReadOnlyReactiveCollection<Buff> ActiveBuffs { get; }
    void ChangeCurrent(float value);
}

[Serializable]

public class Stats : IStats
{
    private ReactiveProperty<float> currentValue = new ReactiveProperty<float>(60);
    private ReactiveProperty<float> maxValue = new ReactiveProperty<float>(100);
    private ReactiveCollection<Buff> activeBuffs = new ReactiveCollection<Buff>();

    public IReactiveProperty<float> CurrentValue => currentValue;
    public IReactiveProperty<float> MaxValue => maxValue;
    public IReadOnlyReactiveCollection<Buff> ActiveBuffs => activeBuffs;

    public bool Regenerable { get; set; }
    public bool FreezeRegen { get; set; }

    private bool isRegenRunning;
    private const int regenRate = 1;
    private float maxValueRegen;

    public Stats(float currentValue, float maxValue) : this(currentValue, maxValue, false, false, false, 0f) { }

    public Stats(float currentValue, float maxValue, bool isRegenerable, bool isFreezeRegen, bool isRegenRunning, float maxValueRegen)
    {
        activeBuffs = new ReactiveCollection<Buff>();
        this.currentValue = new ReactiveProperty<float>(currentValue);
        this.maxValue = new ReactiveProperty<float>(maxValue);
        Regenerable = isRegenerable;
        FreezeRegen = isFreezeRegen;
        this.isRegenRunning = isRegenRunning;
        this.maxValueRegen = maxValueRegen;
    }

    public void ChangeCurrent(float value)
    {
        float newValue = Mathf.Clamp(CurrentValue.Value + value, 0, MaxValue.Value);
        this.currentValue.Value = newValue;

        if (newValue < maxValueRegen && !isRegenRunning)
        {
            UniTask.Void(Regen);
        }
    }

    public async UniTaskVoid Regen()
    {
        isRegenRunning = true;

        while (Regenerable && !FreezeRegen)
        {
            if (CurrentValue.Value < MaxValue.Value)
            {
                this.currentValue.Value = Mathf.Min(CurrentValue.Value + regenRate * Time.deltaTime, MaxValue.Value);
            }

            if (CurrentValue.Value >= MaxValue.Value || FreezeRegen)
            {
                break;
            }

            await UniTask.Yield();
        }

        isRegenRunning = false;
    }

    public async UniTask AddBuff(Buff buff)
    {
        activeBuffs.Add(buff);
        await buff.ApplyEffect(this);
        activeBuffs.Remove(buff);
    }

    // public void AddBuff(Buff buff)
    // {
    //     activeBuffs.Add(buff);
    //     buff.ApplyEffect(this).Forget();
    // }
}
