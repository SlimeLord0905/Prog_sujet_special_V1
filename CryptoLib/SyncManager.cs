namespace CryptoLib;

public class SyncManager
{
    public void SyncPasswords(DBcontroller Db, DBcontrollerOffline DbOf, int CurrentUser)
    {
        List<password> SyncPwd;
        List<password> currentpwd;
        
        
        currentpwd = Db.GetUserPasswords(CurrentUser);
        SyncPwd = DbOf.GetUserPasswords(CurrentUser);
               
        List<int> todelete = DbOf.GetPasswordsToDelete(CurrentUser);
        foreach (int id in todelete)
        {
            Db.DeletePassword(id);
            DbOf.RemovePasswordToDelete(id);
        }

        foreach (password pwd in currentpwd)
        {
            foreach (password sync in SyncPwd)
            {
                if (pwd.id == sync.id && pwd.ModifiedAt != sync.ModifiedAt)
                {
                    Db.UpdatePassword(sync.id, sync.user_id, sync.site, sync.login, sync.Password);
                }
            }
        }
        int addcap = currentpwd.Last().id;
        foreach (password sync in SyncPwd)
        {
            if (sync.id > addcap)
            {
                Db.AddPasswordWithId(sync.id, sync.user_id, sync.site, sync.login, sync.Password);
            }
        }
    }
}