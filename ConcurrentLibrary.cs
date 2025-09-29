using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;

public class ConcurrentCollectionsDemo
{
    public void ConcurrentDictionaryDemo()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        ConcurrentDictionary<int, string> concurrentDict = new ConcurrentDictionary<int, string>();
        bool junkValue = false;
        // create 3 Tasks to add items concurrently
        Task t1 = Task.Run(async () =>
        {

            for (int i = 0; i < 5000000; i++)
            {
                // Modify the following read block to read 100 value before i before insetting new value to increase read operations before write
                for (int j = i - 100; j < i; j++)
                {
                    if (concurrentDict.ContainsKey(j))
                    {
                        junkValue = true;
                        // Console.WriteLine($"Key {j} already exists!");
                    }
                }

                //await Task.Delay(RandomNumberGenerator.GetInt32(1, 100)); // Simulate async work
                concurrentDict.TryAdd(i, $"Value_1_{i}");
            }
        });
        Task t2 = Task.Run(async () =>
        {
            for (int i = 5000000; i < 10000000; i++)
            {
                //await Task.Delay(RandomNumberGenerator.GetInt32(1, 100)); // Simulate async work
                for (int j = i - 100; j < i; j++)
                {
                    if (concurrentDict.ContainsKey(j))
                    {
                        junkValue = true;
                        // Console.WriteLine($"Key {j} already exists!");
                    }
                }

                concurrentDict.TryAdd(i, $"Value_2_{i}");
            }
        });
        Task t3 = Task.Run(async () =>
        {
            for (int i = 10000000; i < 15000000; i++)
            {
                // Modify the following read block to read 100 value before i before insetting new value to increase read operations before write
                for (int j = i - 100; j < i; j++)
                {
                    if (concurrentDict.ContainsKey(i - 2))
                    {
                        junkValue = true;
                        // Console.WriteLine($"Key {i - 2} already exists!");
                    }
                }
                //await Task.Delay(RandomNumberGenerator.GetInt32(1, 100));
                concurrentDict.TryAdd(i, $"Value_3_{i}");
            }
        });

        Task.WaitAll(t1, t2, t3);
        stopwatch.Stop();
        Console.WriteLine($"Time taken to add items in parallel: {stopwatch.ElapsedMilliseconds}ms");

        // List out the keys, there should only be 50 keys
        // Console.WriteLine($"Total items in ConcurrentDictionary: {concurrentDict.Count}");

        // // Display the contents of the ConcurrentDictionary
        // foreach (var kvp in concurrentDict)
        // {
        //     Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
        // }
        SimpleDictionaryDemo();


    }

    public void SimpleDictionaryDemo()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Dictionary<int, string> simpleDict = new Dictionary<int, string>();
        bool junkValue = false;
        // create 3 Tasks to add items concurrently
        Task t1 = Task.Run(async () =>
        {
            for (int i = 0; i < 5000000; i++)
            {
                //await Task.Delay(RandomNumberGenerator.GetInt32(1, 100)); // Simulate async work
                lock (simpleDict)
                {
                    for (int j = i - 100; j < i; j++)
                    {
                        if (simpleDict.ContainsKey(i - 2))
                        {
                            junkValue = true;
                            // Console.WriteLine($"Key {i - 2} already exists!");
                        }
                    }
                    simpleDict[i] = $"Value_1_{i}";
                }
            }
        });
        Task t2 = Task.Run(async () =>
        {
            for (int i = 5000000; i < 10000000; i++)
            {
                // await Task.Delay(RandomNumberGenerator.GetInt32(1, 100)); // Simulate async work
                lock (simpleDict)
                {
                    for (int j = i - 100; j < i; j++)
                    {
                        if (simpleDict.ContainsKey(i - 2))
                        {
                            junkValue = true;
                            // Console.WriteLine($"Key {i - 2} already exists!");
                        }
                        simpleDict[i] = $"Value_2_{i}";
                    }
                }
            }
        });
        Task t3 = Task.Run(async () =>
        {
            for (int i = 10000000; i < 15000000; i++)
            {
                // await Task.Delay(RandomNumberGenerator.GetInt32(1, 100));
                lock (simpleDict)
                {
                    for (int j = i - 100; j < i; j++)
                    {
                        if (simpleDict.ContainsKey(i - 2))
                        {
                            junkValue = true;
                            // Console.WriteLine($"Key {i - 2} already exists!");
                        }
                        simpleDict[i] = $"Value_3_{i}";
                    }
                }
            }
        });

        Task.WaitAll(t1, t2, t3);
        stopwatch.Stop();
        Console.WriteLine($"Time taken to add items in normal dictionary: {stopwatch.ElapsedMilliseconds}ms");

        // List out the keys, there should only be 50 keys
        // Console.WriteLine($"Total items in Simple Dictionary: {simpleDict.Count}");

        // // Display the contents of the Simple Dictionary
        // foreach (var kvp in simpleDict)
        // {
        //     Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
        // }
    }
}