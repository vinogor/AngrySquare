using Agava.YandexGames;

namespace _Project.SDK
{
    public class ButtonOpenLeaderBoard
    {
        protected void Click()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Open();
#endif
        }

        private void Open()
        {
            PlayerAccount.Authorize();

            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
            }

            if (PlayerAccount.IsAuthorized == false)
                return;
        }
    }
}