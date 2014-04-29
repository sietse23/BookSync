using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleverComponents.InetSuite;
using BookSync.Properties;
using System.Windows.Forms;
using System.IO;

namespace BookSync.Classes.FTP
{
    public class FTPClient
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private Ftp m_Ftp;
        private Settings m_Settings;

        private bool m_OK = false;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public FTPClient()
        {
            m_Settings = new Settings();
            m_Settings.Reload();

            try
            {
                m_Ftp = new Ftp();
                m_Ftp.Server = m_Settings.FTPServer;
                m_Ftp.UserName = m_Settings.FTPUser;
                m_Ftp.Password = m_Settings.FTPPass;
                m_Ftp.Port = Convert.ToInt32(m_Settings.FTPPort);
                m_Ftp.PassiveMode = true;

                m_Ftp.Open();

                if (m_Ftp.Active)
                {
                    m_OK = true;

                    m_Ftp.ChangeCurrentDir(@"/httpdocs/Places");
                    m_Ftp.PutFile("places.sqlite", File.OpenRead(m_Settings.PlacesFile));

                    m_Ftp.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/


        #endregion Functions


        #region Properties
        /********************************************** Properties *****************************************************************/

        public bool OK
        {
            get { return this.m_OK; }
        }


        #endregion Properties

    }
}
