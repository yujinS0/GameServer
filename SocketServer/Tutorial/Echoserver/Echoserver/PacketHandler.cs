using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echoserver
{
    public class PacketData
    {
        public NetworkSession session;
        public EFBinaryRequestInfo reqInfo;
    }

    public enum PACKETID : int
    {
        REQ_ECHO = 101, // 에코 서버 한개만 정의
    }

    public class CommonHandler // 요청에 대해 처리할 함수를 선언한 클래스
    {
        public void RequestEcho(NetworkSession session, EFBinaryRequestInfo requestInfo) // 네트워크 세션과, 인포를 슈퍼소켓에서 받아서..
                                                                                         // (MainServer의 RequestReceived 함수)
        {
            var totalSize = (Int16)(requestInfo.Body.Length + EFBinaryRequestInfo.HEADERE_SIZE);

            // 이 부분이 이제 마음대로 바꾸는 부분 (여기는 echo 라 그대로 보내는)
            List<byte> dataSource = new List<byte>();
            dataSource.AddRange(BitConverter.GetBytes(totalSize));
            dataSource.AddRange(BitConverter.GetBytes((Int16)PACKETID.REQ_ECHO));
            dataSource.AddRange(new byte[1]);
            dataSource.AddRange(requestInfo.Body);
            //////

            session.Send(dataSource.ToArray(), 0, dataSource.Count); // 여기가 중요! Send() 를 통해 상대방에게 데이터 보내고 있음!
                                                                     // AppSession 에 Send() 가 정의되어 있음
        }
    }

    public class PK_ECHO
    {
        public string msg;
    }
}
