namespace CryptoLib;

public class Mdp
{
    public string Username { get; set; }
    public string Password { get; set; }

    public int id;
    public int user_id;
    public bool Deleted { get; set; }

    public Mdp(string username, string password)
    {
        Username = username;
        this.Password = password;
        Deleted = false;
    }
}