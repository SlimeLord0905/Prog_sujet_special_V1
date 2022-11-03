````plantuml
@startuml


class AesCrypter {
    -_aes: Aes
    +key: string {get,set}
    +AesCrypter()
    +AesCrypter(string)
    +EncryptToFile(string,string)
    +DecryptFromFile(string)
    +Encrypt(string)
    +Decrypt(string)
}

class Dbcontroller{
    -connectionString: string
    -con :MySqlConnection
    -sql: string
    +DBcontroller()
    +bool adduser(string,string,string,string)
    +user ConnecteUser(string, string)
    +List<password> GetUserPasswords(int)
    +bool ConnectedUpdate(int)
    +bool DeletePassword(int)
    +bool AddPassword(int,string,string,string)
    +bool UpdatePassword(int,int,string,string,string)   
}

class DbcontrollerOffline{
    -connectionString: string
    -con :MySqlConnection
    -sql: string
    +DBcontroller()
    +bool adduser(string,string,string,string)
    +user ConnecteUser(string, string)
    +List<password> GetUserPasswords(int)
    +bool ConnectedUpdate(int)
    +bool DeletePassword(int)
    +bool AddPassword(int,string,string,string)
    +bool UpdatePassword(int,int,string,string,string)
    +bool addToDeleteList(int)
    +bool RemovePasswordToDelete(int)
}

class password{
    +id :int{ get; set; }
    +user_id :int{ get; set; }
    +site :string{ get; set; }
    +login: string { get; set; }
    +Password: string { get; set; }
    +CreatedAt: DateTime { get; set; }
    +ModifiedAt: DateTime { get; set; }
    +password(int,string,string,string,DateTime,DateTime)
    +password(int,string,string,string,string,string)
}

class PasswordGenerator{
    -LowercaseLetters: string
    -UppercaseLetters: string
    -Digits: string
    -Symbols: string 
    +GeneratePassword(int,bool,bool,bool,bool)
    +ValidatePassword(string,bool,bool,bool,bool)
}

class SyncManager{
    +SyncPassword(Dbcontroller, DbcontrollerOffline, int)
}
SyncManager <|-- Dbcontroller
SyncManager <|-- DbcontrollerOffline
class user{
    username: string { get; set; }
    fullname: string { get; set; }
    email: string { get; set; }
    password: string { get; set; }
    id: string{ get; set; } = -1;
    +user(int)
}
PasswordGenerator <|-- password

@enduml
````