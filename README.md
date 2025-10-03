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
