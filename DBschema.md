# HiveServer

```sql
  use hivedb;

  CREATE TABLE account (
      userid INT AUTO_INCREMENT PRIMARY KEY,
      email VARCHAR(255) NOT NULL UNIQUE,
      password CHAR(64) NOT NULL,
      salt CHAR(64) NOT NULL
  );
```

# GameAPIServer

```sql
  use gamedb;

  CREATE TABLE UserGameData (
      UserId VARCHAR(255) NOT NULL,
      Level INT NOT NULL DEFAULT 1,
      Exp INT NOT NULL DEFAULT 0,
      Win INT NOT NULL DEFAULT 0,
      Lose INT NOT NULL DEFAULT 0,
      PRIMARY KEY (UserId)
  );
```

### 출석부
```sql
  CREATE TABLE Attendance (
    AttendanceId INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Date DATE NOT NULL,
    UNIQUE (UserId, Date)
);
```

### 인벤토리
```sql
  CREATE TABLE Inventory (
    UserId INT NOT NULL,
    ItemId INT NOT NULL,
    PRIMARY KEY (UserId, ItemId)
);
```

### 우편함
```sql
CREATE TABLE Mailbox (
    MailId INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Content TEXT NOT NULL,
    SentDate DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

### 친구
```sql
  CREATE TABLE Friends (
    UserId1 INT NOT NULL,
    UserId2 INT NOT NULL,
    PRIMARY KEY (UserId1, UserId2)
);
```
