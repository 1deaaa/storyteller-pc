# Dialog System Toolkit

一个用于 Unity 项目的对话脚本工作流示例，包括：
- **DialogEditor**：WinForms 可视化编辑器，帮助策划维护 JSON 对话脚本；
- **DialogTest**：运行时播放器（WinForms），演示脚本遍历、选项分支、动作回调等逻辑；
- **Manager**：核心解析/运行库，负责加载 `.story` 文件、维护角色/动作映射。

> 本仓库同时附带示例脚本 `对话.story`，展示多场景、多层选项的编排方式，并提供将同一套解析逻辑迁移到 Unity 的指南。

---

## 功能亮点
- **JSON 场景脚本**：支持场景标题（`scene`）、任务提示（`cap`）、进度（`pgrs`）、对话文本、分支选项、动作触发（`act`）与跨场景跳转（`next`）。
- **栈式分支解析**：`Dialog` 静态类通过栈管理子分支，保证任意层级的选项都能回到正确的父节点。
- **动作映射**：脚本中的 `act` 字段映射到 C# 函数（无参与带参均可），方便串联音效、过场、任务逻辑。
- **可视化编辑器**：树形视图编辑/搜索节点，支持历史栈撤销、批量修改角色、选项文本等。
- **Unity 友好**：运行时代码与数据结构可直接迁入 Unity，文档中附带触发器、Story 资产、UI 绑定的建议实现。

---

## 仓库结构
```
GameTest/
├── DialogEditor/        # WinForms 编辑器项目
├── DialogTest/          # WinForms 播放器项目
├── Manager/             # 核心解析库（共享代码）
├── 对话.story           # 示例脚本（JSON 数组）
├── GameTest.sln         # Visual Studio 解决方案
└── README.md
```

---

## 入门指南
### 环境要求
- Windows 10/11
- Visual Studio 2022（或 2019）+ “.NET 桌面开发”工作负载
- .NET Framework 4.8 SDK（VS 安装时会自动带上）

### 获取代码
```powershell
# 克隆仓库
git clone https://github.com/AIdeaStudio/dialog_system.git
cd dialog_system/GameTest
```

### 恢复依赖并编译
项目使用 `packages.config` 管理依赖（仅 `Newtonsoft.Json`）。推荐两种方式：
1. **Visual Studio**：打开 `GameTest.sln`，首次加载会自动还原 NuGet 包并完成构建；
2. **命令行**（需要安装 `nuget.exe` 并加入 PATH）：
   ```powershell
   nuget restore GameTest.sln
   msbuild GameTest.sln /p:Configuration=Debug
   ```

---

## 如何运行
### 编辑器（DialogEditor）
1. 在 VS 中将 `DialogEditor` 设为启动项目；
2. 运行后选择或打开 `对话.story`（可在根目录直接使用示例文件）；
3. 使用树状面板浏览/修改对话节点，必要时保存 JSON 文件供游戏加载。

### 播放器（DialogTest）
1. 将 `DialogTest` 设为启动项目；
2. 启动后会自动读取 `Manager.Manager.DataFilePath` 指定的 `对话.story`；
3. 点击文本区域推进对话，遇到分支时选择按钮，体验完整流程。

---

## 脚本格式速览
一个最小对话节点示例：
```json
{
  "scene": "梦核入口",
  "cap": "初次进入梦境",
  "pgrs": 0.3,
  "dia": [
    {
      "id": 10001,
      "chr": 0,
      "txt": "欢迎来到梦核。",
      "opt": [
        {
          "optn": "继续前行",
          "dia": [{ "id": 10002, "chr": 1, "txt": "勇敢些。" }]
        }
      ],
      "act": {
        "bgm": "dream_theme.mp3"
      }
    }
  ]
}
```
字段说明：
- `scene`：场景/剧本名（唯一标识场景）；
- `cap`/`pgrs`：任务提示 & 进度；
- `dia`：当前场景的主线对话数组；
- `id`：对话节点唯一 ID；
- `chr`：角色编号，解析时通过 `Map.ChrMap` 转换成姓名；
- `txt`：对话内容；
- `opt`：分支选项数组，每个选项可继续嵌套 `dia`；
- `act`：动作指令字典（可映射到音频、动画等逻辑）；
- `next`：跨场景跳转的目标 `scene` 名。

