using Agava.YandexGames;
using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
    // TODO: ??? как сделать чтобы он запускался самым первым? или не обязательно? 
    private void Start()
    {
        Debug.Log("CompositeRoot started");

        // инициализация игровых сущностей
        
        
        // TODO: ??? когда это стоит вызывать? после инициализации того что выше?  
        // YandexGamesSdk.GameReady();
        // Debug.Log("YandexGamesSdk ready");
    }
}