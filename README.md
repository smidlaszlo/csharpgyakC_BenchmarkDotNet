# BenchmarkDotNet Examples for C#

Ez a projekt különböző példákon keresztül mutatja be a [BenchmarkDotNet](https://benchmarkdotnet.org/?utm_source=chatgpt.com) használatát C# környezetben.

A példák segítségével megismerhetők:

* benchmark alapok,
* memóriahasználat mérése,
* paraméterezett benchmarkok,
* validátorok használata,
* string műveletek összehasonlítása,
* listaműveletek teljesítménye,
* ciklusok és matematikai műveletek mérése,
* threading diagnosztika,
* `StringBuilder` vs `string.Join` összehasonlítás.

---

# Tartalomjegyzék

* [Technológiák](#technológiák)
* [Telepítés](#telepítés)
* [Futtatás](#futtatás)
* [Benchmark példák](#benchmark-példák)
* [Használt attribútumok](#használt-attribútumok)
* [Validátorok](#validátorok)
* [Projektstruktúra](#projektstruktúra)
* [Példa eredmény](#példa-eredmény)

---

# Technológiák

* C#
* .NET
* BenchmarkDotNet

NuGet csomag:

```bash
dotnet add package BenchmarkDotNet
```

---

# Telepítés

Repository klónozása:

```bash
git clone <repository-url>
```

Projekt megnyitása:

```bash
cd Benchmark
```

Build:

```bash
dotnet build -c Release
```

---

# Futtatás

⚠️ A benchmarkokat mindig `Release` módban futtasd debugging nélkül.

Futtatás:

```bash
dotnet run -c Release
```

A futtatandó benchmark a `Program.cs` fájlban választható ki:

```csharp
var summary = BenchmarkRunner.Run<StringBenchmark>();
```

Más benchmark futtatásához csak cseréld ki a típust.

Példa:

```csharp
var summary = BenchmarkRunner.Run<ListBenchmark>();
```

---

# Benchmark példák

## 1. StringBenchmark

String összefűzés teljesítményének összehasonlítása:

* `StringConcat`
* `StringInterpolation`

```csharp
text + "!"
$"{text}!"
```

---

## 2. ListBenchmark

Lista műveletek mérése:

* lista összegzés
* rendezés
* lambda alapú rendezés

Attribútumok:

* `[MemoryDiagnoser]`
* `[ShortRunJob]`
* `[Params]`

---

## 3. LoopBenchmark

Ciklus benchmark konfigurációs példák:

* `IterationCount`
* `WarmupCount`
* `InvocationCount`

---

## 4. StringCreationBenchmark

Nagy string létrehozás memóriahasználatának mérése.

---

## 5. StringConcatBenchmark sorozat

Paraméterátadás különböző módjai:

* `[Arguments]`
* `[Params]`
* `[ArgumentsSource]`
* `[ParamsSource]`

---

## 6. ListSumBenchmark

`ReturnValueValidator` használata hibás benchmark eredmények felismerésére.

---

## 7. MathBenchmark / MathBenchmark2

Matematikai műveletek összehasonlítása:

* szorzás
* `Math.Pow`

---

## 8. ThreadingBenchmark

Threading diagnosztika használata:

```csharp
[ThreadingDiagnoser]
```

Segít:

* lock contention,
* deadlock,
* threading problémák elemzésében.

---

## 9. ReturnValueValidationBenchmark

Különböző összegzési módszerek:

* `for`
* `foreach`
* LINQ

Valamint egy szándékosan hibás implementáció.

---

## 10. StringJoinBuilderBenchmarks

Összehasonlítás:

* `string.Join`
* `StringBuilder`

Baseline benchmark használatával.

---

# Használt attribútumok

## Osztályszintű attribútumok

| Attribútum             | Leírás                          |
| ---------------------- | ------------------------------- |
| `MemoryDiagnoser`      | Memóriahasználat mérése         |
| `ShortRunJob`          | Rövidebb benchmark futás        |
| `ThreadingDiagnoser`   | Szálkezelési problémák elemzése |
| `ReturnValueValidator` | Visszatérési érték ellenőrzése  |

---

## Metódusszintű attribútumok

| Attribútum        | Leírás                        |
| ----------------- | ----------------------------- |
| `Benchmark`       | Benchmark metódus             |
| `GlobalSetup`     | Inicializálás benchmark előtt |
| `GlobalCleanup`   | Takarítás benchmark után      |
| `Params`          | Paraméterezés                 |
| `Arguments`       | Metódus argumentumok          |
| `ArgumentsSource` | Dinamikus paraméterforrás     |
| `ParamsSource`    | Paraméterforrás               |

---

# Validátorok

## ReturnValueValidator

Ellenőrzi, hogy a benchmarkok azonos eredményt adnak-e vissza.

Példa:

```csharp
[ReturnValueValidator(true)]
```

---

## BaselineValidator

Biztosítja, hogy csak egy baseline benchmark legyen.

---

## JitOptimizationsValidator

Figyelmeztet nem optimalizált build esetén.

---

# Projektstruktúra

```text
Benchmark/
│
├── Program.cs
├── StringBenchmark
├── ListBenchmark
├── LoopBenchmark
├── MathBenchmark
├── ThreadingBenchmark
└── ...
```

---

# Példa eredmény

```text
| Method              | Mean      | Error    | StdDev   |
|-------------------- |----------:|----------:|----------:|
| StringConcat        | 12.34 ns  | 0.12 ns  | 0.10 ns  |
| StringInterpolation | 15.67 ns  | 0.15 ns  | 0.13 ns  |
```

---

# Fontos megjegyzések

* Benchmarkot mindig `Release` módban futtass.
* Debug módban az eredmények torzak lehetnek.
* Az első futás során a JIT fordítás extra időt vehet igénybe.
* A BenchmarkDotNet automatikusan kezeli a warmup és mérési ciklusokat.

---

# Hasznos linkek

* [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/articles/overview.html?utm_source=chatgpt.com)
* [BenchmarkDotNet GitHub Repository](https://github.com/dotnet/BenchmarkDotNet?utm_source=chatgpt.com)

---

# Licenc

MIT License
