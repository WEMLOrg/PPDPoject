namespace TimeTableApp.Models;

public class Teachers
{
    private string teacherName{get;set;}
    private Guid teacherId{get;set;}
    private List<Guid> teacherSubjects{get;set;}

    Teachers(string teacherName, Guid teacherId, List<Guid> teacherSubjects)
    {
        this.teacherName = teacherName;
        this.teacherId = Guid.NewGuid();
        this.teacherSubjects = teacherSubjects;
    }

    public void addNewSubjectForTeacher(Guid subjectId)
    {
        this.teacherSubjects.Add(subjectId);
    }

    public Boolean doesTeacherTeachSubject(Guid subjectId)
    {
        foreach (Guid id in teacherSubjects)
        {
            if(id == subjectId)
                return true;
        }

        return false;
    }
    
    
}