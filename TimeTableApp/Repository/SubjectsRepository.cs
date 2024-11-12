using TimeTableApp.Models;

namespace TimeTableApp.Repository;

public class SubjectsRepository
{
    private List<Subject> SubjectsList;

    public SubjectsRepository()
    {
        SubjectsList = new List<Subject>();
        SubjectsList.Add(new Subject(30, "mate"));
        SubjectsList.Add(new Subject(30, "romana"));
        SubjectsList.Add(new Subject(50, "engleza"));
        SubjectsList.Add(new Subject(15, "chimie"));
        SubjectsList.Add(new Subject(30, "georgrafie"));
        SubjectsList.Add(new Subject(30, "istorie"));
        SubjectsList.Add(new Subject(30, "educatie civica"));
        SubjectsList.Add(new Subject(30, "sport"));
        SubjectsList.Add(new Subject(28, "germana"));
        SubjectsList.Add(new Subject(31, "franceza"));
    }

    public Guid GetSubjectGuid(String name)
    {
        foreach (Subject subject in SubjectsList )
        {
            if (subject.name == name)
                return subject._id;
        }

        return Guid.Empty;
    }
    public void AddSubject(Subject subject)
    {
        SubjectsList.Add(subject);
    }
    public List<Subject> GetSubjects()
    {
        return SubjectsList;
    }
}