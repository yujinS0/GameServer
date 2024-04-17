using HiveServer.Repository;
using StackExchange.Redis; // [TODO] CloudStructures 라이브러리 사용하기 

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddScoped<DatabaseService>(provider => // 여기서 매번 이렇게 하면, 스트링으로 매번 뽑아오니깐 너무 오류이다! 
//     new DatabaseService(builder.Configuration.GetConnectionString("MysqlHiveDBConnection")));
// 생성자는 스트링=커넥션 주소이다. 
// 데이터 서비스가 얘를..?
// DI 한다는 것? = 컨트롤러의 생성자+인스턴스 초기화 부분.. 즉 함수를 호출하는 것의 객체를 ASP.NET core가 생성해주는
// 즉 인터페이스가 있고 이를 구현하는 구현체가 존재한다!
// 여기서 컨트롤러에서 인터페이스 객체르
// builder.Services.AddScoped<IHiveDb>(provider => // [TODO]: 이런식으로 매번 받아오지말기! -> 완료
//     new HiveDb(
//         builder.Configuration.GetConnectionString("MysqlHiveDBConnection"),
//         provider.GetRequiredService<ILogger<HiveDb>>()
//         // Ihivedb 객체를 생성할 때 위에 내용처럼 이런 규칙으로 생성하겠다!! 라는 규칙? 
//     ));

// DbConfig 설정 로드
builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("ConnectionStrings"));

// mysql 서비스 등록
builder.Services.AddScoped<IHiveDb, HiveDb>();

// Redis configuration [TODO] 이것도 위 바꾼 코드처럼 인터페이스 활용해서 객체 만드는 방식으로 수정
var redisConfiguration = builder.Configuration.GetConnectionString("RedisHiveConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration)); 
// 싱글톤 이유? 레디스 자체의 특성 때문
// 객체를 여러개 만들지 말고, 알아서 락 해줄거니 편하게 써라~ 라는 느낌이다!


// 로깅 설정
builder.Logging.ClearProviders(); // 기본 로깅 프로바이더를 제거합니다.
builder.Logging.AddConsole(); // 콘솔 로거를 추가합니다.
builder.Logging.SetMinimumLevel(LogLevel.Information); // 최소 로깅 레벨을 Information으로 설정합니다.


builder.Services.AddControllers(); // 컨트롤러 만들기 -> 만든 객체들을 asp.net core에 등록하는 

var app = builder.Build(); // 위에는 지금까지 설정 작업을 했고, 해당 내용을 빌드하는 것!! 

// API 엔드포인트 설정
app.MapControllers(); // 컨트롤러를 기본적으로 진행할 것이다 -> 이것도 커스텀 가능! (근데 특별한 경우에만 사용)

app.Run(); // 실제로 컨트롤러를 동작하는 것! (객체가 올라와는 와있는데 이 코드를 작성해야 호출할 준비 상태가 되도록) Listen