namespace CryptoLib;

public class Mdp
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Deleted { get; set; }

    public Mdp(string username, string password)
    {
        Username = username;
        this.Password = password;
        Deleted = false;
    }
}