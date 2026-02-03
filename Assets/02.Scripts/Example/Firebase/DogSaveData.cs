using Firebase.Firestore;
using System;

//개의 종류를 나타내는 도메인
[Serializable]
[FirestoreData]
public class DogSaveData
{
    //필드가 아닌 get/set인 프로퍼티여야하며
    [FirestoreProperty]
    public string Id { get; private set; }
    [FirestoreProperty]
    public string Name { get; private set; }
    [FirestoreProperty]
    public int Age { get; private set; }

    //기본 생성자가 있어야함
    public DogSaveData()
    { }

    public DogSaveData(string name, int age)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentNullException("이름이 비어있습니다");
        }

        if (age <= 0)
        {
            throw new System.ArgumentNullException("나이는 0보다 작을수 없습니다");
        }
        Name = name;
        Age = age;
    }
}

public class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Dog(string name, int age)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentNullException("이름이 비어있습니다");
        }

        if (age <= 0)
        {
            throw new System.ArgumentNullException("나이는 0보다 작을수 없습니다");
        }
        Name = name;
        Age = age;
    }
}
