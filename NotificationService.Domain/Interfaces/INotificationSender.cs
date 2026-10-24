namespace NotificationService.Domain.Interfaces {
    public interface INotificationSender {
        Task<bool> SendNotificationAsync(string email, string message);
    }
}
