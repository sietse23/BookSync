using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Discovery;
using Google.Apis;
using Google.Apis.Authentication;
using Google.Apis.Requests;
using Google.Apis.Util;
using BookSync.Classes.Drive.Authentication;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using BookSync.Classes.Algemeen;

namespace BookSync.Classes.Drive.IO
{
    public delegate void DelegateDone();

    public class GRequestState
    {
        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public GRequestState(int buffSize)
        {
            BytesRead = 0;
            BufferRead = new byte[buffSize];
            StreamResponse = null;
        }

        #endregion Constructor, Load & Closing

        #region Properties
        /********************************************** Properties *****************************************************************/

        public int BytesRead;           
        public long TotalBytes;		    
        public double ProgIncrement;	
        public Stream StreamResponse;	
        public byte[] BufferRead;
        public FileStream WriteStream;

        public HttpWebRequest Request;
        public HttpWebResponse Response;

        public DelegateDriveProgress ProgressEvent;
        public DelegateDriveFinished FinishedEvent;

        #endregion Properties

    }
}
