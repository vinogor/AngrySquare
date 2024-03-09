using System.Collections.Generic;
using Domain.Effects;
using Domain.Spells;

namespace Services.Save
{
    public class DataRecord
    {
        // player 
        public int PlayerDamageValue;
        public int PlayerDefenceValue;
        public int PlayerHealthValue;
        public int PlayerHealthMaxValue;
        public int PlayerManaValue;
        public int PlayerManaMaxValue;
        public List<SpellName> SpellNames;
        public int PlayersCellIndex;

        // enemy
        public int EnemyLevelValue;
        public int EnemyDamageValue;
        public int EnemyDefenceValue;
        public int EnemyHealthValue;
        public int EnemyHealthMaxValue;
        public List<int> TargetCellsIndexes;

        // common
        public Dictionary<int, EffectName> CellIndexesWithEffectNames;
        public string FsmStateTypeName;
        public bool IsTutorialEnable;
    }
}