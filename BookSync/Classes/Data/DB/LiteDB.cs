using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Data.Linq;
using BookSync.Classes.Algemeen;
using System.Data;

namespace BookSync.Classes.Data.DB
{
    public class LiteDB
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private string m_FileName;

        private SQLiteConnection m_Conn;

        private List<int> m_IDs;

        private DataTable m_Table;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public LiteDB(string FileName)
        {
            m_FileName = FileName;

            Init();
        }

        private void Init()
        {
            try
            {
                m_Conn = new SQLiteConnection(string.Format("Data Source={0}", m_FileName));
                m_Conn.Open();

                m_IDs = new List<int>();

                LaadIDs();
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "LiteDB.cs", "Init", ex);
            }

        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        private void LaadIDs()
        {
            try
            {
                using (SQLiteCommand cmd = m_Conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT ID FROM moz_places";

                    using (SQLiteDataAdapter myAdapter = new SQLiteDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            myAdapter.Fill(ds);

                            m_Table = ds.Tables[0];
                        }
                    }
                }

                object myObject = m_Table.Rows[0].ItemArray[0];


                m_IDs = m_Table.Rows.OfType<DataRow>().Select(r => Convert.ToInt32(r.ItemArray[0])).ToList();

                Globals.Log(string.Format("{0} IDs geladen", m_IDs.Count.ToString()));
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "LiteDB.cs", "Init", ex);
            }
        }
        
        #endregion Functions


        #region Properties
        /********************************************** Properties *****************************************************************/


        #endregion Properties

    }
}
