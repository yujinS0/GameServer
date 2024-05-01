using CSCommon;
using MemoryPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable CA1416

namespace OmokClient
{
    [SupportedOSPlatform("windows10.0.177630")]
    public partial class mainForm : Form
    {
        ClientSimpleTcp Network = new ClientSimpleTcp();

        bool IsNetworkThreadRunning = false;
        bool IsBackGroundProcessRunning = false;

        System.Threading.Thread NetworkReadThread = null;
        System.Threading.Thread NetworkSendThread = null;

        PacketBufferManager PacketBuffer = new PacketBufferManager();
        //ConcurrentQueue<byte[]> RecvPacketQueue = new ConcurrentQueue<byte[]>();
        //ConcurrentQueue<byte[]> SendPacketQueue = new ConcurrentQueue<byte[]>();
        Queue<byte[]> RecvPacketQueue = new Queue<byte[]>();
        Queue<byte[]> SendPacketQueue = new Queue<byte[]>();

        System.Windows.Forms.Timer dispatcherUITimer = new();



        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            PacketBuffer.Init((8096 * 10), MemoryPackPacketHeadInfo.HeadSize, 2048);

            IsNetworkThreadRunning = true;
            NetworkReadThread = new System.Threading.Thread(this.NetworkReadProcess);
            NetworkReadThread.Start();
            NetworkSendThread = new System.Threading.Thread(this.NetworkSendProcess);
            NetworkSendThread.Start();

            IsBackGroundProcessRunning = true;
            dispatcherUITimer.Tick += new EventHandler(BackGroundProcess);
            dispatcherUITimer.Interval = 100;
            dispatcherUITimer.Start();

            btnDisconnect.Enabled = false;

            SetPacketHandler(); // 패킷 핸들러 설정


            Omok_Init();
            DevLog.Write("프로그램 시작 !!!", LOG_LEVEL.INFO);
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsNetworkThreadRunning = false;
            IsBackGroundProcessRunning = false;

            Network.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string address = textBoxIP.Text;

            if (checkBoxLocalHostIP.Checked)
            {
                address = "127.0.0.1";
            }

            int port = Convert.ToInt32(textBoxPort.Text);

            if (Network.Connect(address, port))
            {
                labelStatus.Text = string.Format("{0}. 서버에 접속 중", DateTime.Now);
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;

                DevLog.Write($"서버에 접속 중", LOG_LEVEL.INFO);
            }
            else
            {
                labelStatus.Text = string.Format("{0}. 서버에 접속 실패", DateTime.Now);
            }

            PacketBuffer.Clear();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            SetDisconnectd();
            Network.Close();
        }

        

        void NetworkReadProcess()
        {
            while (IsNetworkThreadRunning)
            {
                if (Network.IsConnected() == false)
                {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }

                var recvData = Network.Receive();

                if (recvData != null)
                {
                    PacketBuffer.Write(recvData.Item2, 0, recvData.Item1);

                    while (true)
                    {
                        var data = PacketBuffer.Read();
                        if (data == null)
                        {
                            break;
                        }
                        
                        RecvPacketQueue.Enqueue(data);
                    }
                    //DevLog.Write($"받은 데이터: {recvData.Item2}", LOG_LEVEL.INFO);
                }
                else
                {
                    Network.Close();
                    SetDisconnectd();
                    DevLog.Write("서버와 접속 종료 !!!", LOG_LEVEL.INFO);
                }
            }
        }

        void NetworkSendProcess()
        {
            while (IsNetworkThreadRunning)
            {
                System.Threading.Thread.Sleep(1);

                if (Network.IsConnected() == false)
                {
                    continue;
                }

                lock (((System.Collections.ICollection)SendPacketQueue).SyncRoot)
                {
                    if (SendPacketQueue.Count > 0)
                    {
                        var packet = SendPacketQueue.Dequeue();
                        Network.Send(packet);
                    }
                }
                //if (SendPacketQueue.TryDequeue(out var packet))
                //{
                //    Network.Send(packet);
                //}
            }
        }


