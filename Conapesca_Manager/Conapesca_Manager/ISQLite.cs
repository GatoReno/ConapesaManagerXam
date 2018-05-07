using System;
using System.Collections.Generic;
using System.Text;

namespace Conapesca_Manager
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetConnection();

    }
}
