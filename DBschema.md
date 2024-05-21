# HiveServer

```sql
  use hivedb;
  
  CREATE TABLE account (
      userNum INT AUTO_INCREMENT PRIMARY KEY,
      userId VARCHAR(255) NOT NULL UNIQUE,
      password CHAR(64) NOT NULL,  -- SHA-256 해시 결과는 항상 64 길이의 문자열
      salt CHAR(64) NOT NULL
  );
```

# GameAPIServer

```sql
  use gamedb;
  
  CREATE TABLE UserGameData (
      UserNum INT NOT NULL,
      UserId VARCHAR(255) NOT NULL UNIQUE,
      Level INT NOT NULL DEFAULT 1,
      Exp INT NOT NULL DEFAULT 0,
      Win INT NOT NULL DEFAULT 0,
      Lose INT NOT NULL DEFAULT 0,
      Draw INT NOT NULL DEFAULT 0,
      PRIMARY KEY (UserId)
  );
```

### 출석부
```sql
  CREATE TABLE Attendance (
    AttendanceId INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    Date DATE NOT NULL,
    UNIQUE (UserId, Date)
);
```

### 인벤토리
```sql
  CREATE TABLE Inventory (
    UserId VARCHAR(255) NOT NULL,
    ItemId INT NOT NULL,
    PRIMARY KEY (UserId, ItemId)
);
```

### 우편함
```sql
CREATE TABLE Mailbox (
    MailId INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Content TEXT NOT NULL,
    SentDate DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

### 친구
```sql
  CREATE TABLE Friends (
    UserId1 VARCHAR(255) NOT NULL,
    UserId2 VARCHAR(255) NOT NULL,
    PRIMARY KEY (UserId1, UserId2)
);
```
