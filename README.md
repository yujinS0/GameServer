# GameServer
[송유진] 컴투스 서버 캠퍼스 2기

## 시퀀스 다이어그램
#### 새로운 유저의 계정 생성

```mermaid
sequenceDiagram
	actor User
	participant Game API Server
	participant Hive Server
	participant Hive Mysql
  participant Game Mysql

	User ->> Hive Server: 계정 생성
	Hive Server ->> Hive Mysql : 유저 정보 생성
  Hive Server ->> User: 계정 생성 성공 응답
```

#### 유저의 로그인
```mermaid
sequenceDiagram
	actor User
	participant Game API Server
	participant Hive Server
	participant Hive Mysql
  participant Game Mysql
  participant Hive Redis
  participant Game Redis

	User ->> Hive Server: 하이브 로그인 요청
  Hive Server ->> Hive Mysql : 회원 정보 요청
  Hive Mysql ->> Hive Server : 회원 정보 응답
	Hive Server -->> User : 로그인 실패 응답

  alt 로그인 성공
  Hive Server ->> Hive Redis : 고유번호와 토큰 저장
  Hive Server ->> User : 로그인 성공 응답 (고유번호와 토큰)
  end

	User ->> Game API Server : 고유번호와 토큰을 통해 로그인 요청
	Game API Server ->> Hive Server : 토큰 유효성 확인 요청
	Hive Server ->> Hive Redis : 고유번호와 토큰 정보 확인
	Hive Redis -->> Hive Server : 응답
	Hive Server ->> Game API Server : 토큰 유효 여부 응답

	Game API Server -->> User : 로그인 실패 응답
	
	Game API Server ->> Game Redis : 고유번호와 게임용 토큰 저장
	Game Redis -->> Game API Server : 고유번호와 게임용 토큰 응답

	alt 첫 로그인이면
	Game API Server ->> Game Mysql : 유저 게임 데이터 생성
	end
	Game Mysql -->> Game API Server : 유저 데이터 로드

	Game API Server ->> User : 로그인 성공 응답 (고유번호와 게임용 토큰 + 유저 게임 데이터 정보)
```


## DB 스키마
#### HiveServer

```sql
  use hivedb;

  CREATE TABLE account (
      userid INT AUTO_INCREMENT PRIMARY KEY,
      email VARCHAR(255) NOT NULL UNIQUE,
      password CHAR(64) NOT NULL,
      salt CHAR(64) NOT NULL
  );
```

#### GameAPIServer

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

## 공부 문서 
[ASP.NET core 문서](https://github.com/yujinS0/ASP.NETcore-Study)

---
### 2주차 체크박스
#### API 기능 구현
- [X] 계정생성 (hive server)
- [X] 로그인
  + Hive 서버 로그인 (hive server)
  + API Game 서버 로그인 (api game server)
    * 첫 로그인 시 기본 게임 데이터 생성
  + 인증 토큰 유효 검증 (hive server)
- [ ] 우편함
- [ ] 출석부
- [ ] 친구




#### 피드백
- [X] `program.cs`
  + 데이터베이스 서비스 등록할 때 class-interface 형식으로 받아오게해서 등록
- [X] `model`
  + Req/Res : success & message 제거 -> ErrorCode로 처리
    * 관련해서 controller의 로직들도 대폭 수정 필요!
- [X] `controller`
  + <IActionResult> 이렇게 보내는 경우 수정 : Req/Res 타입 바로 넣게
  + 로직 부분 여기서 처리하지 말기
    * controller / service / repository 적절히 분리하기!!
- [X] `repository`
  + 쿼리문 사용한 경우 -> KATA 사용으로 수정하기
  + 쿼리문에서 함수 처리하지 말기 -> server에서 처리하기
  + Redis 관련 라이브러리 CloudStructures로 변경하기
- [ ] 로그 관련 처리
  + 로그 레벨 확인 후 처리
