# 🚀 BattleStars

BattleStars is my personal training ground for becoming a better software developer. The goal isn’t to build a great game—it’s to build great code.

I use this project to practice clean architecture, behavior-driven development (BDD), software design patterns, structured workflows, and clear communication through code.

I bring a strong academic foundation in what code should be, and BattleStars is where I turn theory into practice—one small, intentional commit at a time.

![Build](https://github.com/CausesHavok/BattleStars/actions/workflows/ci.yml/badge.svg)
[![Coverage Status](https://coveralls.io/repos/github/CausesHavok/BattleStars/badge.svg?branch=master)](https://coveralls.io/github/CausesHavok/BattleStars?branch=master)

---

## 🧩 Features

- 🔫 **BattleStar Core**: Abstracted interfaces for shooting, movement, and destructibility
- 🧠 **Logic Layer**: Collision detection, boundary enforcement, and validation
- 🎯 **Shapes**: Circle, Rectangle, Triangle, and Polygon support with Raylib-style rendering
- 💥 **Shots**: Projectile modeling and factories
- 🧪 **Test Coverage**: 64% line coverage, 49% branch coverage (and climbing!)

---

## 🛠️ Tech Stack

- **C# 12.0**
- **xUnit** for testing
- **FluentAssertions** for expressive assertions
- **Moq** for mocking dependencies
- **Coverlet** for code coverage
- **GitHub Actions** for CI/CD
- **Coveralls** for coverage tracking

---

## 🚦 Getting Started

```bash
git clone https://github.com/CausesHavok/BattleStars.git
cd BattleStars
dotnet build
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 To view coverage locally:

```
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:CoverageReport -reporttypes:Html
code CoverageReport/index.html
```

## 📦 Project Structure

BattleStars/  
├── Application/  
│   ├── Checkers/        # Game rule validation (e.g., collisions, boundaries)  
│   ├── Controllers/     # Orchestrates game logic (player, enemy, shots)  
│   └── Services/        # Game services (input, boundary, collision)  
├── Core/  
│   ├── Guards/          # Centralized validation logic (FloatGuard, VectorGuard, etc.)  
│   │   ├── Utilities/   # Shared helpers like ParamNameResolver  
│   └── Contracts/       # Internal guard contracts and architectural enforcement  
├── Domain/  
│   ├── Entities/        # Core game objects and behaviors  
│   │   └── Shapes/      # Geometric primitives (Circle, Rectangle, etc.)  
│   ├── Interfaces/      # Domain contracts and abstractions  
│   └── ValueObjects/    # Immutable domain concepts (vectors, keys, descriptors)  
├── Infrastructure/  
│   ├── Adapters/        # External system adapters (input, graphics)  
│   ├── Factories/       # Object and service creation  
│   └── Utilities/       # General-purpose helpers  
├── Presentation/  
│   ├── Drawers/         # Shape/UI drawing interfaces and implementations  
│   ├── Renderers        # Frame-level rendering orchestation
│   └── Views/           # UI components (future expansion)  
├── BattleStars.Tests/   # Unit and integration tests with coverage tracking  
├── Program.cs           # Entry point  
├── BattleStars.csproj   # Project file  

## 📜 License
MIT License

## 🧠 Author
Magnus Storm - physicist turned software developer
