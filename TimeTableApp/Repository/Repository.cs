using TimeTableApp.Models;

namespace TimeTableApp.Repository;

public class Repository
{
    private List<Teacher> TeachersList;
    private List<Subject> SubjectsList;
    private List<Room> RoomsList;

    Repository()
    {
        TeachersList = new List<Teacher>();
        SubjectsList = new List<Subject>();
        RoomsList  = new List<Room>();
        
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
        
        List<Guid> TeacherSubjectList1 = new List<Guid>();
        TeacherSubjectList1.Add(SubjectsList[0]._id);
        TeacherSubjectList1.Add(SubjectsList[1]._id);
        
        TeachersList.Add(new Teacher("delia", TeacherSubjectList1 ));
        
        List<Guid> teacherSubjectList2 = new List<Guid>
        {
            SubjectsList[2]._id,
            SubjectsList[3]._id
        };
        TeachersList.Add(new Teacher("Ion", teacherSubjectList2));

        List<Guid> teacherSubjectList3 = new List<Guid>
        {
            SubjectsList[4]._id,
            SubjectsList[5]._id,
            SubjectsList[6]._id
        };
        TeachersList.Add(new Teacher("Maria", teacherSubjectList3));

        List<Guid> teacherSubjectList4 = new List<Guid>
        {
            SubjectsList[7]._id,
            SubjectsList[8]._id,
            SubjectsList[9]._id
        };
        TeachersList.Add(new Teacher("Andrei", teacherSubjectList4));

        RoomsList.Add(new Room(30));
        RoomsList.Add(new Room(50));
        RoomsList.Add(new Room(35));
        RoomsList.Add(new Room(45));
        RoomsList.Add(new Room(100));
        RoomsList.Add(new Room(56));
        RoomsList.Add(new Room(80));
        RoomsList.Add(new Room(20));
        RoomsList.Add(new Room(80));
        RoomsList.Add(new Room(100));

    }

    public void AddTeacher(Teacher teacher)
    {
        TeachersList.Add(teacher);
    }

    public void AddSubject(Subject subject)
    {
        SubjectsList.Add(subject);
    }

    public void AddRoom(Room room)
    {
        RoomsList.Add(room);
    }
}