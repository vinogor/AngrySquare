using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _currentCellIndex;

    // добавляю ячейки вручную чтобы сохранить нужную последовательность
    [SerializeField] private List<Cell> _cells = new();

    [SerializeField] private DiceRoller _diceRoller;

    [SerializeField] float jumpPower = 1f;
    [SerializeField] float jumpDuration = 0.5f;

    private void Awake()
    {
        _diceRoller.PlayerMoveAmountSet += OnPlayerMoveAmountSet;
    }

    private void OnPlayerMoveAmountSet(int amountMoves)
    {
        Move(amountMoves);
    }

    private void Move(int amountMoves)
    {
        if (amountMoves == 0) {
            _cells[_currentCellIndex].ActivateEffect();
            return;
        }

        int nextCellIndex = ++_currentCellIndex % _cells.Count;
        Cell nextCell = _cells[nextCellIndex];
        Vector3 nextCellCenter = nextCell.Center();

        transform
            .DOJump(nextCellCenter + Vector3.up * transform.lossyScale.y, jumpPower, 1, jumpDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // анимация нажатия ячейки 
                nextCell.transform.DOMoveY(nextCell.transform.position.y - 0.1f, 0.2f).SetLoops(2, LoopType.Yoyo);

                // чтобы следующий прыжок начинался только после завершения предыдущего
                Move(--amountMoves);
            });
    }
}