---

## Unity 集成指南（新手到企业）
> 目标：让个人开发者 30 分钟内跑通，同时为团队/企业项目保留扩展空间。

### 0. 场景层级示意
```text
Scene Root
├─ Systems (DontDestroyOnLoad)
│  ├─ DialogueSystem
│  │  ├─ DialogueService (MonoBehaviour)
│  │  └─ StoryRegistry (ScriptableObject 引用)
│  ├─ AudioManager
│  └─ QuestManager
├─ NPCs
│  ├─ NPC_Librarian (NPCDialogueTrigger)
│  └─ NPC_Guard (NPCDialogueTrigger)
├─ InteractionObjects
│  └─ AncientStatue (ObjectDialogueTrigger)
└─ UI
  └─ DialogueCanvas (DialogueUIController)
    ├─ SpeakerText / BodyText
    ├─ PortraitImage (可选)
    ├─ ChoicesRoot
    └─ OptionButtonPrefab
```

### 1. 核心服务：DialogueSystem
1. 在场景中新建空物体 `Systems/DialogueSystem`。
2. 编写 `DialogueService`（MonoBehaviour）：
  - `Awake()` 中调用 `DontDestroyOnLoad(gameObject)`；
  - 暴露字段：`TextAsset defaultStory`（或 StreamingAssets 路径）、`float typingSpeed`、`bool allowSkip`；
  - 方法：`BeginStory(string storyId, string sceneOverride = null)`、`Continue()`、`Choose(int optionIndex)`；
  - 事件：`OnLineStart(string speaker, string text, Sprite portrait)`、`OnChoices(List<ChoiceData>)`、`OnStoryEnd(string storyId)`；
  - 内部负责：读取 `.story`、将 JSON 传给 `Dialog.SceneInit`、缓存已加载的 `JArray`、处理 `Map.ActMap` 回调。
3. 将 `DialogueService` 挂到 `DialogueSystem`，填好默认故事文件或 StreamingAssets 路径。

> **贴士（新手）**：如果只需要一个故事文件，可以直接在 Inspector 指定 `TextAsset`；未来要热更时再切换成 StreamingAssets/Addressables。

### 2. 故事资产：StoryRegistry
1. 创建 `StoryRegistry` ScriptableObject，字段示例：
  ```csharp
  [Serializable] struct StoryEntry {
     public string storyId;       // 逻辑使用的标识
     public string fileName;      // StreamingAssets 中的文件名
     public string defaultScene;  // story 中的默认 scene
     public Sprite cover;         // 可选：用于 UI 列表
  }
  public List<StoryEntry> entries;
  ```
2. 在项目中右键 `Create > Dialog System > Story Registry` 生成资产，录入所有故事。
3. `DialogueService` 中引用该资产，按需懒加载并缓存故事。

> **贴士（企业）**：配合 CI 生成 Registry，确保策划提交 `.story` 后自动写入列表，避免硬编码。

### 3. 对话 UI：DialogueCanvas 预制体
1. 创建 Canvas，设置 `Screen Space - Overlay`。
2. 添加：
  - `SpeakerText` / `BodyText`（TextMeshPro 推荐）；
  - `PortraitImage`（可选）；
  - `ChoicesRoot`（Vertical Layout Group + Content Size Fitter）；
  - `ChoiceButtonPrefab`（包含 `DialogueChoiceButton` 脚本，`Setup(int index, string text, UnityAction<int> onClick)`）。
3. 编写 `DialogueUIController`：
  - Inspector 中拖入 `DialogueService`、文本组件、`ChoicesRoot`、按钮预制体；
  - `OnEnable` 注册 `DialogueService` 事件，`OnDisable` 注销；
  - 处理打字机：监听 `OnLineStart`，启动协程或 async 展示文本；
  - 生成/回收选项按钮，绑定点击事件调用 `service.Choose(index)`。

