Day1

1. 사용자는 로그인 정보 입력후 처리를 요청
2. Controller는 ViewModel을 입력받아 로그인 처리한 후 View에 ViewModel 전달
* ViewModel은 이동수단의 역할
3. View는 ViewModel을 통해 화면을 구성

End User(최종사용자) => 컨트롤러에게 요청

Controller(컨트롤러) => 최종사용자에게 응답

View(뷰) => 응답결과를 화면으로 출력

Model(뷰모델) => 최종사용자로부터 컨트롤러를 거쳐 뷰까지 데이터 이동


Day2

닷넷코어의 두가지 패턴에 대한 복습

mvc는 각자가 맡은 역할을 다할 수 있도록 분리가 되어 있습니다.

의존성 주입도 클래스 인스턴스를 직접 받지 않고 생성자의 파라미터를 통해 주입받아 사용됩니다.

닷넷코어의 키워드는 "분리"입니다.

Day3
1. code-first 코드작성 우선주의
2. Migration 미리 작성된 코드로 데이터베이스에 테이블과 컬럼 생성
3. Database-First 데이터베이스 작업 우선주의
4. Entity Data Modeling 코드를 손쉽게 작성할 수 있도록 도와줌

Code-First 

장점
1. Table과 Column을 Application에서 관리
2. Migrations를 통한 이력관리

단점
1. 사소한 작업을 Migrations하는 것이 번거로움
2. 운영서버에 바로 적용이 어려움

Day4

사용자 아이디는 기본키로 지정됨
사용자 이메일은 유니크 인덱스로 지정됨
그래서 중복체크할 때 사용자 목록 검색이 불필요
소스코드에서 로깅을 하거나 예외사항 처리

Database-First

장점
1. 데이터베이스 작업은 기존과 동일하게 수행가능
2. Entity data modelling으로 손코딩 거의 없음
   
단점
1. 데이터베이스 작업의 이력관리를 하지 못함
2. 테이블 또는 컬럼변경시 c#코드도 수동변경

마이그레이션 기록을 남길 수 있는 code first 방식을 채택하는걸로

db를 통해 데이터를 처리할 때 어지간하면 linq방식 채택 그게 아니라면 저장 프로시저 활용하여 처리

비즈니스 로직이 너무 복잡하거나 민감한 정보를 가지고있을때만 사용 + dba가 존재

Day5

ExecuteSql 메서드는 작업결과 int값을 return하기 때문, procedure에서 database의

insert, update, delete 작업 후 select구문을 추가해도 그 값을 return 할 수 없다

데이터 검색은 별도의 c# 메서드로 분리

Day6

*신원보증

ClaimsPrincipal, ClaimsIdentity

Principal: 본인, Identity: 신원

*허가승인

Authorize(Roles), ClaimTypes.Role

Role: 권한(역할)

Day7

ClaimType.Name(UserId)

=> _context.User.Identity.Name

ClaimType.Role(RoleId)

=> Authorize(Roles = "o, o, o")

ClaimType.UserData(사용자 정의 데이터)

=> RoleName, RolePriority, UserName, UserEmail

Day8

GUID Salt: 사용자 정보의 복잡성을 위해 사용

RNG Salt: 비밀번호해시 생성시 Salt의 복잡성을 위해 사용

PasswordHash: Iterations: 100000, 45000, 25000, 10000 ...

Day9

Entity Framework Core

데이터의 insert 작업 지원 => Add 메서드

데이터 단일 건 추가

_context.Add(user) 또는

_context.Add<User>(user)

SaveChanges 메서드

_context.SaveChanges() <= 변경사항 저장

데이터의 Update 작업 지원 => Update 메서드

데이터 단일 건 업데이트

_context.Update(userInfo) 또는

_context.Update<User>(userInfo)

변경할 데이터 나열

userInfo.UserName = user.UserName;

_context.SaveChanges() <= 변경사항 저장