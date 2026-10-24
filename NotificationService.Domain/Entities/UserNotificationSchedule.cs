namespace NotificationService.Domain.Entities {
    public class UserNotificationSchedule {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public bool NotificationSent { get; set; } = false;
    }
}
