using System;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerMana : Effect
    {
        private Mana _playerMana;
        private PlayerJumper _playerJumper;

        public PlayerMana(Mana playerMana, PlayerJumper playerJumper)
        {
            _playerMana = playerMana;
            _playerJumper = playerJumper;
        }

        public override void Activate(Action onComplete)
        {
            base.Activate(onComplete);

            _playerMana.ReplenishToMax();
            _playerJumper.PlayerJumpInPlace(onComplete.Invoke);
        }
    }
}