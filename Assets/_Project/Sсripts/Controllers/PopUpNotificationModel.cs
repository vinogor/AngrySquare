namespace _Project.Sсripts.UI.PopUpNotification
{
    public class PopUpNotificationModel
    {
        public readonly string Title;
        public readonly string Info;

        public PopUpNotificationModel(string title, string info)
        {
            Title = title;
            Info = info;
        }
    }
}