using System.Collections.Generic;
using System.Linq;
using Sсripts;
using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("CompositeRoot started");

        List<Cell> cells = FindObjectsByType<Cell>(FindObjectsSortMode.None).ToList();

        List<CellInfo> cellInfos = Resources.LoadAll<CellInfo>("CellsInfo").ToList();

        CellInfo cellInfo = cellInfos[0];

        Effect attackEffect = cellInfo.Effect;

        Sprite attackSprite = cellInfo.Sprite;
        int attackAmount = cellInfo.Amount;

        // потом в цикле сделать для каждого эффекта 
        CellsWithoutEffect(cells)
            .Shuffle()
            .Take(attackAmount)
            .ToList()
            .ForEach(cell =>
            {
                cell.SetEffect(attackEffect);
                cell.SetSprite(attackSprite);
            });

    }

    private static List<Cell> CellsWithoutEffect(List<Cell> cells)
    {
        return cells.Where(cell => cell.IsEffectSet() == false).ToList();
    }
}