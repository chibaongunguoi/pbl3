public class NotificationViewModel
{
    public List<Notification> Notifications { get; set; } = [];
    public int UnreadCount { get; set; } = 0;

    public NotificationViewModel(List<Notification> notifications, int unreadCount)
    {
        this.Notifications = notifications;
        this.UnreadCount = unreadCount;
    }
}