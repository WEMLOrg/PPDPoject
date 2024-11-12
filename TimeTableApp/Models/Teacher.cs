namespace TimeTableApp.Models;

public class Teacher
{
    public string teacherName{get;set;}
    public Guid _id{get;set;}
    public List<Guid> teacherSubjects{get;set;}

    public Teacher(Guid id, string teacherName, List<Guid> teacherSubjects)
    {
        this.teacherName = teacherName;
        this._id = id;
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

    public static bool operator ==(Teacher t1, Teacher t2)
    {
        if (ReferenceEquals(t1, t2)) return true;
        if (t1 is null || t2 is null) return false;
        return t1._id == t2._id;
    }
    public static bool operator !=(Teacher t1, Teacher t2)
    {
        return !(t1 == t2);
    }


}