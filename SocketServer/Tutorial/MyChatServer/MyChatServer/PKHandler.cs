using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSBaseLib;


namespace ChatServer
{
    public class PKHandler // 얘를 공통으로 상속받는 PKHCommon과 Room 이 있음.
        // 패킷 처리 로직은 Common과 Room으로 나뉨 (하나의 파일이 너무 커지는 것을 막기 위해)
    {
        protected MainServer ServerNetwork;
        protected UserManager UserMgr = null;


        public void Init(MainServer serverNetwork, UserManager userMgr)
        {
            ServerNetwork = serverNetwork;
            UserMgr = userMgr;
        }            
                
    }
}
