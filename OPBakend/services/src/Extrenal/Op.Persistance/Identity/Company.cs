namespace Op.Persistance.Identity;


public class Company : IAuditable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    // Postgres bağlantı parametreleri
    public string Host { get; set; } = default!;    // örn: "localhost" veya "db.mycorp.com"
    public int Port { get; set; }
    public string Database { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!; // (prod’da şifreyi şifreleyin/vault)

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
