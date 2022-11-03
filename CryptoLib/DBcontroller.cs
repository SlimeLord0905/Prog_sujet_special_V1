﻿namespace CryptoLib;

using MySql.Data.MySqlClient;


public class DBcontroller
{
    private string connectionString;
    private MySqlConnection con;
    private string sql;
    
    public DBcontroller()
    {
        connectionString = "Host=192.168.163.129;Username=dev;Password=pwd;Database=mdp_app";

        con = new MySqlConnection(connectionString);
        con.Open();

        sql = "SELECT version()";

        /*using (var cmd = new MySqlCommand(sql, con))
        {
            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"MySQL version: {version}");
        }*/
        
        
        sql = "UPDATE users SET password=SHA2('12345678', 0) WHERE id = 1";
        var cmd = new MySqlCommand(sql, con);
        cmd.ExecuteNonQuery();
    }

    public bool adduser(String username, String fullname , String email, String password)
    {
        sql = "INSERT into users (username,fullname,password,email) VALUES ('"+username+"','"+fullname+"','"+password+"','"+email+"')";
        var cmd = new MySqlCommand(sql, con);
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
        sql = "SELECT * FROM users WHERE username ='" + username + "' and password = SHA2('" + password + "',0)";
        using (var cmd = new MySqlCommand(sql, con))
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
        }

        ConnectedUpdate(usr.id);
        return usr;
    }

    public List<password> GetUserPasswords(int id)
    {
        List<password> userMdp = new List<password>();

        sql = "SELECT * FROM passwords WHERE user_id ='"+id+"'";
        using (var cmd = new MySqlCommand(sql, con))
        {
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                userMdp.Add(new password(reader.GetInt32(0)
                    ,reader.GetInt32(1)
                    ,reader.GetString(2)
                    ,reader.GetString(3)
                    ,reader.GetString(4)
                    ,reader.GetDateTime(5)
                    ,reader.GetDateTime(6)));
            }
            reader.Close();        
        }
        return userMdp;
    }

    public bool ConnectedUpdate(int id)
    {
        sql = "UPDATE users SET last_connection = CURRENT_DATE()";
        var cmd = new MySqlCommand(sql, con);
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
        var cmd = new MySqlCommand(sql, con);
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
        sql = "INSERT into passwords (user_id,site,login,password) VALUES ('"+usr_id+"','"+site+"','"+login+"','"+password+"')";
        var cmd = new MySqlCommand(sql, con);
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
    public bool AddPasswordWithId(int id , int usr_id, String site, String login , String password )
    {
        sql = "INSERT into passwords (id,user_id,site,login,password) VALUES ('"+id+"','"+usr_id+"','"+site+"','"+login+"','"+password+"')";
        var cmd = new MySqlCommand(sql, con);
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
        sql = "UPDATE passwords SET user_id ='"+usr_id+"' ,site = '"+site+"',login = '"+login+"',password = '"+password+"',modified_at = CURRENT_DATE WHERE id = '"+id+"'";
        var cmd = new MySqlCommand(sql, con);
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