# GameServer
[송유진] 컴투스 서버 캠퍼스 2기
<br><br>
## Game API Server
- [시퀀스 다이어그램](SequenceDiagram.md)
- [DB 스키마](DBschema.md)
- [ASP.NET core 공부 문서](https://github.com/yujinS0/ASP.NETcore-Study)

## Game Socket Server
- `OmokServer` 디렉토리 : 실제 오목 서버
- `OmokClient` 디렉토리 : 실제 오목 클라이언트
- 나머지 디렉토리 : 공부 및 실습 연습
  + `MyChatServer` 디렉토리 : 채팅 서버 로직을 자세한 주석과 함께 설명 (로직 헷갈릴 때 참고하기)

---
## 진행상황
### 소켓 프로그래밍
- [X] 방 입장(최대 2명까지)
- [X] 방 나가기
- [X] 방 채팅
- [X] 게임 시작
  - 두명이 모두 게임 시작을 요청하면 바로 게임 시작
- [ ] 돌두기 
  - 시간 제한을 넘으면 상대방에게 턴이 자동으로 넘겨야 됨
  - 연속으로 6번 자동으로 턴이 넘어가면 게임 취소
- [X] 오목 로직
- [X] 게임 종료
  - 게임이 끝나면 서버는 결과를 알려준다.
  - 클라이언트는 결과를 표시한다
- [ ] 로그인에 DB 연동
  - Redis에 있는 유저 정보로 로그인 체크
  - 게임 결과 DB 저장
- [ ] Heart-Beat 구현





---
### API 기능 구현
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
