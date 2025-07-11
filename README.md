# Digital Solutions Web 数字未来科技官网

## 项目概述

Digital Solutions Web 是一个基于ASP.NET Core MVC开发的企业官网系统，旨在展示数字未来科技有限公司的服务、案例和团队信息，同时提供联系表单功能，方便潜在客户与公司取得联系。该项目采用现代化的响应式设计，确保在各种设备上都能提供良好的用户体验。

## 技术栈

- **后端框架**：ASP.NET Core 8.0 MVC
- **数据库**：SQLite
- **身份认证**：ASP.NET Core Identity
- **前端框架**：Bootstrap 5
- **JavaScript库**：jQuery, AOS (Animate On Scroll)
- **图标库**：Bootstrap Icons

## 主要功能

1. **公司介绍**：展示公司简介、历史、团队和核心价值观
2. **服务展示**：详细介绍公司提供的各类数字化解决方案
3. **案例研究**：展示成功案例，包括项目背景、挑战、解决方案和成果
4. **客户评价**：展示客户对公司服务的评价
5. **联系表单**：访客可以填写联系表单，发送咨询或合作意向
6. **管理后台**：管理员可以登录后台管理系统，处理联系消息和更新内容

## 项目结构

```
DigitalSolutionsWeb/
├── Areas/                      # 区域文件夹
│   ├── Admin/                  # 管理员区域
│   └── Identity/               # 身份验证区域
├── Controllers/                # 控制器
│   ├── AccountController.cs    # 账户相关控制器，处理密码修改等功能
│   ├── HomeController.cs       # 首页和主要页面控制器
│   ├── ImageController.cs      # 图片处理控制器
│   └── Api/                    # API控制器
├── Data/                       # 数据访问层
├── Migrations/                 # 数据库迁移文件
├── Models/                     # 模型类
├── Services/                   # 服务类
│   ├── EmailService.cs         # 邮件服务
│   ├── ImageService.cs         # 图片服务
│   ├── PasswordPolicyService.cs# 密码策略服务
│   └── PasswordChangeMiddleware.cs # 密码修改中间件
├── Views/                      # 视图文件
│   ├── Account/                # 账户相关视图（如密码修改）
│   ├── Home/                   # 主页视图
│   └── Shared/                 # 共享视图组件
├── wwwroot/                    # 静态资源文件
│   ├── css/                    # CSS样式
│   ├── images/                 # 图片资源
│   ├── js/                     # JavaScript文件
│   └── lib/                    # 前端库
├── appsettings.json            # 应用程序配置
└── Program.cs                  # 应用程序入口
```

## 安装与运行

1. **克隆项目**：
   ```
   git clone <repository-url>
   cd DigitalSolutionsWeb
   ```

2. **安装依赖**：
   ```
   dotnet restore
   ```

3. **运行数据库迁移**：
   ```
   dotnet ef database update
   ```

4. **运行应用**：
   ```
   dotnet run
   ```

5. **访问网站**：
   打开浏览器访问 `https://localhost:5001` 或 `http://localhost:5000`

## 系统访问

### 初始设置

首次启动应用程序时，系统会自动创建一个默认管理员账户。账户凭据处理流程如下：

1. 启动应用程序：`dotnet run`
2. 查看控制台输出，系统会显示临时管理员凭据信息（sfrost@qq.com Admin.888）
3. 使用这些临时凭据登录系统 
4. **安全措施**：系统会强制要求首次登录后立即修改默认密码

### 自定义管理员账户

可以通过环境变量自定义管理员账户：

```bash
# macOS/Linux
export ADMIN_EMAIL="your-admin@example.com"
export ADMIN_PASSWORD="YourSecurePassword123!"
dotnet run

# Windows (PowerShell)
$env:ADMIN_EMAIL="your-admin@example.com"
$env:ADMIN_PASSWORD="YourSecurePassword123!"
dotnet run
```

如果未设置这些环境变量，系统会使用默认管理员邮箱并自动生成安全的临时密码。

## 图片资源管理

项目使用本地图片资源，所有图片均存储在 `/wwwroot/images/` 目录下：
- `/images/stock/` - 存储通用图片
- `/images/team/` - 团队成员头像
- `/images/slide/` - 首页轮播图
- `/images/case-studies/` - 案例相关图片

## 系统设置

系统设置存储在 `appsettings.json` 文件中的 `SystemSettings` 节点下，包括：
- 公司名称
- 公司地址
- 联系电话
- 联系邮箱

## 安全特性

### 密码策略与管理

系统实现了一套完善的密码安全机制：

1. **强制首次修改密码**：
   - 初始管理员账号在首次登录时会被强制要求修改默认密码
   - 密码修改提示会在页面顶部持续显示，直到用户完成密码修改

2. **密码复杂度要求**：
   - 最小长度：8个字符
   - 必须包含：大写字母、小写字母和数字
   - 可选包含特殊字符（推荐）

3. **密码状态跟踪**：
   - 系统使用声明（Claim）机制记录用户是否已修改初始密码
   - `PasswordPolicyService`负责检查和管理密码状态
   - `PasswordChangeMiddleware`在应用程序请求管道中监控用户访问并提醒密码修改

4. **安全密码生成**：
   - 系统自动为初始管理员账户生成高强度随机密码
   - 生成的密码仅在服务器控制台显示一次，不会被持久化存储

## 开发者

数字未来科技有限公司开发团队 - 2025年
