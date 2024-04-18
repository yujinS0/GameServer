# GameServer
[송유진] 컴투스 서버 캠퍼스 2기

## 수정해야하는 부분
- `program.cs` 데이터베이스 서비스 등록할 때 class-interface 형식으로 받아오기
- 로그 레벨 확인해서 처리하기
- `controller` Req/Res : success & message 제거하고 ErrorCode로 처리하기
  + <IActionResult> 이렇게 보내는 경우 수정하기 : Req/Res 타입 바로 넣게
  + 로직 부분 여기서 처리하지 말기
- `repository` 쿼리문 사용한 경우 KATA 사용으로 수정하기
  + 쿼리문에서 함수 처리하지 말기
  + Redis 관련 라이브러리 CloudStructures로 변경하기
