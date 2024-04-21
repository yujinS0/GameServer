# 시퀀스 다이어그램
## 새로운 유저의 계정 생성

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

## 유저의 로그인
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
  Hive Server ->> User : 고유번호와 토큰 응답
  end









```

