using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
Osztályszintű attribútumok:

[BenchmarkDotNet.Attributes.MemoryDiagnoser]: Memóriahasználat mérésére szolgál.
[BenchmarkDotNet.Attributes.ShortRunJob]: Gyors futtatású tesztekhez használatos.
[BenchmarkDotNet.Attributes.LongRunJob]: Hosszabb ideig futó tesztekhez.
[BenchmarkDotNet.Attributes.WarmupCount(int count)]: A bemelegítési ciklusok számát állítja be.
[BenchmarkDotNet.Attributes.IterationCount(int count)]: A mérési ciklusok számát állítja be.
[BenchmarkDotNet.Attributes.InvocationCount(int count)]: A mérési ciklusokon belüli meghívások számát állítja be.
[BenchmarkDotNet.Attributes.GcServer(bool value)]: Szerver GC használatának beállítására szolgál.
[BenchmarkDotNet.Attributes.GcConcurrent(bool value)]: Párhuzamos GC használatának beállítására szolgál.
[BenchmarkDotNet.Attributes.ThreadingDiagnoser]: Szálkezelési problémák elemzésére használható.
[BenchmarkDotNet.Attributes.HardwareCounters(HardwareCounter[] counters)]: Hardveres számlálók mérésére használható.

Módszerszintű attribútumok:

[BenchmarkDotNet.Attributes.Benchmark]: A mérésre szánt metódust jelöli.
[BenchmarkDotNet.Attributes.Setup]: A mérési metódus előtt futó beállítási metódust jelöli.
[BenchmarkDotNet.Attributes.Cleanup]: A mérési metódus után futó takarítási metódust jelöli.
[BenchmarkDotNet.Attributes.IterationSetup]: Minden iteráció előtt futó beállítási metódust jelöli.
[BenchmarkDotNet.Attributes.IterationCleanup]: Minden iteráció után futó takarítási metódust jelöli.
[BenchmarkDotNet.Attributes.GlobalSetup]: Minden benchmark futás előtt futó beállítási metódust jelöli.
[BenchmarkDotNet.Attributes.GlobalCleanup]: Minden benchmark futás után futó takarítási metódust jelöli.
[BenchmarkDotNet.Attributes.Arguments(object[] values)]: Argumentumok átadására szolgál a mérési metódusnak.
[BenchmarkDotNet.Attributes.ArgumentsSource(string methodName)]: Argumentumok forrásaként szolgáló metódust jelöl.
[BenchmarkDotNet.Attributes.Params(params object[] values)]: Paraméterek átadására szolgál a mérési metódusnak.
[BenchmarkDotNet.Attributes.ParamsSource(string methodName)]: Paraméterek forrásaként szolgáló metódust jelöl.

Egyéb fontos attribútumok:

[BenchmarkDotNet.Attributes.Orderer(Type ordererType)]: A benchmarkok sorrendjének beállítására szolgál.
[BenchmarkDotNet.Attributes.Exporter(params Type[] exporterTypes)]: Az eredmények exportálásának beállítására szolgál.
[BenchmarkDotNet.Attributes.Column(string columnName)]: Egyéni oszlopok hozzáadására szolgál az eredményekhez.
[BenchmarkDotNet.Attributes.Config(Type configType)]: Egyéni konfiguráció használatára szolgál.

Néhány fontos validátor:

BaselineValidator:
 - Ellenőrzi, hogy egy benchmark osztályban legfeljebb egy benchmark rendelkezik-e a [Baseline] attribútummal.
 - Ez a validátor kötelező, és a benchmarkok futtatása előtt lefut.
JitOptimizationsValidator:
 - Ellenőrzi, hogy a hivatkozott szerelvények optimalizáltak-e.
 - Ez a validátor opcionális, és alapértelmezés szerint engedélyezve van.
ExecutionValidator:
 - Ellenőrzi, hogy a benchmarkok futtathatók-e azáltal, hogy mindegyiket egyszer lefuttatja.
 - Ez a validátor opcionális.
ReturnValueValidator:
 - Ellenőrzi, hogy a nem void benchmarkok egyenlő értékeket adnak-e vissza.
 - Ez a validátor opcionális.
 */

