using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine.Protocol;


namespace Echoserver
{
    public class EFBinaryRequestInfo : BinaryRequestInfo // BinaryRequestInfo 상속받는 클래스 정의해주기! 
    {
        // 패킷 헤더용 변수
        public Int16 TotalSize { get; private set; } // 패킷 전체 크기
        public Int16 PacketID { get; private set; } // 어떤 패킷인지
        public SByte Value1 { get; private set; } // 구분하는 정보 (지금 사용 안함)

        public const int HEADERE_SIZE = 5;

        // 하나의 패킷을 가리키는 함수!
        public EFBinaryRequestInfo(Int16 totalSize, Int16 packetID, SByte value1, byte[] body) // header와 body 데이터를 가지는 부분
            : base(null, body)
        {
            // 마음대로 커스텀 가능 (ex. 회사명..?)
            this.TotalSize = totalSize;
            this.PacketID = packetID;
            this.Value1 = value1;
        }
    }

    // [ReceiveFilter class] : 이거 덕분에 슈퍼소켓에서 알아서 패킷에서 header와 body 부분의 정보를 채워서 우리에게 알려줌!
    public class ReceiveFilter : FixedHeaderReceiveFilter<EFBinaryRequestInfo> // Header 크기 고정되어 있는 데이터를 필터링 하겠다.
    {
        public ReceiveFilter() : base(EFBinaryRequestInfo.HEADERE_SIZE) // base에 헤더 크기 꼭 넣어주기! -> 그래야지 나중에 header로 넘겨요
        {
        }

        // 여기서 아래 두 함수를 꼭 재정의 해줘야 한다!
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length) // body의 크기를 알려주는 함수
                                                       // 매개변수들은 supersocket이 다 넣어줌
        {
            if (!BitConverter.IsLittleEndian) // LittleEndian으로 바꾸기 (C#은 cpu에 상관 없이 .net에서는 LittleEndian이 기본)
                Array.Reverse(header, offset, 2);
            // 여기가 핵심
            var packetTotalSize = BitConverter.ToInt16(header, offset); // header 크기
            return packetTotalSize - EFBinaryRequestInfo.HEADERE_SIZE; // body = totalSize - header
        }

        protected override EFBinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
                                                                                            // bodyBuffer 이거 사실 진짜 바디버퍼가 아니라
                                                                                            // ~버퍼에 들어있는 내용이 바디부분이라는 느낌?
            if (!BitConverter.IsLittleEndian) // LittleEndian으로 바꾸기 (C#은 cpu에 상관 없이 .net에서는 LittleEndian이 기본)
                Array.Reverse(header.Array, 0, EFBinaryRequestInfo.HEADERE_SIZE);
            // 여기가 핵심 
            return new EFBinaryRequestInfo(BitConverter.ToInt16(header.Array, 0), // header
                                           BitConverter.ToInt16(header.Array, 0 + 2), // body
                                           (SByte)header.Array[4],
                                           bodyBuffer.CloneRange(offset, length)); // body data 위치
        }
    }
}