        void BackGroundProcess(object sender, EventArgs e)
        {
            ProcessLog();

            try
            {
                byte[] packet = null;

                lock (((System.Collections.ICollection)RecvPacketQueue).SyncRoot)
                {
                    if (RecvPacketQueue.Count() > 0)
                    {
                        packet = RecvPacketQueue.Dequeue();
                    }
                }

                if (packet != null)
                {
                    PacketProcess(packet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("ReadPacketQueueProcess. error:{0}", ex.Message));
            }
        }

        private void ProcessLog()
        {
            // 너무 이 작업만 할 수 없으므로 일정 작업 이상을 하면 일단 패스한다.
            int logWorkCount = 0;

            while (IsBackGroundProcessRunning)
            {
                System.Threading.Thread.Sleep(1);

                string msg;

                if (DevLog.GetLog(out msg))
                {
                    ++logWorkCount;

                    if (listBoxLog.Items.Count > 512)
                    {
                        listBoxLog.Items.Clear();
                    }

                    listBoxLog.Items.Add(msg);
                    listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                }
                else
                {
                    break;
                }

                if (logWorkCount > 8)
                {
                    break;
                }
            }
        }


        public void SetDisconnectd()
        {
            if (btnConnect.Enabled == false)
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }

            //while (true)
            //{
            //    if (SendPacketQueue.TryDequeue(out var temp) == false)
            //    {
            //        break;
            //    }
            //}

            listBoxRoomChatMsg.Items.Clear(); // ?
            listBoxRoomUserList.Items.Clear();

            EndGame();

            labelStatus.Text = "서버 접속이 끊어짐";
        } 

        void PostSendPacket(PACKETID packetID, byte[] packetData)
        {
            if (Network.IsConnected() == false)
            {
                DevLog.Write("서버 연결이 되어 있지 않습니다", LOG_LEVEL.ERROR);
                return;
            }

            // packetData가 null인 경우 비어 있는 바이트 배열로 처리
            // new byte[MemoryPackPacketHeadInfo.HeadSize]
            //if (packetData == null)
            //{
            //    packetData = new byte[0];
            //}

            var header = new MemoryPackPacketHeadInfo();
            header.TotalSize = (UInt16)packetData.Length;
            header.Id = (UInt16)packetID;
            header.Type = 0;
            header.Write(packetData);

            SendPacketQueue.Enqueue(packetData);
        }

        
        void AddRoomUserList(string userID)
        {
            listBoxRoomUserList.Items.Add(userID);
        }

        void RemoveRoomUserList(string userID) 
        {
            object removeItem = null;

            foreach( var user in listBoxRoomUserList.Items)
            {
                if((string)user == userID)
                {
                    removeItem = user;
                    break;
                }
            }

            if (removeItem != null)
            {
                listBoxRoomUserList.Items.Remove(removeItem);
            }
        }

        static public string ToReadableByteArray(byte[] bytes)
        {
            return string.Join(", ", bytes);
        }

        string GetOtherPlayer(string myName)
        {
            if(listBoxRoomUserList.Items.Count != 2)
            {
                return null;
            }

            var firstName = (string)listBoxRoomUserList.Items[0];
            if (firstName == myName)
            {
                return firstName;
            }
            else 
            {
                return (string)listBoxRoomUserList.Items[1];
            }
        }


        // 로그인 요청
        private void button2_Click(object sender, EventArgs e)
        {
            var loginReq = new PKTReqLogin();
            loginReq.AuthToken = textBoxUserPW.Text;
            loginReq.UserID = textBoxUserID.Text;
            var packet = MemoryPackSerializer.Serialize(loginReq);
                        
            PostSendPacket(PACKETID.REQ_LOGIN, packet);            
            DevLog.Write($"로그인 요청:  {textBoxUserID.Text}, {textBoxUserPW.Text}");
            DevLog.Write($"로그인 요청: {ToReadableByteArray(packet)}");
        }

        private void btn_RoomEnter_Click(object sender, EventArgs e)
        {
            var requestPkt = new PKTReqRoomEnter(); // 방 입장 요청
            requestPkt.RoomNumber = textBoxRoomNumber.Text.ToInt32(); // 방 번호

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt); // 직렬화

