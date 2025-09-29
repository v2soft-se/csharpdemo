public class LinqQueries
{
    // Sample data class
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public void RunQueries()
    {
        List<Person> people = new List<Person>();
        people.Add(new Person { Name = "Amit Sharma", Age = 25 });
        people.Add(new Person { Name = "Priya Singh", Age = 32 });
        people.Add(new Person { Name = "Rahul Verma", Age = 17 });
        people.Add(new Person { Name = "Sneha Patel", Age = 45 });
        people.Add(new Person { Name = "Vikram Gupta", Age = 21 });




        // Method syntax
        var adultsMethodSyntax = people.Where(p => p.Age >= 18).OrderBy(p => p.Age);
        Console.WriteLine("Adults (Method Syntax):" + adultsMethodSyntax.Count());
        adultsMethodSyntax.ToList().ForEach(p => Console.WriteLine($"{p.Name}, Age: {p.Age}"));
        adultsMethodSyntax.Select(p => doubleTheAge(p.Age)).ToList().ForEach(age => Console.WriteLine(age));


        // Query syntax
        var adultsQuerySyntax = from p in people
                                where p.Age >= 18
                                orderby p.Name
                                select p;

        Console.WriteLine("Adults (Query Syntax):" + adultsQuerySyntax.Count());
        adultsQuerySyntax.ToList().ForEach(p => Console.WriteLine($"{p.Name}, Age: {p.Age}"));




    }
    private int doubleTheAge(int age)
    {
        return age * 2;
    }
}