public class  AsyncAwaitExample
{
    

void doWork()
{
    Thread.Sleep(2000); // Simulates work by waiting 2 seconds
    Console.WriteLine("Work done");
}
async Task<int> doWorkAsync(int delay)
{
    if (delay < 0)
    {
        throw new ArgumentException("Delay must be positive");
    }
    else if (delay == 0)
    {
        throw new ArgumentException("Delay cannot be zero");
    }
    await Task.Delay(delay * 2000); // Asynchronously waits 2 seconds
                                    //Why delay value is not getting printed correctly here?

    Console.WriteLine($"Work done async with delay # {delay}");
    return 42 * delay;
}   

public async Task TestAsyncMethod()
{
string[] imageFiles = { "img1.jpg", "img2.jpg", "img3.jpg" };
Task[] tasks = imageFiles.Select(img => ProcessImageAsync(img)).ToArray();

await Task.WhenAll(tasks);
Console.WriteLine("All images processed!");
}
async Task ProcessImageAsync(string fileName)
{
    // Simulate image processing
    await Task.Delay(1000);
    Console.WriteLine($"Processed  {fileName}");
}
}
