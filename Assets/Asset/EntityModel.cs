using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Lukomor.Reactive;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class EntityModel : BaseModel
{
    [field: SerializeField] public string EntityName { get; set; }
    [field: SerializeField] public string EntityDescription { get; set; }

    [field: SerializeField] public ReactiveProperty<Stats> Health = new(new Stats(30, 30));
    [field: SerializeField] public ReactiveProperty<Stats> Manna = new(new Stats(30, 30));
    [field: SerializeField] public ReactiveProperty<Stats> Defense = new(new Stats(30, 30));
    [field: SerializeField] public ReactiveProperty<Stats> Speed = new(new Stats(30, 30));

    [field: SerializeField] public ReactiveCollection<Buff> Buffs;

    public override void Initialize()
    {
        Debug.Log("Load Recourses...");
        Debug.Log("Load Success");
    }

    public void AddBuff(Buff buff)
    {
        Buffs.Add(buff);

        switch (buff.stat)
        {
            case Stat.Health:
                Health.Value.AddBuff(buff).Forget();
                break;
            case Stat.Mana:
                Manna.Value.AddBuff(buff).Forget();
                break;
            case Stat.Defense:
                Defense.Value.AddBuff(buff).Forget();
                break;
            case Stat.Speed:
                Speed.Value.AddBuff(buff).Forget();
                break;
            default:
                throw new ArgumentException($"Unknown target stat: {buff.stat}");
        }
    }
}
