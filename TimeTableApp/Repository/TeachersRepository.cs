using TimeTableApp.Models;

namespace TimeTableApp.Repository;

public class TeachersRepository
{
    private List<Teacher> TeachersList;
    private SubjectsRepository SubjectsRepository;

    TeachersRepository()
    {
        TeachersList = new List<Teacher>();
        
        List<Guid> TeacherSubjectList1 = new List<Guid>();
        TeacherSubjectList1.Add(SubjectsRepository.GetSubjectGuid("romana"));
        TeacherSubjectList1.Add(SubjectsRepository.GetSubjectGuid("mate"));
        
        TeachersList.Add(new Teacher("delia", TeacherSubjectList1 ));
        
        List<Guid> teacherSubjectList2 = new List<Guid>
        {
            SubjectsRepository.GetSubjectGuid("engleza"),
            SubjectsRepository.GetSubjectGuid("germana")
        };
        TeachersList.Add(new Teacher("Ion", teacherSubjectList2));

        List<Guid> teacherSubjectList3 = new List<Guid>
        {
            SubjectsRepository.GetSubjectGuid("istorie"),
            SubjectsRepository.GetSubjectGuid("educatie civica"),
            SubjectsRepository.GetSubjectGuid("romana")
        };
        TeachersList.Add(new Teacher("Maria", teacherSubjectList3));

        List<Guid> teacherSubjectList4 = new List<Guid>
        {
            SubjectsRepository.GetSubjectGuid("geografie"),
            SubjectsRepository.GetSubjectGuid("germana"),
            SubjectsRepository.GetSubjectGuid("sport")
        };
        TeachersList.Add(new Teacher("Andrei", teacherSubjectList4));

    }

    public void AddTeacher(Teacher teacher)
    {
        TeachersList.Add(teacher);
    }
    public List<Teacher> GetTeachers()
    {
        return TeachersList;
    } 
}