# Unity对话驱动系统
这是对话系统最早期的实现 功能很简单 仅作原型验证使用 现在留着纪念那些纯手搓 AI还是弱智的日子

### 项目初衷是尽可能的减少编剧程序对接 增加灵活程度 可以让编剧连unity都不装 且能随时更改剧本

项目用于大部分Unity游戏的对话交互系统 理论上可以直接用这个手搓简单的gal 但如果只是做gal 其实现在早就有用浏览器实现gal效果的工具(WebGal)了 何必多折腾一步呢

### 项目始于2023年年底 部分高级功能 比如复杂进度判断、分支记录等 需要自己实现 目前其迭代产品更换了前后端框架 并在高级功能方面做了诸多改进 

# 使用说明

一个用于 Unity 项目的对话脚本工作流示例，包括：
- **DialogEditor**：WinForms 可视化编辑器，帮助策划维护 JSON 对话脚本；
- **DialogTest**：运行时播放器（WinForms），演示脚本遍历、选项分支、动作回调等逻辑；
- **Manager**：核心解析/运行库，负责加载 `.story` 文件、维护角色/动作映射。

> 本仓库同时附带示例脚本 `对话.story`，展示多场景、多层选项的编排方式，并提供将同一套解析逻辑迁移到 Unity 的指南。

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