            PostSendPacket(PACKETID.REQ_ROOM_ENTER, sendPacketData); // 패킷 전송
            DevLog.Write($"방 입장 요청:  {textBoxRoomNumber.Text} 번");
        }

        private void btn_RoomLeave_Click(object sender, EventArgs e)
        {
            //PostSendPacket(PACKET_ID.ROOM_LEAVE_REQ,  null);
            PostSendPacket(PACKETID.REQ_ROOM_LEAVE, new byte[MemoryPackPacketHeadInfo.HeadSize]);
            DevLog.Write($"방 퇴장 요청:  {textBoxRoomNumber.Text} 번");
        }

        //private void btnRoomChat_Click(object sender, EventArgs e)
        //{
        //    if (textBoxRoomSendMsg.Text.IsEmpty())
        //    {
        //        MessageBox.Show("채팅 메시지를 입력하세요");
        //        return;
        //    }

        //    var requestPkt = new RoomChatReqPacket();
        //    requestPkt.SetValue(textBoxRoomSendMsg.Text);

        //    PostSendPacket(PACKETID.REQ_ROOM_CHAT, requestPkt.ToBytes());
        //    DevLog.Write($"방 채팅 요청");
        //}

        ////private void btnMatching_Click(object sender, EventArgs e)
        ////{
        ////    PostSendPacket(PACKETID.MATCH_USER_REQ, null);
        ////    DevLog.Write($"매칭 요청");
        ////}

        private void btnRoomChat_Click(object sender, EventArgs e)
        {
            if (textBoxRoomSendMsg.Text.IsEmpty())
            {
                MessageBox.Show("채팅 메시지를 입력하세요");
                return;
            }

            var requestPkt = new PKTReqRoomChat();
            requestPkt.ChatMessage = textBoxRoomSendMsg.Text;

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);

            PostSendPacket(PACKETID.REQ_ROOM_CHAT, sendPacketData);
            DevLog.Write($"방 채팅 요청");
        }

        //private void btnRoomRelay_Click(object sender, EventArgs e)
        //{
        //    if (textBoxRelay.Text.IsEmpty())
        //    {
        //        MessageBox.Show("릴레이 할 데이터가 없습니다");
        //        return;
        //    }

        //    /*var bodyData = Encoding.UTF8.GetBytes(textBoxRelay.Text);
        //    PostSendPacket(PACKET_ID.PACKET_ID_ROOM_RELAY_REQ, bodyData);
        //    DevLog.Write($"방 릴레이 요청");*/
        //}


        private void listBoxRoomChatMsg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxRelay_TextChanged(object sender, EventArgs e)
        {

        }


        void SendPacketOmokPut(int x, int y)
        {
            // 현재 돌의 종류를 확인
            int stoneType = OmokLogic.Is흑돌차례() ? (int)CSCommon.OmokRule.돌종류.흑돌 : (int)CSCommon.OmokRule.돌종류.백돌;

            var requestPkt = new PKTReqPutMok
            {
                PosX = x,
                PosY = y
            };

            var packet = MemoryPackSerializer.Serialize(requestPkt);
            PostSendPacket(PACKETID.ReqPutMok, packet);

            DevLog.Write($"put stone 요청 : x  [ {x} ], y: [ {y} ], 돌: [{stoneType}]");
        }

        private void btn_GameStartClick(object sender, EventArgs e)
        {
            DevLog.Write("게임 시작 버튼 없음 - 둘다 Ready 하면 알아서 시작함");
            //PostSendPacket(PACKETID.REQ_GAME_START, new byte[MemoryPackPacketHeadInfo.HeadSize]);
            //StartGame(true, "My", "Other");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddUser("test1");
            AddUser("test2");
        }

        void AddUser(string userID)
        {
            var value = new PvPMatchingResult
            {
                IP = "127.0.0.1",
                Port = 32451,
                RoomNumber = 0,
                Index = 1,
                Token = "123qwe"
            };
            var saveValue = MemoryPackSerializer.Serialize(value);

            var key = "ret_matching_" + userID;

            var redisConfig = new CloudStructures.RedisConfig("omok", "127.0.0.1");
            var RedisConnection = new CloudStructures.RedisConnection(redisConfig);

            var v = new CloudStructures.Structures.RedisString<byte[]>(RedisConnection, key, null);
            var ret = v.SetAsync(saveValue).Result;
        }

        //[MemoryPackable]
        public class PvPMatchingResult
        {
            public string IP;
            public UInt16 Port;
            public Int32 RoomNumber;
            public Int32 Index;
            public string Token;
        }

        // 게임 준비 요청
        private void button3_Click(object sender, EventArgs e)
        {
            PostSendPacket(PACKETID.ReqReadyOmok, new byte[MemoryPackPacketHeadInfo.HeadSize]);

            DevLog.Write($"게임 준비 완료 요청");
        }
    }
}
