using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.UI;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts
{
    public class UiRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private PopUpWinDefeat _popUpWinDefeat;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private HealthBar _playerHealthBar;
        [SerializeField] [Required] private ManaBar _playerManaBar;
        [SerializeField] [Required] private DamageText _playerDamageText;
        [SerializeField] [Required] private DefenceText _playerDefenceText;
        [SerializeField] [Required] private PopUpChoiceOfThree _popUpChoiceOfThree;
        [SerializeField] [Required] private SpellBar _spellBar;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private HealthBar _enemyHealthBar;
        [SerializeField] [Required] private DamageText _enemyDamageText;
        [SerializeField] [Required] private DefenceText _enemyDefenceText;

        public PopUpWinDefeat PopUpWinDefeat => _popUpWinDefeat;
        public PopUpChoiceOfThree PopUpChoiceOfThree => _popUpChoiceOfThree;

        public PopUpWinDefeatController PopUpWinDefeatController { get; private set; }
        public SpellBarController SpellBarController { get; private set; }

        public void Initialize(Health playerHealth, Mana playerMana, Health enemyHealth, Damage playerDamage,
            Damage enemyDamage, Defence playerDefence, Defence enemyDefence, CellsAndSpellsSettings cellsAndSpellsSettings)
        {
            _playerHealthBar.Initialize(playerHealth);
            _playerManaBar.Initialize(playerMana);
            _enemyHealthBar.Initialize(enemyHealth);
            _playerDamageText.Initialize(playerDamage);
            _enemyDamageText.Initialize(enemyDamage);
            _playerDefenceText.Initialize(playerDefence);
            _enemyDefenceText.Initialize(enemyDefence);

            PopUpWinDefeatController = new PopUpWinDefeatController(_popUpWinDefeat);
            SpellBarController = new SpellBarController(_spellBar, cellsAndSpellsSettings);
        }
    }
}