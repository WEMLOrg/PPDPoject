namespace TimeTableApp.Models;

public class Teacher
{
    public string teacherName{get;set;}
    public Guid _id{get;set;}
    public List<Guid> teacherSubjects{get;set;}

    public Teacher(string teacherName, List<Guid> teacherSubjects)
    {
        this.teacherName = teacherName;
        this._id = Guid.NewGuid();
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