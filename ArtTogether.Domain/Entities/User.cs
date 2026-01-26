namespace ArtTogether.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public bool IsGuest { get; set; }
}
