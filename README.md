# [Unity 2D] 카타나 제로 (모작) (팀 포트폴리오)
## 1. 소개

<div align="center">
  <img src="https://github.com/k660323/BossHunter/blob/main/Images/%EB%A9%94%EC%9D%B8%20%ED%99%94%EB%A9%B4.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/BossHunter/blob/main/Images/%EC%84%A0%ED%83%9D%EC%B0%BD.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/BossHunter/blob/main/Images/%EB%A7%88%EC%9D%84.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/BossHunter/blob/main/Images/%EB%8D%98%EC%A0%84%20%EC%84%A0%ED%83%9D%EC%B0%BD.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/BossHunter/blob/main/Images/%EB%8D%98%EC%A0%84.JPG" width="49%" height="300"/>
  <img src="https://github.com/k660323/BossHunter/blob/main/Images/%EB%B3%B4%EC%8A%A4.JPG" width="49%" height="300"/>
  
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
| 디자인 패턴 | ● **싱글톤** 패턴 Managers클래스에 적용하여 여러 객체 관리 <br> ● **FSM** 패턴을 사용하여 플레이어 및 AI 기능 구현 <br> ● **옵저버** 패턴을 사용하여 플레이어 상태, 스킬 상태를 변경시에만 UI 업데이트|
| Object Pooling | 자주 사용되는 객체는 Pool 관리하여 재사용 |

<br>

## 4. 구현 기능

### **구조 설계**

대부분 유니티 프로젝트에서 사용되고 자주 사용하는 기능들을 구현하여 싱글톤 클래스인 Managers에서 접근할 수 있도록 구현
      
#### **코어 매니저**

+ Managers - 모든 매니저들을 관리하는 매니저 클래스 및 미러 콜백 함수, 미러 커스텀 메시지 정의 및 처리하는 클래스
+ DataManager - 데이터 관리 매니저
+ InputManager - 사용자 입력 관리 매니저
+ PoolManager - 오브젝트 풀링 매니저
+ ResourceManager - 리소스 매니저
+ SceneManager - 씬 매니저
+ SoundManager - 사운드 매니저
+ UIManager - UI 매니저

        
#### **컨텐츠 매니저**

+ GameManager
  + 월드 아이템을 스폰하는 매니저 클래스

+ OptionManager
  + 게임 해상도, 그래픽 품질, 사운드, 마우스 감도 값들을 관리하는 매니저
  + Json파일로 데이터를 저장 및 불러옵니다.
  + UI_Preferences클래스에서 UI로 환경 설정하면 값이 반영됩니다.
 
+ AnimatorHashMaanger
  + 유니티의 Animator에서 String으로 파라미터를 접근할 수 있는데 내부적으로 String을 해쉬값으로 변환하는 과정을 거치기에 미리 사용할 데이터를 해쉬값으로 변환하여 런타임에 바로 사용하기 위한 매니저 클래스
 
+ LayerManager
  + Layer의 비트마스크를 미리 계산해 캐싱해놓은 매니저 클래스
         
[Managers.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Managers/Managers.cs)


<br>

---

<br>

### **핵심 컨텐츠**

#### **생명체**
+ Creature
  + 컨텐츠 핵심 클래스
  + 사용할 여러 컴포넌트 관리 및 초기화하는 매니저 클래스
  + Player, Monster 클래스의 부모 클래스
[Creature.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Creature.cs)

<br>
<br>

##### **플레이어**
+ Player
  + 플레이어 캐릭터의 기초가 클래스
  + 로컬 플레이어가 쉽게 사용할 수 있도록 클라이언트 한정 싱글톤 구현
  + 컨트롤러, 파티, 클라이언트 전용 UI, 인벤토리 등 플레이어가 사용할 컴포넌트 관리 및 초기화

[Player.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Player/Player.cs)

+ Warrior, Archor
  + Player를 상속받은 클래스
  + FSM, PlayerType 정의 및 초기화
  
[Warrior.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Player/Player.cs)
  
[Archor.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Player/Archer.cs)

<br>
<br>

##### **몬스터**
+ Monster
  + 몬스터의 기초가 되는 클래스
  + 몬스터 컨트롤러, 스텟, 상태머신, 삭제 액션 등 컴포넌트 관리 및 초기화

