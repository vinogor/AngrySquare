using UnityEngine;

namespace Sсripts
{
    [CreateAssetMenu(fileName = "CellInfo", menuName = "Gameplay/CellInfo")]
    public class CellInfo : ScriptableObject
    {
        // TODO: почему-то через UI unity не сеттится в это поле отнаследованные от Effect классы 
        [SerializeField] private Effect _effect;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _amount;

        public Effect Effect => _effect;
        public Sprite Sprite => _sprite;
        public int Amount => _amount;
    }
}