> **贴士（新手）**：先确保按钮是“复制 + 修改文本”，后期再引入对象池／动画。

### 4. 通用触发器组件
1. 创建 `DialogueTriggerBase`（MonoBehaviour）：
  - 字段：`DialogueService service`、`string storyId`、`string sceneOverride`、`bool autoPlayOnEnter`；
  - 方法 `Trigger()` 调用 `service.BeginStory(storyId, sceneOverride)`；
  - 可选事件：`UnityEvent onBeforePlay`、`UnityEvent onAfterPlay`。
2. 派生：
  - `NPCDialogueTrigger`：在 `OnTriggerEnter` 检查玩家，必要时显示“按 E 对话”的 UI；
  - `ObjectDialogueTrigger`：暴露 `Interact()`，由通用交互系统或 Timeline 调用；
  - `AutoDialogueTrigger`：`Start()` 时直接触发，适合剧情开场。
3. 将触发器脚本打包进 NPC/物件预制体；实例化后仅需在 Inspector 修改 `storyId` 或 `sceneOverride`。

> **贴士（扩展）**：将触发器字段换成 `enum` 或 `ScriptableObject`，避免硬编码字符串。

### 5. 从零到可玩（最小流程）
1. 将 `对话.story` 放入 `Assets/StreamingAssets/Dialogues/`。
2. 在 `StoryRegistry` 中添加条目：`storyId = "DreamIntro"`、`fileName = "对话.story"`、`defaultScene = "这是标题 也用于定位场景"`。
3. 场景中放置玩家、一个带 `NPCDialogueTrigger` 的 NPC，`storyId` 填 `DreamIntro`。
4. 运行游戏，靠近 NPC（或按提示键）调用 `Trigger()`，对话 UI 自动弹出。
5. 点击/选择选项，确认栈式分支与 `act` 映射触发正常。

### 6. 进阶到企业级项目
- **资产分层**：将故事文件拆成 Addressables Group，按章节/语言分区；`StoryRegistry` 只维护元数据。
- **数据验证**：CI 中运行 JSON Schema 检查、重复 ID 检测、`next` 指向校验。
- **分析与监控**：`DialogueService` 打点（storyId / scene / option），上传到行为分析平台。
- **协同编辑**：保留 WinForms/自研 Unity Editor 同步脚本；或编写 GraphView 节点编辑器供策划使用。
- **多语言支持**：`txt` 替换为 Localization Key，运行时通过 Unity Localization 抓取文本；`StoryRegistry` 记录语言包。
- **存档 & 网络**：`DialogueService` 暴露可序列化状态（当前 storyId、scene、栈路径），方便存档或同步到服务器。


## 常见问题
- **无法加载 JSON**：确保 `Manager.Manager.DataFilePath` 指向正确路径；如果迁移到 Unity，使用完整路径或 `StreamingAssets`。
- **NuGet 包缺失**：执行 `nuget restore`，或在 VS 中右键解决方案选择“还原 NuGet 包”。
- **打字机被打断**：`Dialog.DisplayOne` 中通过 `CancellationTokenSource` 控制，可根据需要调整 `Dialog.TypingSpeed` 或 `Dialog.AllowSkip`。
- **选项按钮不出现**：检查 `Dialog.ActMap` 是否抛错导致流程提前返回；同时确认 UI 事件订阅正确。

---

## 后续计划（建议）
- Editor 中增加 JSON 校验 & 图形化节点连接；
- 引入单元测试覆盖 `Dialog` 栈逻辑；
- 官方 Unity Package 化（ScriptableObject 数据 + Inspector）；
- 添加 Localization & Save/Load 示例；
- 提供 CLI 工具把 CSV/Excel 转换成 `.story`。

---

## 许可
仓库未附带正式许可证。如需开源发布，请补充相应 LICENSE 文件或在 README 中声明使用条款。
