## HiveServer

```sql
  use hivedb;

  CREATE TABLE account (
      userid INT AUTO_INCREMENT PRIMARY KEY,
      email VARCHAR(255) NOT NULL UNIQUE,
      password CHAR(64) NOT NULL,
      salt CHAR(64) NOT NULL
  );
```

## GameAPIServer

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
