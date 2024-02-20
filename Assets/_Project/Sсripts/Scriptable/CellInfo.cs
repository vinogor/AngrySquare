using System;
using _Project.Sсripts.Services.Effects;
using UnityEngine;

namespace _Project.Sсripts.Scriptable
{
    [Serializable]
    public class CellInfo
    {
        [field: SerializeField] public EffectName EffectName { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}