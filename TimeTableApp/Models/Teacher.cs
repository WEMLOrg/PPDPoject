namespace TimeTableApp.Models;

public class Teacher
{
    private string teacherName{get;set;}
    private Guid teacherId{get;set;}
    private List<Guid> teacherSubjects{get;set;}

    public Teacher(string teacherName, List<Guid> teacherSubjects)
    {
        this.teacherName = teacherName;
        this.teacherId = Guid.NewGuid();
        this.teacherSubjects = teacherSubjects;
    }

    public void AddNewSubjectForTeacher(Guid subjectId)
    {
        this.teacherSubjects.Add(subjectId);
    }

    public Boolean DoesTeacherTeachSubject(Guid subjectId)
    {
        foreach (Guid id in teacherSubjects)
        {
            if(id == subjectId)
                return true;
        }

        return false;
    }
    
    
}