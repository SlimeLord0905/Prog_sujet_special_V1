using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace CryptoLib;

public class DBcontrollerOffline
{
     private string connectionString;
    private SQLiteConnection con;
    private string sql;
    
    public DBcontrollerOffline()
    {
        string path = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? "";
        path = Path.Combine(path, "passwords.sqlite");
        connectionString = "URI=file:" + path;
        
        con = new SQLiteConnection(connectionString);
        con.Open();

        
    }

    public bool adduser(String username, String fullname , String email, String password)
    {
        sql = "INSERT into users (username,fullname,password,email) VALUES ('"+username+"','"+fullname+"','"+password+"','"+email+"')";
        var cmd = new SQLiteCommand(sql, con);
        try
        {
            var result = cmd.ExecuteNonQuery();
            if (result != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The information you entered were invalid");
            return false;
        }
    }
    public user ConnecteUser(String username, String password)
    {
        user usr = new user(-1);
        sql = "SELECT * FROM users WHERE username ='" + username + "'";
        using (var cmd = new SQLiteCommand(sql, con))
        {
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                usr.id = reader.GetInt32(0);
                usr.username = reader.GetString(1);
                usr.fullname = reader.GetString(2);
                usr.password = reader.GetString((3));
                usr.email = reader.GetString(4);
            }
            reader.Close();
            if (BCrypt.Net.BCrypt.Verify(usr.password, password))
            {
                ConnectedUpdate(usr.id);
            }
            else
            {
                usr.id = -1;
            }
        }
        return usr;
    }

    public List<password> GetUserPasswords(int id)
    {
        List<password> userMdp = new List<password>();

        sql = "SELECT * FROM passwords WHERE user_id ='"+id+"'";
        using (var cmd = new SQLiteCommand(sql, con))
        {
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                userMdp.Add(new password(reader.GetInt32(0)
                    ,reader.GetInt32(1)
                    ,reader.GetString(2)
                    ,reader.GetString(3)
                    ,reader.GetString(4) ));
            }
            reader.Close();        
        }
        return userMdp;
    }

    public bool ConnectedUpdate(int id)
    {
        sql = "UPDATE users SET last_connection = DATE('now')";
        var cmd = new SQLiteCommand(sql, con);
        try
        {
            var result = cmd.ExecuteNonQuery();
            if (result != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The information you entered were invalid");
            return false;
        }
    }
    
    public bool DeletePassword(int id)
    {
        sql = "DELETE FROM passwords WHERE id ='" + id + "'";
        var cmd = new SQLiteCommand(sql, con);
        var exe = cmd.ExecuteNonQuery();
        if (exe > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool AddPassword(int usr_id, String site, String login , String password )
    {
        sql = "INSERT into passwords (user_id,site,username,password,created_at,modified_at) VALUES ('"+usr_id+"','"+site+"','"+login+"','"+password+"',DATE('now'), DATE('now') )";
        var cmd = new SQLiteCommand(sql, con);
        try
        {
            var result = cmd.ExecuteNonQuery();
            if (result != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The information you entered were invalid");
            return false;
        }
    }
    public bool UpdatePassword(int id, int usr_id, String site, String login, String password)
    {
        sql = "UPDATE passwords SET user_id ='"+usr_id+"' ,site = '"+site+"',username = '"+login+"',password = '"+password+"',modified_at = DATE('now') WHERE id = '"+id+"'";
        var cmd = new SQLiteCommand(sql, con);
        try
        {
            var result = cmd.ExecuteNonQuery();
            if (result != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The information you entered were invalid");
            return false;
        }
    }

    public bool AddPasswordToDelete(int id)
    {
        sql = "INSERT into Delete_at_sync (password_id) VALUES ('" + id + "')";
        var cmd = new SQLiteCommand(sql, con);
        try
        {
            var result = cmd.ExecuteNonQuery();
            if (result != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The information you entered were invalid");
            return false;
        }
    }

    public bool RemovePasswordToDelete(int id)
    {
        sql = "DELETE from Delete_at_sync WHERE password_id = ('" + id + "')";
        var cmd = new SQLiteCommand(sql, con);
        try
        {
            var result = cmd.ExecuteNonQuery();
            if (result != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The information you entered were invalid");
            return false;
        }
    }
}