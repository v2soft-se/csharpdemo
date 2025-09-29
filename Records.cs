public class RecordsDemo
{
    // Demonstrating record features
    public record Person(string Name, int Age);

    public void ShowRecordFeatures()
    {
        Person person1 = new Person("Alice", 30);
        Person person2 = new Person("Bob", 25);

        // Displaying record properties
        Console.WriteLine($"Name: {person1.Name}, Age: {person1.Age}");
        Console.WriteLine($"Name: {person2.Name}, Age: {person2.Age}");

        // Records are immutable
        // person1.Age = 31; // This will cause a compile-time error

        // Using with-expressions to create modified copies
        Person person3 = person1 with { Age = 31 };
        Console.WriteLine($"Name: {person3.Name}, Age: {person3.Age}");
        if (person1 == person3)
        {
            Console.WriteLine("person1 and person3 are equal");
        }
        else
        {
            Console.WriteLine("person1 and person3 are not equal");
        }
    }
}