[Monster.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Monster/Monster.cs)

<br>

+ DefaultMonster
  + 일반 몬스터가 사용하는 클래스
  + 피격판정을 처리하는 IHitable, 아이템을 드랍하는 IDropable 인터페이스를 상속받아 구현하고 있습니다.

[DefaultMonster.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Monster/DefaultMonster.cs)

<br>

+ FlowerDryad
  + 보스 몬스터가 사용하는 클래스
  + 피격판정을 처리하는 IHitable, 아이템을 드랍하는 IDropable 인터페이스를 상속받아 구현하고 있습니다.
  + 콜라이더 정보를 처리하는 ICollider 인터페이스를 추가로 상속받아 구현하고 있습니다.
  + Skill State가 추가로 등록되어 일정 확률로 스킬을 사용합니다.

[FlowerDryad](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Creature/Monster/FlowerDryad.cs)

<br>

---

<br>

#### **StateMachine**
+ StateMachine
  + FSM 방식으로 구현
  + 행위(상태)를 관리하는 컨트롤러

[StateMachine.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/StateMachine/StateMachine.cs)

+ BaseState
  + StateMachine이 내부적으로 사용하는 클래스
  + 상태에 대한 상세한 기능을 구현한 클래스
  + 상태 진입 조건, 상태 진입, 상태 진행중, 상태 변경에 따른 가상 함수 구현
    
[BaseState](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/StateMachine/BaseState/BaseState.cs)

**상태를 구현시 모든 클래스는 BaseState를 상속받아 구현 한다.**

