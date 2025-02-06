# [Unity 2D] 카타나 제로 (모작) (팀 포트폴리오)
## 1. 소개

<div align="center">
  <img src="https://github.com/k660323/KatanaZero/blob/main/Images/%EB%A9%94%EC%9D%B8%ED%99%94%EB%A9%B4.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/KatanaZero/blob/main/Images/%ED%8C%A90.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/KatanaZero/blob/main/Images/%ED%8C%A91.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/KatanaZero/blob/main/Images/%ED%8C%A92.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/KatanaZero/blob/main/Images/%EB%B3%B4%EC%8A%A4.JPG" width="49%" height="300"/>
  
  < 게임 플레이 사진 >
</div>

+ 카타나 제로란?
  + 네온으로 점철된 펑키한 그래픽, 부드럽고 섬세한 도트, 강렬한 색감과 유혈을 자랑하는 2D 액션 게임으로, 매력적인 배경 설정과 그래픽, 강렬한 다크신스웨이브풍 사운드트랙, 화려한 게임플레이와 깊은 누아르 색의 스토리 숙련을 요하는 난이도와 무한한 도전이 가능하다는 특징
  + 스피디한 2D 횡스크롤 방식의 게임.
    
+ 목표
  + 원작의 Katana Zero의 핵심 요소들을 유사하게 구현하는 것을 목표

+ 포지션
  + 컨텐츠 코어 기능, 몬스터 AI, 투사체, 이펙트 구현, 최적화 및 팀원 어시스트 담당

+ 게임 흐름
  + 타이틀 화면에서 게임 시작 및 옵션 게임 종료 기능 구현
  + 총 4개의 씬에서 컨텐츠 진행
  + 각 씬마다 제한시간안에 모든 적을 섬멸하는 것을 목표

<br>

## 2. 프로젝트 정보

+ 사용 엔진 : UNITY
  
+ 엔진 버전 : 2022.3.10f1

+ 사용 언어 : C#
  
+ 작업 인원 : 3명
  
+ 작업 영역 : 콘텐츠 제작, 기획
  
+ 장르      : 액션 2d 횡스크롤
  
+ 소개      : 카타나 제로를 모작한 포트폴리오
  
+ 플랫폼    : PC
  
+ 개발기간  : 2024.05.01 ~ 2024.05.26
  
+ 형상관리  : GitHub Desktop

<br>

## 3. 사용 기술
| 기술 | 설명 |
|:---:|:---|
| 디자인 패턴 | ● **싱글톤** 패턴 Managers클래스에 적용하여 여러 객체 관리 <br> ● **FSM** 패턴을 사용하여 플레이어 및 AI 기능 구현 <br>
| Object Pooling | 자주 사용되는 객체는 Pool 관리하여 재사용 |

<br>

## 4. 구현 기능

### **구조 설계**

대부분 유니티 프로젝트에서 사용되고 자주 사용하는 기능들을 구현하여 싱글톤 클래스인 Managers에서 접근할 수 있도록 구현
      
### **코어 매니저**

+ Managers - 매니저들을 관리하는 매니저 클래스
  + Scene 이동시 ObjectPooling 오브젝트들을 모두 제거

[Managers.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Manager/Managers.cs)

<br>

+ ResourceManager - 리소스 매니저
  
[ResourceManager.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Manager/ResourceManager.cs)

<br>

+ SoundManager - 사운드 매니저

[SoundManager.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Manager/SoundManager.cs)

<br>
        
### **컨텐츠 매니저**

+ MapManager
  + A*를 사용하기위한 맵 정보와 관련된 기능을 제공하는 매니저

**핵심 함수**

LoadMap - 해당 씬에 맞는 데이터를 불러와 변수에 기록합니다.

FindPath -현재 맵 정보와 시작 지점, 목표 지점을 통해 최단 경로 a*를 수행합니다.

CalcCellPathFromParent - A* 결과로 나온 parent 함수를 통해 최단 경로를 만들어 반환합니다.

Cango - 해당 위치 이동 가능 여부를 확인하는 함수 입니다.
    
[MapManager.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Manager/MapManager.cs)

<br>

+ AStarLoad
  + 각 씬마다 존재하며 해당 씬 이름에 맞는 맵 데이터를 불러오도록 MapManager의 LoadMap 함수에 맵 이름을 매개변수를 넣어 호출합니다.

[AStarLoad.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Scene/AStarLoad.cs)

<br>

---

<br>

### **핵심 컨텐츠**

#### **몬스터**
+ Creature
  + 컨텐츠 핵심 클래스
  + 사용할 여러 컴포넌트 관리 및 초기화하는 매니저 클래스
  + 플레이어를 적대하는 몬스터가 사용하는 클래스
  + 일반 몬스터가 사용하는 FSM 상태 정의
  + 피격처리를 위한 IHitable 인터페이스 상속

[Creature.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Creature/Creature.cs)

+ KissfaceEnemy
  + Creature를 상속받은 클래스
  + 보스 클래스
  + 해당 보스에서 사용하는 FSM 상태 정의
  + 보스 전용 UI 함수 정의

[KissfaceEnemy](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Creature/KissfaceEnemy.cs)

<br>

---

##### **AI 로직**
+ StateMachine
  + 행위(상태)를 관리하는 컨트롤러
  + 상태 등록, 삭제, 상태 전환 및 현제 상태에 대한 로직을 수행합니다.
  + Creature를 상속받은 클래스가 상태 초기화 등록을 수행합니다.

[StateMachine.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/StateMachine/StateMachine.cs)

<br>

+ BaseState
  + StateMachine이 내부적으로 사용하는 클래스
  + 상태에 대한 상세한 기능을 구현한 클래스
  + 상태 진입 조건, 상태 진입, 상태 진행중, 상태 변경에 따른 가상 함수 구현

