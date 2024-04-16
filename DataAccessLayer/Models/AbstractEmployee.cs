namespace DataAccessLayer.Models;

public abstract class AbstractEmployee
{
    public AbstractEmployee() { }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int AccessLevel { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}