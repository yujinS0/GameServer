namespace OmokClient
{
    partial class mainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            btnSocketDisconnect = new System.Windows.Forms.Button();
            btnSocketConnect = new System.Windows.Forms.Button();
            SocketGroup = new System.Windows.Forms.GroupBox();
            socketPortTextBox = new System.Windows.Forms.TextBox();
            socketPortText = new System.Windows.Forms.Label();
            checkBoxLocalHostIPSocket = new System.Windows.Forms.CheckBox();
            socketIPTextBox = new System.Windows.Forms.TextBox();
            socketIPText = new System.Windows.Forms.Label();
            labelStatus = new System.Windows.Forms.Label();
            listBoxLog = new System.Windows.Forms.ListBox();
            UserIDText = new System.Windows.Forms.Label();
            UserIDTextBox = new System.Windows.Forms.TextBox();
            UserPWTextBox = new System.Windows.Forms.TextBox();
            UserPWText = new System.Windows.Forms.Label();
            btnLogin = new System.Windows.Forms.Button();
            Room = new System.Windows.Forms.GroupBox();
            button3 = new System.Windows.Forms.Button();
            btnMatching = new System.Windows.Forms.Button();
            btnRoomChat = new System.Windows.Forms.Button();
            textBoxRoomSendMsg = new System.Windows.Forms.TextBox();
            listBoxRoomChatMsg = new System.Windows.Forms.ListBox();
            listBoxRoomUserList = new System.Windows.Forms.ListBox();
            btn_RoomLeave = new System.Windows.Forms.Button();
            btn_RoomEnter = new System.Windows.Forms.Button();
            textBoxRoomNumber = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            ApiGroup = new System.Windows.Forms.GroupBox();
            GamePortTextBox = new System.Windows.Forms.TextBox();
            GamePortText = new System.Windows.Forms.Label();
            checkBoxLocalHostIPGame = new System.Windows.Forms.CheckBox();
            GameIPTextBox = new System.Windows.Forms.TextBox();
            GameIPText = new System.Windows.Forms.Label();
            HivePortTextBox = new System.Windows.Forms.TextBox();
            HivePortText = new System.Windows.Forms.Label();
            checkBoxLocalHostIPHive = new System.Windows.Forms.CheckBox();
            HiveIPTextBox = new System.Windows.Forms.TextBox();
            HiveIPText = new System.Windows.Forms.Label();
            btnRegister = new System.Windows.Forms.Button();
            GamedataText1 = new System.Windows.Forms.Label();
            DataUserId = new System.Windows.Forms.Label();
            GameDataGroup = new System.Windows.Forms.GroupBox();
            label6 = new System.Windows.Forms.Label();
            DataExp = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            DataDraw = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            DataLose = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            DataWin = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            DataLevel = new System.Windows.Forms.Label();
            btnCancelMatching = new System.Windows.Forms.Button();
            SocketGroup.SuspendLayout();
            Room.SuspendLayout();
            ApiGroup.SuspendLayout();
            GameDataGroup.SuspendLayout();
            SuspendLayout();
            // 
            // btnSocketDisconnect
            // 
            btnSocketDisconnect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btnSocketDisconnect.Location = new System.Drawing.Point(526, 51);
            btnSocketDisconnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnSocketDisconnect.Name = "btnSocketDisconnect";
            btnSocketDisconnect.Size = new System.Drawing.Size(89, 29);
            btnSocketDisconnect.TabIndex = 29;
            btnSocketDisconnect.Text = "접속끊기";
            btnSocketDisconnect.UseVisualStyleBackColor = true;
            btnSocketDisconnect.Click += btnDisconnect_Click;
            // 
            // btnSocketConnect
            // 
            btnSocketConnect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btnSocketConnect.Location = new System.Drawing.Point(526, 16);
            btnSocketConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnSocketConnect.Name = "btnSocketConnect";
            btnSocketConnect.Size = new System.Drawing.Size(89, 30);
            btnSocketConnect.TabIndex = 28;
            btnSocketConnect.Text = "접속하기";
            btnSocketConnect.UseVisualStyleBackColor = true;
            btnSocketConnect.Click += btnConnect_Click;
            // 
            // SocketGroup
            // 
            SocketGroup.Controls.Add(socketPortTextBox);
            SocketGroup.Controls.Add(socketPortText);
            SocketGroup.Controls.Add(checkBoxLocalHostIPSocket);
            SocketGroup.Controls.Add(socketIPTextBox);
            SocketGroup.Controls.Add(socketIPText);
            SocketGroup.Controls.Add(btnSocketConnect);
            SocketGroup.Controls.Add(btnSocketDisconnect);
            SocketGroup.Location = new System.Drawing.Point(15, 20);
            SocketGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            SocketGroup.Name = "SocketGroup";
            SocketGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            SocketGroup.Size = new System.Drawing.Size(639, 87);
            SocketGroup.TabIndex = 27;
            SocketGroup.TabStop = false;
            SocketGroup.Text = "Socket Server 설정";
            // 
            // socketPortTextBox
            // 
            socketPortTextBox.Location = new System.Drawing.Point(289, 33);
            socketPortTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            socketPortTextBox.MaxLength = 6;
            socketPortTextBox.Name = "socketPortTextBox";
            socketPortTextBox.Size = new System.Drawing.Size(64, 27);
            socketPortTextBox.TabIndex = 18;
            socketPortTextBox.Text = "32451";
            socketPortTextBox.WordWrap = false;
            socketPortTextBox.TextChanged += socketPortTextBox_TextChanged;
            // 
            // socketPortText
            // 
            socketPortText.AutoSize = true;
            socketPortText.Location = new System.Drawing.Point(210, 40);
            socketPortText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            socketPortText.Name = "socketPortText";
            socketPortText.Size = new System.Drawing.Size(77, 20);
            socketPortText.TabIndex = 17;
            socketPortText.Text = "포트 번호:";
            // 
            // checkBoxLocalHostIPSocket
            // 
            checkBoxLocalHostIPSocket.AutoSize = true;
            checkBoxLocalHostIPSocket.Location = new System.Drawing.Point(363, 30);
            checkBoxLocalHostIPSocket.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxLocalHostIPSocket.Name = "checkBoxLocalHostIPSocket";
            checkBoxLocalHostIPSocket.Size = new System.Drawing.Size(127, 24);
            checkBoxLocalHostIPSocket.TabIndex = 15;
            checkBoxLocalHostIPSocket.Text = "localhost 사용";
            checkBoxLocalHostIPSocket.UseVisualStyleBackColor = true;
            checkBoxLocalHostIPSocket.CheckedChanged += checkBoxLocalHostIPSocket_CheckedChanged;
            // 
            // socketIPTextBox
            // 
            socketIPTextBox.Location = new System.Drawing.Point(87, 30);
            socketIPTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            socketIPTextBox.MaxLength = 30;
            socketIPTextBox.Name = "socketIPTextBox";
            socketIPTextBox.Size = new System.Drawing.Size(111, 27);
            socketIPTextBox.TabIndex = 11;
            socketIPTextBox.Text = "34.22.95.236";
            socketIPTextBox.WordWrap = false;
            socketIPTextBox.TextChanged += socketIPTextBox_TextChanged;
            // 
            // socketIPText
            // 
            socketIPText.AutoSize = true;
            socketIPText.Location = new System.Drawing.Point(8, 38);
            socketIPText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            socketIPText.Name = "socketIPText";
            socketIPText.Size = new System.Drawing.Size(77, 20);
            socketIPText.TabIndex = 10;
            socketIPText.Text = "서버 주소:";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new System.Drawing.Point(18, 1016);
            labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new System.Drawing.Size(138, 20);
            labelStatus.TabIndex = 40;
            labelStatus.Text = "서버 접속 상태: ???";
            // 
            // listBoxLog
            // 
            listBoxLog.FormattingEnabled = true;
            listBoxLog.HorizontalScrollbar = true;
            listBoxLog.Location = new System.Drawing.Point(15, 727);
            listBoxLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new System.Drawing.Size(637, 264);
            listBoxLog.TabIndex = 41;
            // 
            // UserIDText
            // 
            UserIDText.AutoSize = true;
            UserIDText.Location = new System.Drawing.Point(40, 267);
            UserIDText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            UserIDText.Name = "UserIDText";
            UserIDText.Size = new System.Drawing.Size(57, 20);
            UserIDText.TabIndex = 42;
            UserIDText.Text = "UserID:";
            // 
            // UserIDTextBox
            // 
            UserIDTextBox.Location = new System.Drawing.Point(100, 261);
            UserIDTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            UserIDTextBox.MaxLength = 40;
            UserIDTextBox.Name = "UserIDTextBox";
            UserIDTextBox.Size = new System.Drawing.Size(163, 27);
            UserIDTextBox.TabIndex = 43;
            UserIDTextBox.Text = "1111@gmail.com";
            UserIDTextBox.WordWrap = false;
            // 
            // UserPWTextBox
            // 
            UserPWTextBox.Location = new System.Drawing.Point(308, 262);
            UserPWTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            UserPWTextBox.MaxLength = 20;
            UserPWTextBox.Name = "UserPWTextBox";
            UserPWTextBox.Size = new System.Drawing.Size(111, 27);
            UserPWTextBox.TabIndex = 45;
            UserPWTextBox.Text = "test1111@";
            UserPWTextBox.WordWrap = false;
            // 
            // UserPWText
            // 
            UserPWText.AutoSize = true;
            UserPWText.Location = new System.Drawing.Point(270, 267);
            UserPWText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            UserPWText.Name = "UserPWText";
            UserPWText.Size = new System.Drawing.Size(35, 20);
            UserPWText.TabIndex = 44;
            UserPWText.Text = "PW:";
            UserPWText.Click += UserPWText_Click;
            // 
            // btnLogin
            // 
            btnLogin.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btnLogin.Location = new System.Drawing.Point(545, 258);
            btnLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(89, 43);
            btnLogin.TabIndex = 46;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += button_Login_Click;
            // 
            // Room
            // 
            Room.Controls.Add(button3);
            Room.Controls.Add(btnRoomChat);
            Room.Controls.Add(textBoxRoomSendMsg);
            Room.Controls.Add(listBoxRoomChatMsg);
            Room.Controls.Add(listBoxRoomUserList);
            Room.Controls.Add(btn_RoomLeave);
            Room.Controls.Add(btn_RoomEnter);
            Room.Controls.Add(textBoxRoomNumber);
            Room.Controls.Add(label3);
            Room.Location = new System.Drawing.Point(16, 405);
            Room.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Room.Name = "Room";
            Room.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Room.Size = new System.Drawing.Size(636, 314);
            Room.TabIndex = 47;
            Room.TabStop = false;
            Room.Text = "Room";
            // 
            // button3
            // 
            button3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            button3.Location = new System.Drawing.Point(419, 28);
            button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(166, 47);
            button3.TabIndex = 57;
            button3.Text = "Game Ready";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // btnMatching
            // 
            btnMatching.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btnMatching.Location = new System.Drawing.Point(520, 322);
            btnMatching.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnMatching.Name = "btnMatching";
            btnMatching.Size = new System.Drawing.Size(124, 34);
            btnMatching.TabIndex = 54;
            btnMatching.Text = "Matching";
            btnMatching.UseVisualStyleBackColor = true;
            btnMatching.Click += btnMatching_Click_1;
            // 
            // btnRoomChat
            // 
            btnRoomChat.Font = new System.Drawing.Font("맑은 고딕", 9F);
            btnRoomChat.Location = new System.Drawing.Point(560, 275);
            btnRoomChat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnRoomChat.Name = "btnRoomChat";
            btnRoomChat.Size = new System.Drawing.Size(64, 29);
            btnRoomChat.TabIndex = 53;
            btnRoomChat.Text = "chat";
            btnRoomChat.UseVisualStyleBackColor = true;
            btnRoomChat.Click += btnRoomChat_Click;
            // 
            // textBoxRoomSendMsg
            // 
            textBoxRoomSendMsg.Location = new System.Drawing.Point(14, 277);
            textBoxRoomSendMsg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxRoomSendMsg.MaxLength = 50;
            textBoxRoomSendMsg.Name = "textBoxRoomSendMsg";
            textBoxRoomSendMsg.Size = new System.Drawing.Size(538, 27);
            textBoxRoomSendMsg.TabIndex = 52;
            textBoxRoomSendMsg.Text = "test1";
            textBoxRoomSendMsg.WordWrap = false;
            textBoxRoomSendMsg.TextChanged += textBoxRoomSendMsg_TextChanged;
            // 
            // listBoxRoomChatMsg
            // 
            listBoxRoomChatMsg.FormattingEnabled = true;
            listBoxRoomChatMsg.Location = new System.Drawing.Point(180, 83);
            listBoxRoomChatMsg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxRoomChatMsg.Name = "listBoxRoomChatMsg";
            listBoxRoomChatMsg.Size = new System.Drawing.Size(448, 184);
            listBoxRoomChatMsg.TabIndex = 51;
            listBoxRoomChatMsg.SelectedIndexChanged += listBoxRoomChatMsg_SelectedIndexChanged;
            // 
            // listBoxRoomUserList
            // 
            listBoxRoomUserList.FormattingEnabled = true;
            listBoxRoomUserList.Location = new System.Drawing.Point(14, 83);
            listBoxRoomUserList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxRoomUserList.Name = "listBoxRoomUserList";
            listBoxRoomUserList.Size = new System.Drawing.Size(157, 184);
            listBoxRoomUserList.TabIndex = 49;
            // 
            // btn_RoomLeave
            // 
            btn_RoomLeave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btn_RoomLeave.Location = new System.Drawing.Point(282, 28);
            btn_RoomLeave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btn_RoomLeave.Name = "btn_RoomLeave";
            btn_RoomLeave.Size = new System.Drawing.Size(85, 43);
            btn_RoomLeave.TabIndex = 48;
            btn_RoomLeave.Text = "Leave";
            btn_RoomLeave.UseVisualStyleBackColor = true;
            btn_RoomLeave.Click += btn_RoomLeave_Click;
            // 
            // btn_RoomEnter
            // 
            btn_RoomEnter.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btn_RoomEnter.Location = new System.Drawing.Point(189, 28);
            btn_RoomEnter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btn_RoomEnter.Name = "btn_RoomEnter";
            btn_RoomEnter.Size = new System.Drawing.Size(85, 43);
            btn_RoomEnter.TabIndex = 47;
            btn_RoomEnter.Text = "Enter";
            btn_RoomEnter.UseVisualStyleBackColor = true;
            btn_RoomEnter.Click += btn_RoomEnter_Click;
            // 
            // textBoxRoomNumber
            // 
            textBoxRoomNumber.Location = new System.Drawing.Point(130, 31);
            textBoxRoomNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxRoomNumber.MaxLength = 6;
            textBoxRoomNumber.Name = "textBoxRoomNumber";
            textBoxRoomNumber.Size = new System.Drawing.Size(48, 27);
            textBoxRoomNumber.TabIndex = 44;
            textBoxRoomNumber.Text = "0";
            textBoxRoomNumber.WordWrap = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(10, 40);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(112, 20);
            label3.TabIndex = 43;
            label3.Text = "Room Number:";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Peru;
            panel1.Location = new System.Drawing.Point(670, 20);
            panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(777, 1095);
            panel1.TabIndex = 57;
            panel1.Paint += panel1_Paint;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            // 
            // ApiGroup
            // 
            ApiGroup.Controls.Add(GamePortTextBox);
            ApiGroup.Controls.Add(GamePortText);
            ApiGroup.Controls.Add(checkBoxLocalHostIPGame);
            ApiGroup.Controls.Add(GameIPTextBox);
            ApiGroup.Controls.Add(GameIPText);
            ApiGroup.Controls.Add(HivePortTextBox);
            ApiGroup.Controls.Add(HivePortText);
            ApiGroup.Controls.Add(checkBoxLocalHostIPHive);
            ApiGroup.Controls.Add(HiveIPTextBox);
            ApiGroup.Controls.Add(HiveIPText);
            ApiGroup.Location = new System.Drawing.Point(15, 117);
            ApiGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ApiGroup.Name = "ApiGroup";
            ApiGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ApiGroup.Size = new System.Drawing.Size(639, 131);
            ApiGroup.TabIndex = 30;
            ApiGroup.TabStop = false;
            ApiGroup.Text = "API Server 설정";
            ApiGroup.Enter += groupBox1_Enter;
            // 
            // GamePortTextBox
            // 
            GamePortTextBox.Location = new System.Drawing.Point(384, 81);
            GamePortTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GamePortTextBox.MaxLength = 6;
            GamePortTextBox.Name = "GamePortTextBox";
            GamePortTextBox.Size = new System.Drawing.Size(64, 27);
            GamePortTextBox.TabIndex = 23;
            GamePortTextBox.Text = "5022";
            GamePortTextBox.WordWrap = false;
            GamePortTextBox.TextChanged += textBox3_TextChanged;
            // 
            // GamePortText
            // 
            GamePortText.AutoSize = true;
            GamePortText.Location = new System.Drawing.Point(306, 90);
            GamePortText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            GamePortText.Name = "GamePortText";
            GamePortText.Size = new System.Drawing.Size(77, 20);
            GamePortText.TabIndex = 22;
            GamePortText.Text = "포트 번호:";
            // 
            // checkBoxLocalHostIPGame
            // 
            checkBoxLocalHostIPGame.AutoSize = true;
            checkBoxLocalHostIPGame.Checked = true;
            checkBoxLocalHostIPGame.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxLocalHostIPGame.Location = new System.Drawing.Point(459, 80);
            checkBoxLocalHostIPGame.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxLocalHostIPGame.Name = "checkBoxLocalHostIPGame";
            checkBoxLocalHostIPGame.Size = new System.Drawing.Size(127, 24);
            checkBoxLocalHostIPGame.TabIndex = 21;
            checkBoxLocalHostIPGame.Text = "localhost 사용";
            checkBoxLocalHostIPGame.UseVisualStyleBackColor = true;
            checkBoxLocalHostIPGame.CheckedChanged += checkBoxLocalHostIPGame_CheckedChanged;
            // 
            // GameIPTextBox
            // 
            GameIPTextBox.Location = new System.Drawing.Point(182, 78);
            GameIPTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GameIPTextBox.MaxLength = 30;
            GameIPTextBox.Name = "GameIPTextBox";
            GameIPTextBox.Size = new System.Drawing.Size(111, 27);
            GameIPTextBox.TabIndex = 20;
            GameIPTextBox.Text = "127.0.0.1";
            GameIPTextBox.WordWrap = false;
            // 
            // GameIPText
            // 
            GameIPText.AutoSize = true;
            GameIPText.Location = new System.Drawing.Point(64, 81);
            GameIPText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            GameIPText.Name = "GameIPText";
            GameIPText.Size = new System.Drawing.Size(113, 20);
            GameIPText.TabIndex = 19;
            GameIPText.Text = "[Game API] IP :";
            // 
            // HivePortTextBox
            // 
            HivePortTextBox.Location = new System.Drawing.Point(384, 35);
            HivePortTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            HivePortTextBox.MaxLength = 6;
            HivePortTextBox.Name = "HivePortTextBox";
            HivePortTextBox.Size = new System.Drawing.Size(64, 27);
            HivePortTextBox.TabIndex = 18;
            HivePortTextBox.Text = "5092";
            HivePortTextBox.WordWrap = false;
            HivePortTextBox.TextChanged += HivePortTextBox_TextChanged;
            // 
            // HivePortText
            // 
            HivePortText.AutoSize = true;
            HivePortText.Location = new System.Drawing.Point(306, 44);
            HivePortText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            HivePortText.Name = "HivePortText";
            HivePortText.Size = new System.Drawing.Size(77, 20);
            HivePortText.TabIndex = 17;
            HivePortText.Text = "포트 번호:";
            // 
            // checkBoxLocalHostIPHive
            // 
            checkBoxLocalHostIPHive.AutoSize = true;
            checkBoxLocalHostIPHive.Checked = true;
            checkBoxLocalHostIPHive.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxLocalHostIPHive.Location = new System.Drawing.Point(459, 34);
            checkBoxLocalHostIPHive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxLocalHostIPHive.Name = "checkBoxLocalHostIPHive";
            checkBoxLocalHostIPHive.Size = new System.Drawing.Size(127, 24);
            checkBoxLocalHostIPHive.TabIndex = 15;
            checkBoxLocalHostIPHive.Text = "localhost 사용";
            checkBoxLocalHostIPHive.UseVisualStyleBackColor = true;
            checkBoxLocalHostIPHive.CheckedChanged += checkBoxLocalHostIPHive_CheckedChanged;
            // 
            // HiveIPTextBox
            // 
            HiveIPTextBox.Location = new System.Drawing.Point(182, 32);
            HiveIPTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            HiveIPTextBox.MaxLength = 30;
            HiveIPTextBox.Name = "HiveIPTextBox";
            HiveIPTextBox.Size = new System.Drawing.Size(111, 27);
            HiveIPTextBox.TabIndex = 11;
            HiveIPTextBox.Text = "127.0.0.1";
            HiveIPTextBox.WordWrap = false;
            // 
            // HiveIPText
            // 
            HiveIPText.AutoSize = true;
            HiveIPText.Location = new System.Drawing.Point(74, 38);
            HiveIPText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            HiveIPText.Name = "HiveIPText";
            HiveIPText.Size = new System.Drawing.Size(103, 20);
            HiveIPText.TabIndex = 10;
            HiveIPText.Text = "[Hive API] IP :";
            // 
            // btnRegister
            // 
            btnRegister.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btnRegister.Location = new System.Drawing.Point(438, 258);
            btnRegister.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new System.Drawing.Size(89, 43);
            btnRegister.TabIndex = 58;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // GamedataText1
            // 
            GamedataText1.AutoSize = true;
            GamedataText1.Location = new System.Drawing.Point(26, 25);
            GamedataText1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            GamedataText1.Name = "GamedataText1";
            GamedataText1.Size = new System.Drawing.Size(62, 20);
            GamedataText1.TabIndex = 59;
            GamedataText1.Text = "[userID]";
            GamedataText1.Click += label1_Click;
            // 
            // DataUserId
            // 
            DataUserId.AutoSize = true;
            DataUserId.Location = new System.Drawing.Point(32, 51);
            DataUserId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DataUserId.Name = "DataUserId";
            DataUserId.Size = new System.Drawing.Size(50, 20);
            DataUserId.TabIndex = 60;
            DataUserId.Text = "userId";
            // 
            // GameDataGroup
            // 
            GameDataGroup.Controls.Add(label6);
            GameDataGroup.Controls.Add(DataExp);
            GameDataGroup.Controls.Add(label4);
            GameDataGroup.Controls.Add(DataDraw);
            GameDataGroup.Controls.Add(label2);
            GameDataGroup.Controls.Add(DataLose);
            GameDataGroup.Controls.Add(label5);
            GameDataGroup.Controls.Add(DataWin);
            GameDataGroup.Controls.Add(label1);
            GameDataGroup.Controls.Add(DataLevel);
            GameDataGroup.Controls.Add(GamedataText1);
            GameDataGroup.Controls.Add(DataUserId);
            GameDataGroup.Location = new System.Drawing.Point(15, 311);
            GameDataGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GameDataGroup.Name = "GameDataGroup";
            GameDataGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GameDataGroup.Size = new System.Drawing.Size(483, 86);
            GameDataGroup.TabIndex = 30;
            GameDataGroup.TabStop = false;
            GameDataGroup.Text = "게임 정보";
            GameDataGroup.Enter += GameDataGroup_Enter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(108, 25);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(43, 20);
            label6.TabIndex = 69;
            label6.Text = "[exp]";
            // 
            // DataExp
            // 
            DataExp.AutoSize = true;
            DataExp.Location = new System.Drawing.Point(108, 51);
            DataExp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DataExp.Name = "DataExp";
            DataExp.Size = new System.Drawing.Size(33, 20);
            DataExp.TabIndex = 70;
            DataExp.Text = "exp";
            DataExp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(414, 25);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(34, 20);
            label4.TabIndex = 67;
            label4.Text = "[무]";
            // 
            // DataDraw
            // 
            DataDraw.AutoSize = true;
            DataDraw.Location = new System.Drawing.Point(408, 51);
            DataDraw.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DataDraw.Name = "DataDraw";
            DataDraw.Size = new System.Drawing.Size(42, 20);
            DataDraw.TabIndex = 68;
            DataDraw.Text = "draw";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(350, 25);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(34, 20);
            label2.TabIndex = 65;
            label2.Text = "[패]";
            // 
            // DataLose
            // 
            DataLose.AutoSize = true;
            DataLose.Location = new System.Drawing.Point(344, 51);
            DataLose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DataLose.Name = "DataLose";
            DataLose.Size = new System.Drawing.Size(36, 20);
            DataLose.TabIndex = 66;
            DataLose.Text = "lose";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(270, 25);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(34, 20);
            label5.TabIndex = 63;
            label5.Text = "[승]";
            // 
            // DataWin
            // 
            DataWin.AutoSize = true;
            DataWin.Location = new System.Drawing.Point(266, 51);
            DataWin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DataWin.Name = "DataWin";
            DataWin.Size = new System.Drawing.Size(33, 20);
            DataWin.TabIndex = 64;
            DataWin.Text = "win";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(186, 25);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(50, 20);
            label1.TabIndex = 61;
            label1.Text = "[level]";
            // 
            // DataLevel
            // 
            DataLevel.AutoSize = true;
            DataLevel.Location = new System.Drawing.Point(186, 51);
            DataLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            DataLevel.Name = "DataLevel";
            DataLevel.Size = new System.Drawing.Size(40, 20);
            DataLevel.TabIndex = 62;
            DataLevel.Text = "level";
            DataLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancelMatching
            // 
            btnCancelMatching.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            btnCancelMatching.Location = new System.Drawing.Point(520, 363);
            btnCancelMatching.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnCancelMatching.Name = "btnCancelMatching";
            btnCancelMatching.Size = new System.Drawing.Size(124, 34);
            btnCancelMatching.TabIndex = 59;
            btnCancelMatching.Text = "Cancel Match";
            btnCancelMatching.UseVisualStyleBackColor = true;
            btnCancelMatching.Click += btnCancelMatching_Click;
            // 
            // mainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1476, 1132);
            Controls.Add(btnCancelMatching);
            Controls.Add(GameDataGroup);
            Controls.Add(btnMatching);
            Controls.Add(btnRegister);
            Controls.Add(ApiGroup);
            Controls.Add(panel1);
            Controls.Add(Room);
            Controls.Add(btnLogin);
            Controls.Add(UserPWTextBox);
            Controls.Add(UserPWText);
            Controls.Add(UserIDTextBox);
            Controls.Add(UserIDText);
            Controls.Add(labelStatus);
            Controls.Add(listBoxLog);
            Controls.Add(SocketGroup);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "mainForm";
            Text = "네트워크 테스트 클라이언트";
            FormClosing += mainForm_FormClosing;
            Load += mainForm_Load;
            SocketGroup.ResumeLayout(false);
            SocketGroup.PerformLayout();
            Room.ResumeLayout(false);
            Room.PerformLayout();
            ApiGroup.ResumeLayout(false);
            ApiGroup.PerformLayout();
            GameDataGroup.ResumeLayout(false);
            GameDataGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnSocketDisconnect;
        private System.Windows.Forms.Button btnSocketConnect;
        private System.Windows.Forms.GroupBox SocketGroup;
        private System.Windows.Forms.TextBox socketPortTextBox;
        private System.Windows.Forms.Label socketPortText;
        private System.Windows.Forms.CheckBox checkBoxLocalHostIPSocket;
        private System.Windows.Forms.TextBox socketIPTextBox;
        private System.Windows.Forms.Label socketIPText;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Label UserIDText;
        private System.Windows.Forms.TextBox UserIDTextBox;
        private System.Windows.Forms.TextBox UserPWTextBox;
        private System.Windows.Forms.Label UserPWText;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.GroupBox Room;
        private System.Windows.Forms.Button btn_RoomLeave;
        private System.Windows.Forms.Button btn_RoomEnter;
        private System.Windows.Forms.TextBox textBoxRoomNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRoomChat;
        private System.Windows.Forms.TextBox textBoxRoomSendMsg;
        private System.Windows.Forms.ListBox listBoxRoomChatMsg;
        private System.Windows.Forms.ListBox listBoxRoomUserList;
        private System.Windows.Forms.Button btnMatching;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox ApiGroup;
        private System.Windows.Forms.TextBox GamePortTextBox;
        private System.Windows.Forms.Label GamePortText;
        private System.Windows.Forms.CheckBox checkBoxLocalHostIPGame;
        private System.Windows.Forms.TextBox GameIPTextBox;
        private System.Windows.Forms.Label GameIPText;
        private System.Windows.Forms.TextBox HivePortTextBox;
        private System.Windows.Forms.Label HivePortText;
        private System.Windows.Forms.CheckBox checkBoxLocalHostIPHive;
        private System.Windows.Forms.TextBox HiveIPTextBox;
        private System.Windows.Forms.Label HiveIPText;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label GamedataText1;
        private System.Windows.Forms.Label DataUserId;
        private System.Windows.Forms.GroupBox GameDataGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DataLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DataLose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label DataWin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label DataDraw;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label DataExp;
        private System.Windows.Forms.Button btnCancelMatching;
    }
}

