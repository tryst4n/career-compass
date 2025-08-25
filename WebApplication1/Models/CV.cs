namespace CareerCompass.Models
{
    public class CV
    {
        public int Id { get; set; }

        // Title (e.g., “Software Dev CV – Jan 2025”)
        public string Title { get; set; } = "My CV";

        // Store raw text
        public string Content { get; set; } = string.Empty;

        // Date of upload
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Constructeur (facultatif)
        public CV(int id, string title, string content, DateTime uploadedAt, string userId)
        {
            Id = id;
            Title = title;
            Content = content;
            UploadedAt = uploadedAt;
            UserId = userId;
        }

        // Constructeur vide (obligatoire si on a mis un autre constructeur)
        public CV() { }

        // Foreign key → link to User
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
