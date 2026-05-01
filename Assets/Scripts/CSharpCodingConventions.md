# C# 코딩 컨벤션 - 명명 규칙 가이드

> Microsoft .NET 공식 코딩 스타일 기반 정리

---

## 1. 네임스페이스 (Namespace)

**규칙**: `PascalCase`, 역 도메인 표기법

```csharp
namespace CompanyName.ProjectName.Feature;
namespace Unity.Core.Module;
namespace Game.Systems.Combat;
```

---

## 2. 클래스 (Class)

**규칙**: `PascalCase`

```csharp
public class PlayerController { }
public class DataService { }
internal class CacheManager { }
```

---

## 3. 구조체 (Struct)

**규칙**: `PascalCase`

```csharp
public struct Vector3 { }
public struct ValueCoordinate { }
public readonly struct Color32 { }
```

---

## 4. 인터페이스 (Interface)

**규칙**: `I` + `PascalCase`

```csharp
public interface IDisposable { }
public interface IWorkerQueue { }
public interface ISessionChannel<TSession> { }
```

---

## 5. 열거형 (Enum)

**규칙**: `PascalCase`, 비플래그는 단수 명사, 플래그는 복수 명사

```csharp
// 비플래그 - 단수
public enum Direction
{
    North,
    South,
    East,
    West
}

// 플래그 - 복수
[Flags]
public enum Permissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}
```

---

## 6. 델리게이트 (Delegate)

**규칙**: `PascalCase`

```csharp
public delegate void EventHandler(object sender, EventArgs e);
public delegate TOutput Converter<TInput, TOutput>(TInput from);
```

---

## 7. 메서드 (Method)

**규칙**: `PascalCase`

```csharp
public void StartEventProcessing() { }
public int CalculateDamage() { }
private void InitializeComponents() { }
protected virtual void OnDestroy() { }
internal void ResetState() { }
```

---

## 8. 로컬 함수 (Local Function)

**규칙**: `PascalCase`

```csharp
public void Process()
{
    static int CountItems() => items.Count;

    bool ValidateInput(string input) => !string.IsNullOrEmpty(input);
}
```

---

## 9. 속성 (Property)

**규칙**: `PascalCase` (접근 제한자 무관)

```csharp
// public
public string Name { get; set; }
public int Health { get; private set; }
public bool IsAlive => Health > 0;

// protected
protected float MoveSpeed { get; set; }

// internal
internal int ConnectionCount { get; set; }

// private
private string CachedValue { get; set; }

// init-only
public required string Id { get; init; }
```

---

## 10. 필드 (Field)

### 10.1 public 필드

**규칙**: `PascalCase` (가능한 사용 자제, 속성 권장)

```csharp
public bool IsValid;
public int MaxRetryCount;
```

### 10.2 private / internal 인스턴스 필드

**규칙**: `_` + `camelCase`

```csharp
private int _health;
private string _playerName;
private IWorkerQueue _workerQueue;
private readonly float _moveSpeed;
internal int _connectionCount;
```

### 10.3 private / internal static 필드

**규칙**: `s_` + `camelCase`

```csharp
private static IWorkerQueue s_workerQueue;
private static int s_instanceCount;
private static readonly string s_defaultName;
internal static bool s_isInitialized;
```

### 10.4 ThreadStatic 필드

**규칙**: `t_` + `camelCase`

```csharp
[ThreadStatic]
private static TimeSpan t_timeSpan;

[ThreadStatic]
private static int t_threadLocalCounter;
```

---

## 11. 상수 (Const)

**규칙**: `PascalCase` (필드 상수, 로컬 상수 모두)

```csharp
// 필드 상수
public const int MaxItemCount = 100;
private const string DefaultPrefix = "item_";
public const float Gravity = 9.81f;

// 로컬 상수
public void Calculate()
{
    const int MaxIterations = 10;
    const double Pi = 3.14159;
}
```

---

## 12. 이벤트 (Event)

**규칙**: `PascalCase`

```csharp
public event Action EventProcessing;
public event EventHandler<PlayerEventArgs> PlayerDied;
public event Action<int, string> ScoreChanged;
```

---

## 13. 메서드 매개변수 (Method Parameter)

**규칙**: `camelCase`

```csharp
public void SetPosition(float xPos, float yPos) { }
public T SomeMethod<T>(int someNumber, bool isValid) { }
public void Initialize(string playerName, int maxHealth) { }
```

---

## 14. 지역 변수 (Local Variable)

**규칙**: `camelCase`

```csharp
public void Process()
{
    int itemCount = 0;
    string displayName = "Player";
    var currentTemperature = 27;
    bool isRunning = true;
}
```

---

## 15. 주 생성자 매개변수 (Primary Constructor Parameter)

### 15.1 class / struct 타입

**규칙**: `camelCase` (일반 매개변수와 동일)

```csharp
public class DataService(IWorkerQueue workerQueue, ILogger logger)
{
    public void Process()
    {
        logger.LogInformation("Processing");
        workerQueue.Enqueue("data");
    }
}

public struct Point(double x, double y)
{
    public double Distance => Math.Sqrt(x * x + y * y);
}
```

### 15.2 record 타입

**규칙**: `PascalCase` (공개 속성이 되므로)

```csharp
public record Person(string FirstName, string LastName);
public record PhysicalAddress(string Street, string City, string ZipCode);
```

---

## 16. 제네릭 타입 매개변수 (Generic Type Parameter)

