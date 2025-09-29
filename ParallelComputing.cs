using System.Diagnostics;
using System.Collections.Concurrent;

public class ParallelComputation
{
    public void ComparePerformance()
    {
        var data = Enumerable.Range(1, 1000000).ToArray();

        // Sequential
        var sw1 = Stopwatch.StartNew();
        var sequential = data.Where(x => x % 2 == 0).Select(x => (long)x * x).Sum();
        sw1.Stop();

        // Parallel
        var sw2 = Stopwatch.StartNew();
        var parallel = data.AsParallel().Where(x => x % 2 == 0).Select(x => (long)x * x).Sum();
        sw2.Stop();

        // Implement data chunking manually
        int chunkSize = 10000;
        var sw3 = Stopwatch.StartNew();
        var chunks = System.Collections.Concurrent.Partitioner.Create(0, data.Length, chunkSize);
        long manualParallelSum = 0;
        // Implement data chunking manually
        Console.WriteLine($"Manual Parallel Sum: {manualParallelSum}");

        System.Threading.Tasks.Parallel.ForEach(chunks, range =>
        {
            long chunkSum = 0;
            for (int i = range.Item1; i < range.Item2; i++)
            {
                if (data[i] % 2 == 0)
                {
                    chunkSum += (long)data[i] * data[i];
                }
            }
            Interlocked.Add(ref manualParallelSum, chunkSum);
        });
        sw3.Stop();

        Console.WriteLine($"Sequential: {sw1.ElapsedMilliseconds}ms: result = {sequential}");
        Console.WriteLine($"Parallel: {sw2.ElapsedMilliseconds}ms: result = {parallel}");
        Console.WriteLine($"Manual Parallel: {sw3.ElapsedMilliseconds}ms: result = {manualParallelSum}");
    }

    // Demonstrate Task-based parallelism
    public void TaskBasedParallelism()
    {
        Console.WriteLine("\n=== Task-Based Parallelism ===");

        var sw = Stopwatch.StartNew();

        // Create multiple tasks that can run in parallel
        var task1 = Task.Run(() => ComputeIntensiveOperation(1, 250000));
        var task2 = Task.Run(() => ComputeIntensiveOperation(250001, 500000));
        var task3 = Task.Run(() => ComputeIntensiveOperation(500001, 750000));
        var task4 = Task.Run(() => ComputeIntensiveOperation(750001, 1000000));

        // Wait for all tasks to complete and get results
        Task.WaitAll(task1, task2, task3, task4);

        var totalResult = task1.Result + task2.Result + task3.Result + task4.Result;
        sw.Stop();

        Console.WriteLine($"Task-based parallel result: {totalResult} in {sw.ElapsedMilliseconds}ms");
    }

    // Demonstrate Parallel.Invoke for different operations
    public void ParallelInvokeDemo()
    {
        Console.WriteLine("\n=== Parallel.Invoke Demo ===");

        var sw = Stopwatch.StartNew();

        // Execute different methods in parallel
        Parallel.Invoke(
            () => ProcessDataTypeA(),
            () => ProcessDataTypeB(),
            () => ProcessDataTypeC()
        );

        sw.Stop();
        Console.WriteLine($"Parallel.Invoke completed in {sw.ElapsedMilliseconds}ms");
    }

    // Demonstrate thread-safe collections
    public void ConcurrentCollectionsDemo()
    {
        Console.WriteLine("\n=== Concurrent Collections Demo ===");

        var concurrentBag = new ConcurrentBag<int>();
        var concurrentQueue = new ConcurrentQueue<string>();
        var concurrentDict = new ConcurrentDictionary<int, string>();

        // Add items from multiple threads safely
        Parallel.For(0, 1000, i =>
        {
            concurrentBag.Add(i);
            concurrentQueue.Enqueue($"Item {i}");
            concurrentDict.TryAdd(i, $"Value {i}");
        });

        Console.WriteLine($"ConcurrentBag count: {concurrentBag.Count}");
        Console.WriteLine($"ConcurrentQueue count: {concurrentQueue.Count}");
        Console.WriteLine($"ConcurrentDictionary count: {concurrentDict.Count}");
    }

    // Demonstrate Partitioner for custom data partitioning
    public void PartitionerDemo()
    {
        Console.WriteLine("\n=== Partitioner Demo ===");

        var data = Enumerable.Range(1, 100000).ToArray();

        // Different partitioning strategies
        var sw1 = Stopwatch.StartNew();
        Parallel.ForEach(Partitioner.Create(data, true), ProcessItem);
        sw1.Stop();

        var sw2 = Stopwatch.StartNew();
        Parallel.ForEach(Partitioner.Create(0, data.Length, 1000), range =>
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                ProcessItem(data[i]);
            }
        });
        sw2.Stop();

        Console.WriteLine($"Default partitioning: {sw1.ElapsedMilliseconds}ms");
        Console.WriteLine($"Range partitioning: {sw2.ElapsedMilliseconds}ms");
    }

    // Demonstrate ParallelOptions for configuration
    public void ParallelOptionsDemo()
    {
        Console.WriteLine("\n=== ParallelOptions Demo ===");

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount / 2, // Use half the cores
            CancellationToken = CancellationToken.None
        };

        var sw = Stopwatch.StartNew();
        Parallel.For(0, 1000000, options, i =>
        {
            // Some work
            Math.Sqrt(i);
        });
        sw.Stop();

        Console.WriteLine($"Limited parallelism ({options.MaxDegreeOfParallelism} cores): {sw.ElapsedMilliseconds}ms");
    }

    // Helper methods
    private long ComputeIntensiveOperation(int start, int end)
    {
        long sum = 0;
        for (int i = start; i <= end; i++)
        {
            if (i % 2 == 0)
            {
                sum += (long)i * i;
            }
        }
        return sum;
    }

    private void ProcessDataTypeA()
    {
        Thread.Sleep(100); // Simulate work
        Console.WriteLine("Processing data type A completed");
    }

    private void ProcessDataTypeB()
    {
        Thread.Sleep(150); // Simulate work
        Console.WriteLine("Processing data type B completed");
    }

    private void ProcessDataTypeC()
    {
        Thread.Sleep(120); // Simulate work
        Console.WriteLine("Processing data type C completed");
    }

    private void ProcessItem(int item)
    {
        // Simulate some processing
        Math.Sqrt(item);
    }

    // Demonstrate all parallel computing features
    public void RunAllDemos()
    {
        Console.WriteLine("=== C# Parallel Computing Features Demo ===\n");

        ComparePerformance();
        TaskBasedParallelism();
        ParallelInvokeDemo();
        ConcurrentCollectionsDemo();
        PartitionerDemo();
        ParallelOptionsDemo();

        Console.WriteLine("\n=== Demo Complete ===");
    }
}