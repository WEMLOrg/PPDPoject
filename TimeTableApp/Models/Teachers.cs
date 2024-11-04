namespace TimeTableApp.Models;

public class Teachers
{
    public string teacherName{get;set;}
    public Guid teacherId{get;set;}
    public List<Guid> teacherSubjects{get;set;}

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