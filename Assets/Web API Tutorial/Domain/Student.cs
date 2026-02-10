using CsvHelper.Configuration.Attributes;

public class Student
{
    [Name("id")]
    public int ID;
    [Name("name")]
    public string Name;
    [Name("age")]
    public int Age;

    public Student()
    {
    }

    public Student(int id, string name ,int age)
    {
        ID = id;
        Name = name;
        Age = age;
    }
}