BaseState 함수
+ 생성자 - 제어할 객체와 컨트롤러를 초기화 한다.
+ CheckConditon() - 상태 전환 여부를 확인 하는 함수 bool형을 반환한다.
+ EnterState() - 해당 상태 전환시 한번 호출되는 함수 초기화를 담당한다.
+ ExitState() - 다음 상태로 전환시 현재 상태에 대한 초기화하는 함수
+ FixedState() - 현재 상태에 대한 물리적인 로직을 수행하는 함수로 매 고정된 프레임 단위로 호출된다.
+ UpdateState() - 현재 상태에 대한 입력 로직을 수행하는 함수 매 프레임 단위로 호출된다.


[BaseState.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/State/BaseState.cs)

**상태를 구현시 모든 클래스는 BaseState를 상속받아 구현 한다.**

[State 폴더](https://github.com/k660323/KatanaZero/tree/main/Scripts/Contents/State)

<br>

---

<br>

#### **Controller**
+ Controller
  + Creature클래스 지닌 오브젝트의 방향, 위치, 플레이어 정보, 스폰 위치, 추적 위치 같은 State에서 사용할 공용 데이터를 관리하는 클래스

[Controller.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Controller/Controller.cs)

<br>

---

<br>

#### **능력치**
+ Stat
  + Creature의 능력치를 관리하는 클래스 (moveSpeed, detecteRange...)

[Stat.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Stat/Stat.cs)

<br>

---

<br>

#### **무기**
+ Weapon
  + Creature가 사용할 무기를 정의하는 클래스
  + Weapon클래스를 상속받는 근거리 무기 Melee, 원거리 무기 Range, 공통적인 부분은 Weapon 세세한 부분은 자식 클래스에서 구현
    
[Weapon.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Weapon/Weapon.cs)

[Melee.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Weapon/Melee.cs)

[Range.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Weapon/Range.cs)
    
<br>

---

<br>

#### **투사체**
+ Projectile
  + 원거리 발사 오브젝트들을 해당 클래스를 상속받아 구현

[Porjectile.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Range/Projectile.cs)

+ Bullet
  + Projectile을 상속받으며 일정시간동안 목표물에 피격할때 까지 해당 방향으로 이동하는 오브젝트
  + IReflectable을 상속받아 플레이어가 타이밍에 맞게 공격시 Bullet을 반대 방향으로 전환시킬 수 있다. 

[Bullet.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Range/Bullet.cs)

+ Axe
  + KissfaceEnemy 보스 몬스터가 사용하는 투척용 도끼
  + Projectile을 상속받으며 일정 거리를 이동 후 다시 돌아오는 오브젝트

[Axe.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Range/Axe.cs)

+ JumpSwingAxe
  + KissfaceEnemy 보스 몬스터가 사용하는 회전 도끼
  + Projectile을 상속받으며 JumpSwingAxeHit(도끼)를 소환하며 보스 몬스터를 주위를 일정시간 동안 회전 하는 오브젝트

[JumpSwingAxe](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/Range/JumpSwingAxe.cs)

<br>

---

<br>

#### **애니메이션 이벤트**
+ AiAnimationEvent
  + 애니메이션에 맞게 이벤트를 발생시키고자 할때 사용하는 클래스
  + 근접 판정 시작과 끝, 원거리 공격, 공격 애니메이션 끝, 장전 콜백함수 구현

[AiAnimationEvent.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/AnimationEvent/AiAnimationEvent.cs)

<br>

+ KissfaceAnimationEvent
  + 보스 클래스에서 사용하는 애니메이션 이벤트
  + AiAnimationEvent을 상속받아 확장
  + 무기 던지기, 무기 잡기 등 추가 행위 작성

[KissfaceAnimationEvent.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Contents/AnimationEvent/KissfaceAnimationEvent.cs)

<br>

---

<br>

#### **기타**
+ Define
  + StateMahcine에서 현재 State를 쉽게 구분 하기 위해 enum 자료형의 State 정의
    
[Define.cs](https://github.com/k660323/KatanaZero/blob/main/Scripts/Define.cs)

<br>

---

<br>

## 5. 구현에 어려웠던 점과 해결과정
+ 구현에 어려움보단 팀 포트폴리오이기에 협동 작업 역할 분담 및 각자 만든 작업물의 상호작용, 또는 깃 허브 연동하는게 어려웠다. 왜냐하면 씬, 태그, 레이어, 같은 게임 오브젝트를 수정하면 그 값이 덮혀버리기 때문에 소통이 되지 않으면 그 날 작업한 내용이 날아가서 난감했다. 
  + 씬은 씬 담당자만이 작업을 하도록하고 만약 변경사항이 있으면 담당자에게 전달사항을 전달해 수정하도록 하였다.

+ 각자 만든 작업물을 매끄럽게 연결 시킬지 몰라 많은 어려움을 겪었습니다.
  + 각자 만든 작업물은 인터페이스 클래스를 만들어서 각자 만든 작업물 에서 호출하여 상호작용 하도록 작성하였다. ex) 피격 인터페이스, 총알 반사 함수 
  

## 6. 느낀점
+ 처음 해본 팀플과 GitHub 문제로 기간내에 해결할 수 있을지 걱정이였지만 팀원분들이 자기가 맡은 역할을 잘 수행해주어서 좋았으나 팀원간 의사소통을 적극적으로 하지 않아서 아쉽습니다. 다음 팀 작업 기회가 오다면 적극적인 자세로 작업물에 대한 피드백을 많이 해야겠습니다.

## 7. 플레이 영상
+ https://www.youtube.com/watch?v=TcZmbVpudik
