# GanEcosystem

研究优秀的游戏开发设计，将可复用的解决方案沉淀为可独立使用、可组合、可跨引擎的模块。

## 设计原则

1. 模块独立
   1. 每个模块都可以独立使用，而不是依赖整个生态。
2. 依赖抽象
   1. 模块永远依赖抽象，而不是具体实现。
   2. 通过接口和手动注入解耦具体实现。
3. 核心跨引擎
   1. 业务逻辑与引擎 API 分离，通过适配层支持不同引擎。
   2. 分为Core和Runtime俩个部分，Core为核心模块，Runtime为引擎适配层。
4. 组合优于框架
5. 渐进演化
   1. 抽象来自真实需求，而不是预先设计。
6. 源码优先
   1. 复制文件即可使用。

## 模块

### UI

核心实现UIManager，支持UI的创建、显示、隐藏、销毁等操作。
基于MVP设计模式，将UI部分划分为Presenter、Viewer俩部分，Presenter负责处理UI逻辑，Viewer负责UI的显示。
Presenter通过事件总线获得游戏内具体数据，控制Viewer进行数据显示。
ICursorController，负责鼠标光标的控制。

- UnityRuntime (已实现)
  - 需要注入的模块：IUIEventBus、IUIResLoader
- GodotRuntime（计划实现）

## 后续计划

- 持久化
- 事件总线
- 资源读取
- Log日志
- 更新管理器