[States](https://github.com/k660323/BossHunter/tree/main/Scripts/Contents/StateMachine/BaseState)


<br>

---

<br>

#### **Controller**
+ PlayerController
  + 사용자 입력에 대한 함수 바인딩 및 입력 여부 캐싱

[PlayerController.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Controllers/PlayerController.cs)

+ MonsterController
  + 인공지능이 StateMachine를 통해 필요로 하는 데이터를 정의한 클래스 (스폰 위치, 추적 시작 위치, 공격 대상) 

[MonsterController.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Controllers/MonsterController.cs)


<br>

---

<br>

#### **파티 시스템**
+ Party
  + 유저와 파티를 맺어 같이 던전에 입장 하도록 하는 기능 수행


**파티 초대 과정**
1. UI_Party를 열고 만들기 버튼을 눌러 서버 함수인 CTS_CreateParty()를 서버에게 호출 하도록 요청합니다.
2. 서버에서 CTS_CreateParty() 호출하여 해당 파티를 창설합니다.
3. 상대 플레이어 캐릭터에 마우스를 대고 클릭
4. PlayerController에서 OnPlayerCilcked 함수가 콜백 호출되어 Raycast를 수행한다.
5. Hit 오브젝트가 있고 플레이어이나 로컬 플레이어가 아니면 PlayerUI 멤버변수인 UI_Interaction을 띄운다. 

[UI_Interaction.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/SubItem/UI_Interaction.cs)


6. 파티요청 버튼을 클릭하면 해당 버튼에 바인딩된 함수를 호출하여 조건 충족시 서버 함수인 CTS_PartyApplication()를 호출 하도록 요청합니다.
7. CTS_PartyApplication() 함수를 호출하는 서버는 해당 유저의 파티 여부를 확인후 없으면 해당 유저에게 파티 요청에 대한 UI를 띄우도록 RPC_ShowPartyInvitation 함수를 RPC 합니다.
8. 서버의 RPC 요청을 받은 클라이언트는 UI_PartyInvitation를 화면에 띄웁니다.

[UI_PartyInvitation.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/Popup/UI_PartyInvitation.cs)


9. 해당 UI는 초대자의 이름으로 파티 요청에 대한 정보며 수락/취소가 가능합니다.
10. 수락시 수락한 플레이어의 파티 클래스에서 서버 함수인 CTS_RequestJoinPart() 함수를 서버에게 호출하도록 요청합니다.
11. 서버에서는 수락한 플레이어의 파티 클래스의 CTS_RequestJoinPart()를 실행하며 매개변수로 들어온 파티장의 파티 객체의 JoinParty()에 수락한 플레이어를 매개변수로 넣오 파티에 참가 시킵니다.
12. JoinParty()는 각 파티원에게 Party객체에 접근하여 참가자의 정보를 partDic 자료구조에 저장합니다. 그리고 새로운 참가자의 Party객체에 파티원 목록을 넣어주면 파티 초대가 완료 됩니다.


**파티 탈퇴 과정**
1. 클라이언트에서 파티 탈퇴 버튼을 누르면 서버 함수인 CTS_ResignPlayerToParty() 호출 하도록 요청합니다.
2. 요청받은 서버는 CTS_ResignPlayerToParty()를 호출 하며 대상 플레이어 객체의 Party객체의 partyDic에서 해당 플레이어의 제거하고 서버함수인 CTS_SecessionParty()를 호출합니다.
3. 파티가 없으면 return을 수행 합니다.
4. 탈퇴한 유저가 만약 파티장이면 파티원에게 해당 파티가 사라짐을 알리기 위해 partDic 변수를 초기화 시킵니다.
5. 파티장이 아니면 모든 파티원에게 해당 플레이어가 사라짐을 알리기 위해 모든 파티원의 partDic 변수에 해당 파티원을 제거합니다.

[Party.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Party/Party.cs)

<br>

---

<br>

#### **인벤토리**
+ Inventory
  + 플레이어가 소유한 아이템 데이터를 관리하는 클래스

[Inventory.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Inventory/Inventory.cs)


+ UI_Inventory
  + 플레이어가 소유한 아이템을 시각화 하여 보여주는 UI
  + 아이템과 상호작용 가능
    
[UI_Inventory.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/SubItem/UI_Inventory.cs)

     
+ Item
  + 아이템 메타 데이터를 정의하는 클래스
  + 모든 아이템은 상위 클래스 Item을 상속받아 구현

[Item](https://github.com/k660323/BossHunter/tree/main/Scripts/Contents/Item)  


<br>

---

<br>

#### **장비창**
+ Equipment
  + 플레이어가 장착한 장비 데이터를 관리하는 클래스
  + 장착시 아이템 정보에 따라 해당 능력치 콜백 방식으로 반영
  + Equipment를 상속받은 아이템에 한해서 장착 가능
  + 장착 부위들을 Enum 구현하여 Func 배열을 통해 Enum값에 따른 콜백 기능 구현
[Equipment.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Weapon/EquipmentManager.cs)


+ UI_Equipment
  + 플레이어가 장착한 장비 데이터를 시각화 하여 UI에 표시하는 클래스
 
[UI_Equipment.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/SubItem/UI_Inventory.cs)


<br>

---

<br>

#### **능력치**
+ Stat
  + Creature(Player, Monster)의 능력치를 관리하는 클래스 (Hp,Mp,Speed...)
  + 값 변경에 따른 결과를 사전에 등록한 관찰자에게 알려주는 방식의 옵저버 패턴 구현

[Stat.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/Stat/Stat.cs)


<br>

---

<br>


### **씬**
전체적인 씬은 오프라인, 온라인, 로비, 마을, 던전 씬으로 나눠서 구현

+ BaseScene
  + 씬마다 존재하는 씬 관리 클래스
  + 해당 씬에 존재하는 모든 네트워크 오브젝트들을 관리
    
[BaseScene](https://github.com/k660323/BossHunter/blob/main/Scripts/Scenes/BaseScene.cs)

   
#### **오프라인 씬**
+ OfflineScene
  + 서버를 열거나 서버에 참여하기 전의 씬
  + 각종 Managers클래스의 데이터 및 리소스들을 미리 로드하는 씬 입니다.

[OfflineScene](https://github.com/k660323/BossHunter/blob/main/Scripts/Scenes/OfflineScene.cs)


**호스트 서버 생성 흐름**
1. 게임 시작 버튼 클릭
2. Managers클래스의 StartHost()함수 호출
3. Mirror에서 내부 코어 함수 호출
5. 호스트 서버 생성 연결
6. OnlineScene 로드 
7. 로드 완료시 OnServerConnect를 호출하여 호스트 입장
8. 서버에게 패킷을 보내 OnServerReady함수 콜백 호출
9. LobbyScene 추가 생성 및 서버와 클라와 통신할 오브젝트 생성
    
+ UI_MenuScene
   + 호스트 서버 생성, 서버 참여, 옵션, 종료 기능이 구현되어 있습니다.
     
[UI_MenuScene.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/Scene/UI_MenuScene.cs)


##### **온라인 씬**
+ OnlineScene
  + 클라이언트와 서버가 최초로 생성하는 씬
  + 여러 씬에서 사용되는 오브젝트들을 묶어 해당 씬에서 관리한다. (카메라, 포스트 프로세싱, 조명, 이벤트 시스템 등)
 
[OnlineScene.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Scenes/OnlineScene.cs)


##### **로비 씬**
+ LobbyScene
  + OnlineScene 로드후 다음으로 로드되는 컨텐츠 씬
  + 유저가 플레이할 캐릭터를 선택하는 씬

+ UI_LobbyScene
  + 유저가 플레이할 캐릭터를 보여주고 선택하게 해주는 UI

**마을 입장 과정**
1. UI_LobbyScene의 스크립트 Init()함수 초기화
2. Init()에서 버튼 바인딩 및 초기화
3. 플레이어가 캐릭터와 닉네임을 정하고 시작 버튼을 누른다.
4. 해당 버튼에 바인딩된 함수 실행하여 올바른 정보인지 확인하고 서버에게 StartSpawnPlayer 구조체 메시지를 보낸다.
5. Managers클래스에서 시작시 해당 구조체와 바인딩한 함수 ResourcesMangers의 OnStartSpawnPlayer를 호출
6. 해당 플레이어의 오브젝트를 제거 및 연결을 끊고 새로운 오브젝트를 생성, 마을 씬으로 이동 시킨후 다시 클라이언트와 연결 시킨다.
    
[UI_LobbyScene.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/Scene/UI_LobbyScene.cs)

[Resources.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Managers/Core/ResourceManager.cs)


##### **마을 씬**
+ TownScene
  + 플레이어 생성시 입장 하는 마을
  + 채팅, 플레이어와 상호 작용, 던전 입장을 할 수 있습니다.

+ InstacnePortal
  + 던전에 입장하기 위한 포탈
  + 해당 포탈에 트리거 충돌시 던전 UI 생성
  + 던전 선택시 바인딩된 함수 MoveToInstanceScene이 서버에서 실행하여 씬 생성후 유저들을 이동 시켜줍니다.
    
[InstancePortal.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/InstancePortal.cs)


<br>

---

<br>

#### **던전 씬**
+ BaseInstanceScene
  + BaseScene을 상속받은 클래스
  + 동적으로 생성되고 사라지는 씬
  + 모든 네트워크 오브젝트를 가지고 있던 BaseScene을 상속받았기에 플레이어 오브젝트를 추적하여 씬 제거 여부를 수행한다.
    
[BaseInstanceScene.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Scenes/Instance/BaseInstanceScene.cs)


+ MonsterSpawner
  + 몬스터는 동적으로 생성하는 스포너
  + 몬스터 종류, 스폰 수, 스폰 반경, 지속성을 미리 설정하면 런타임에 설정 값에 맞게 몬스터를 스폰해준다.
  + 몬스터 스폰 최대 숫자는 스폰 수를 넘어가지 않는다.

[MonsterSpanwer.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/Contents/MonsterSpawner.cs)


<br>

---

<br>

#### **기타**
+ UI_Chat
  + 채팅 오브젝트
  + InputField에 보낼 텍스트를 입력 후 전송시, 포멧으로 플레이어 닉네임 삽입 후 씬에 BaseSceneNetwork를 상속받은 클래스를 찾아 IChatable인터페이스를 상속받은지 확인한다.
  + 상속받았을 경우 리플렉션이[Command]인 CTS.ChatRPC 함수를 호출하여 해당씬에 존재하는 플레이어에게 채팅 메시지를 전송합니다.
    
[UI_Chat.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/Scene/UI_Chat.cs)


+ UI_PlayerUI
  + 환경 설정, 인벤토리, 능력치, 파티 등 플레이어에 관련된 UI를 접근하게 하는 클래스
    
[UI_PlayerUI.cs](https://github.com/k660323/BossHunter/blob/main/Scripts/UI/Scene/UI_PlayerUI.cs)
     

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