**규칙**: `T` 접두사 + `PascalCase` (단일 매개변수는 `T`만 허용)

```csharp
// 단일 타입 매개변수
public class List<T> { }
public delegate bool Predicate<T>(T item);

// 설명적 이름
public interface ISessionChannel<TSession> { }
public delegate TOutput Converter<TInput, TOutput>(TInput from);
public class Repository<TEntity> where TEntity : class { }

// 제약 조건을 이름에 반영
public class Handler<TMessage> where TMessage : IMessage { }
```

---

## 17. Attribute 클래스

**규칙**: `PascalCase` + `Attribute` 접미사

```csharp
public class SerializableAttribute : Attribute { }
public class ObsoleteAttribute : Attribute { }
public class CustomValidationAttribute : Attribute { }
```

---

## 전체 요약 표

| 대상 | 규칙 | 예시 |
|------|------|------|
| 네임스페이스 | `PascalCase` | `Game.Systems.Combat` |
| 클래스 | `PascalCase` | `PlayerController` |
| 구조체 | `PascalCase` | `ValueCoordinate` |
| 인터페이스 | `I` + `PascalCase` | `IWorkerQueue` |
| 열거형 (비플래그) | `PascalCase` 단수 | `Direction` |
| 열거형 (플래그) | `PascalCase` 복수 | `Permissions` |
| 열거형 멤버 | `PascalCase` | `Direction.North` |
| 델리게이트 | `PascalCase` | `EventHandler` |
| 메서드 | `PascalCase` | `CalculateDamage()` |
| 로컬 함수 | `PascalCase` | `CountItems()` |
| 속성 | `PascalCase` | `MaxHealth` |
| public 필드 | `PascalCase` | `IsValid` |
| private/internal 필드 | `_camelCase` | `_playerName` |
| private/internal static 필드 | `s_camelCase` | `s_instanceCount` |
| ThreadStatic 필드 | `t_camelCase` | `t_timeSpan` |
| 상수 (const) | `PascalCase` | `MaxItemCount` |
| 이벤트 | `PascalCase` | `PlayerDied` |
| 매개변수 | `camelCase` | `playerName` |
| 지역 변수 | `camelCase` | `itemCount` |
| 주 생성자 (class/struct) | `camelCase` | `(ILogger logger)` |
| 주 생성자 (record) | `PascalCase` | `(string FirstName)` |
| 제네릭 타입 매개변수 | `T` + `PascalCase` | `TSession`, `TInput` |
| Attribute 클래스 | `PascalCase` + `Attribute` | `SerializableAttribute` |

---

## 전체 예시 코드

```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Systems.Combat;

/// <summary>
/// 전투 시스템의 데미지 계산을 담당하는 서비스.
/// </summary>
public class DamageCalculator : IDamageCalculator
{
    // const - PascalCase
    public const float DefaultMultiplier = 1.0f;
    private const int MaxComboCount = 10;

    // public 필드 - PascalCase (가급적 속성 사용 권장)
    public bool IsEnabled;

    // private 필드 - _camelCase
    private readonly float _baseDamage;
    private int _comboCount;

    // private static 필드 - s_camelCase
    private static int s_totalCalculations;

    // ThreadStatic 필드 - t_camelCase
    [ThreadStatic]
    private static float t_cachedResult;

    // 속성 - PascalCase
    public float CurrentDamage { get; private set; }
    public required string WeaponName { get; init; }

    // 이벤트 - PascalCase
    public event Action<float> DamageApplied;
    public event EventHandler<CombatEventArgs> CriticalHitOccurred;

    // 생성자 매개변수 - camelCase
    public DamageCalculator(float baseDamage, int initialCombo)
    {
        _baseDamage = baseDamage;
        _comboCount = initialCombo;
    }

    // 메서드 - PascalCase / 매개변수 - camelCase
    public float CalculateDamage(int attackPower, bool isCritical)
    {
        // 지역 변수 - camelCase
        var multiplier = isCritical ? 2.0f : DefaultMultiplier;

        // 로컬 상수 - PascalCase
        const float MinDamage = 1.0f;

        // 로컬 함수 - PascalCase
        static float ClampValue(float value, float min) => Math.Max(value, min);

        var result = ClampValue(_baseDamage * attackPower * multiplier, MinDamage);
        s_totalCalculations++;

        return result;
    }
}

// 인터페이스 - I + PascalCase
public interface IDamageCalculator
{
    float CalculateDamage(int attackPower, bool isCritical);
}

// record - 주 생성자 매개변수 PascalCase
public record CombatResult(string AttackerName, float DamageDealt, bool WasCritical);

// struct - PascalCase
public readonly struct DamageRange
{
    public float Min { get; init; }
    public float Max { get; init; }
}

// enum 비플래그 - 단수
public enum DamageType
{
    Physical,
    Magical,
    True
}

// enum 플래그 - 복수
[Flags]
public enum StatusEffects
{
    None = 0,
    Burning = 1,
    Frozen = 2,
    Poisoned = 4
}

// Attribute - PascalCase + Attribute 접미사
[AttributeUsage(AttributeTargets.Method)]
public class CombatActionAttribute : Attribute
{
    public string Description { get; set; }
}

// 제네릭 타입 - T 접두사
public class Repository<TEntity> where TEntity : class
{
    private readonly List<TEntity> _items = [];

    public TEntity FindById<TKey>(TKey id) where TKey : IEquatable<TKey>
    {
        return default;
    }
}
```
