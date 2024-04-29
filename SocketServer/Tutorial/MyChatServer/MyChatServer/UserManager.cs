using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSBaseLib;

namespace ChatServer
{
    /// 유저 관리
    /// 로그인 성공한 유저만 방에 들어가고
    /// 방에 들어가기 전 유저 관리를 위해
    public class UserManager // 유저 관리를 위해
    {
        // 접속 후 유저 할당하고 있음
        int MaxUserCount;
        UInt64 UserSequenceNumber = 0;

        Dictionary<string, User> UserMap = new Dictionary<string, User>();

        public void Init(int maxUserCount)
        {
            MaxUserCount = maxUserCount;
        }

        public ERROR_CODE AddUser(string userID, string sessionID)
        {
            if(IsFullUserCount()) // 유저가 너무 많으면 안된다
                // 슈퍼소켓에서도 네트워크 쪽에서도 최대 수가 있어야하지만,
                // 애플리케이션 서버 쪽에도 최대 수가 있어야 한다!
                // 따라서 최대 수가 넘으면 막아주는 코드
            {
                return ERROR_CODE.LOGIN_FULL_USER_COUNT;
            }

            if (UserMap.ContainsKey(sessionID)) // 중복 체크
            {
                return ERROR_CODE.ADD_USER_DUPLICATION;
            }


            ++UserSequenceNumber; // 추가하면 유저의 유니크 번호를 +1
            
            var user = new User();
            user.Set(UserSequenceNumber, sessionID, userID);
            UserMap.Add(sessionID, user);

            return ERROR_CODE.NONE;
            // 그리고 이때 현재 우리 프로젝트는 원스레드
            // 따라서 락 사용 안하고 있음
        }

        public ERROR_CODE RemoveUser(string sessionID)
        {
            if(UserMap.Remove(sessionID) == false)
            {
                return ERROR_CODE.REMOVE_USER_SEARCH_FAILURE_USER_ID;
            }

            return ERROR_CODE.NONE;
        }

        public User GetUser(string sessionID)
        {
            User user = null;
            UserMap.TryGetValue(sessionID, out user);
            return user;
        }

        bool IsFullUserCount()
        {
            return MaxUserCount <= UserMap.Count();
         }
                
    }

    public class User
    {
        UInt64 SequenceNumber = 0; // 고유 번호 (매번 바뀜)
        string SessionID; // 슈퍼소켓의 세션 아이디
                          // MainServer의 ClientSession 코드를 보기!
                            // 세션아이디를 슈퍼소켓에게 알려줘야지,
                            // 어떤 세션 객체에 send를 해야할 지 알 수가 있음!
       
        public int RoomNumber { get; private set; } = -1; // 방에 들어갔을 때 room num
        string UserID; // 로그인 하면 유저 아이디 (어떤 유저인지)
                
        public void Set(UInt64 sequence, string sessionID, string userID)
        {
            SequenceNumber = sequence;
            SessionID = sessionID;
            UserID = userID;
        }                   
        
        public bool IsConfirm(string netSessionID)
        {
            return SessionID == netSessionID;
        }

        public string ID()
        {
            return UserID;
        }

        public void EnteredRoom(int roomNumber)
        {
            RoomNumber = roomNumber;
        }

        public void LeaveRoom()
        {
            RoomNumber = -1;
        }

        public bool IsStateLogin() { return SequenceNumber != 0; }

        public bool IsStateRoom() { return RoomNumber != -1; }
    }
    
}