namespace Benchmark
{    
    public class StringBenchmark
    {
        private string text = "Hello, World!";

        [BenchmarkCategory("String", "Concat")]
        [Benchmark]
        public string StringConcat() => text + "!";

        [BenchmarkCategory("String", "Interpolation")]
        [Benchmark]
        public string StringInterpolation() => $"{text}!";
    }

    [MemoryDiagnoser]
    [ShortRunJob]
    public class ListBenchmark
    {
        private List<int> list;

        [Params(1_000, 10_000)]
        public int ListSize;

        [GlobalSetup]
        public void Setup()
        {
            list = new List<int>();
            for (int i = 0; i < ListSize; i++)
            {
                list.Add(i);
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            list = null;
        }

        [Benchmark]
        public int ListSum()
        {
            int sum = 0;
            foreach (int item in list)
            {
                sum += item;
            }
            return sum;
        }

        [Benchmark]
        public void ListSort()
        {
            list.Sort();
        }

        [Benchmark]
        public void ListSortLambda()
        {
            list.Sort((x, y) => x.CompareTo(y));
        }
    }

    public class LoopBenchmark
    {
        [Benchmark]
        [IterationCount(5)] //Ez az attribútum határozza meg, hogy a benchmark hányszor fusson le egy iterációban.
        [WarmupCount(2)] //Ez az attribútum határozza meg, hogy a benchmark hányszor fusson le a tényleges mérések előtt, a JIT fordítás és egyéb bemelegítési folyamatok elvégzésére.
        //[MeasurementCount(10)] //Ez az attribútum azt határozza meg, hogy a mérés hányszor fusson le.
        [InvocationCount(100)] //Ez az attribútum azt határozza meg, hogy egy mérésen belül hányszor hívódjon meg a tesztelendő metódus.

        public void Loop()
        {
            for (int i = 0; i < 100; i++)
            {
                // Valamilyen művelet
            }
        }
    }

    [MemoryDiagnoser]
    public class StringCreationBenchmark
    {
        [Benchmark]
        public string CreateString() => new string('A', 1000);
    }

    public class StringConcatBenchmark
    {
        [Benchmark]
        [Arguments("Hello", "World")]
        [Arguments("Benchmark", "DotNet")]
        public string StringConcat(string a, string b) => a + " " + b;
    }

    public class StringConcatBenchmark2
    {
        [Params("Hello", "Benchmark")]
        public string a;

        [Params("World", "DotNet")]
        public string b;

        [Benchmark]
        public string StringConcat() => a + " " + b;
    }

    public class StringConcatBenchmark3
    {
        public static IEnumerable<string[]> Data()
        {
            yield return new[] { "Hello", "World" };
            yield return new[] { "Benchmark", "DotNet" };
            yield return new[] { "Dynamic", "Params" };
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public string StringConcat(string a, string b) => a + " " + b;
    }

    public class StringConcatBenchmark4
    {
        public static IEnumerable<string> Data()
        {
            yield return "Hello";
            yield return "Benchmark";
            yield return "Dynamic";
        }

        [ParamsSource(nameof(Data))]
        public string a;

        [Params("World", "DotNet", "Params")]
        public string b;

        [Benchmark]
        public string StringConcat() => a + " " + b;
    }

    [ReturnValueValidator(true)]
    public class ListSumBenchmark
    {
        [Params(10, 100, 1000)]
        public int ListSize { get; set; }

        private int[] list;

        [GlobalSetup]
        public void Setup()
        {
            list = Enumerable.Range(0, ListSize).ToArray();
        }

        [Benchmark]
        public int Sum()
        {
            return list.Sum();
        }

        [Benchmark]
        public int SumValidator()
        {
            //10-re jo, de 100, 100-re nem jo
            return ListSize * (ListSize - 1) / 2;
        }
    }

    [ReturnValueValidator(true)]
    public class MathBenchmark
    {
        [Params(10, 100)]
        //[Params(2, 4, 8, 16, 32, 64, 128)]
        public int Number { get; set; }

        public int Square(int number)
        {
            return number * number;
        }

        public int Power(int number)
        {
            return (int)Math.Pow(number, 2);
        }

        [Benchmark]
        public int SqWithParameter()
        {
            return Square(Number);
        }

        [Benchmark]
        public int Sq2WithParameter()
        {
            return Power(Number);
        }
    }

    [ReturnValueValidator(true)]
    public class MathBenchmark2
    {
        [Params(10, 20)]
        public int Parameter { get; set; }

        [Params(2)]
        public int Number { get; set; }

        [Benchmark]
        //[Arguments(2)]
        //public int Square(int number)
        public int Square()
        {
            int result = 1;

            for (int i = 0; i < Parameter; i++)
            {
                result *= Number;
            }

            return result;
        }

        [Benchmark]
        public int Square2()
        {
            return (int)Math.Pow(Number, Parameter);
        }
    }

    //.NET Core 3-tol
    [ThreadingDiagnoser]//Segít azonosítani a szálak közötti versengést, a holtpontokat és más szálkezelési problémákat.
    public class ThreadingBenchmark
    {
        private object lockObject = new object();
        private int counter = 0;

        [Benchmark]
        public void IncrementCounter()
        {
            lock (lockObject)
            {
                counter++;
            }
        }
    }

    public class ReturnValueValidationBenchmark
    {
        private List<int> data;

        [GlobalSetup]
        public void Setup()
        {
            data = new List<int> { 1, 2, 3, 4, 5 };
        }

        [Benchmark]
        public int SumUsingForLoop()
        {
            int sum = 0;
            for (int i = 0; i < data.Count; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        [Benchmark]
        public int SumUsingForEachLoop()
        {
            int sum = 0;
            foreach (int item in data)
            {
                sum += item;
            }
            return sum;
        }

        [Benchmark]
        public int SumUsingLinq()
        {
            return data.Sum();
        }

        [Benchmark]
        public int SumUsingFaultyLogic()
        {
            // Ez a metódus hibás eredményt ad vissza
            int sum = 0;
            for (int i = 0; i < data.Count - 1; i++) // Szándékosan rossz ciklusvég
            {
                sum += data[i];
            }
            return sum;
        }
    }

    public class ReturnValueValidationConfig : ManualConfig
    {
        public ReturnValueValidationConfig()
        {
            Add(DefaultConfig.Instance);
            AddValidator(ReturnValueValidator.FailOnError);
        }
    }

    [MemoryDiagnoser]
    [ReturnValueValidator(failOnError: true)]
    public class StringJoinBuilderBenchmarks
    {
        [Params(5, 50, 500)]
        public int N { get; set; }

        [Benchmark(Baseline = true)]
        public string StringJoin()
        {
            return string.Join(", ", Enumerable.Range(0, N).Select(i => i.ToString()));
        }

        [Benchmark]
        public string StringBuilder()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < N - 1; i++)
            {
                sb.Append(i);
                sb.Append(", ");
            }

            sb.Append(N - 1);

            return sb.ToString();
        }
    }

    internal class Program
    {
        //futtatas Release konfiguracioban, debugging nelkul
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StringBenchmark>();
            //var summary = BenchmarkRunner.Run<ListBenchmark>();
            //var summary = BenchmarkRunner.Run<LoopBenchmark>();
            //var summary = BenchmarkRunner.Run<StringCreationBenchmark>();
            //var summary = BenchmarkRunner.Run<StringConcatBenchmark>();
            //var summary = BenchmarkRunner.Run<StringConcatBenchmark2>();
            //var summary = BenchmarkRunner.Run<StringConcatBenchmark3>();
            //var summary = BenchmarkRunner.Run<StringConcatBenchmark4>();
            //var summary = BenchmarkRunner.Run<ListSumBenchmark>();
            //var summary = BenchmarkRunner.Run<MathBenchmark>();
            //var summary = BenchmarkRunner.Run<MathBenchmark2>();
            //var summary = BenchmarkRunner.Run<ThreadingBenchmark>();
            //var summary = BenchmarkRunner.Run<ReturnValueValidationBenchmark>(new ReturnValueValidationConfig());
            //var summary = BenchmarkRunner.Run<StringJoinBuilderBenchmarks>();
        }
    }
}
