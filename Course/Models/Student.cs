using System;
using System.Collections.Generic;

namespace Course.Models;

public partial class Student
{
    public string Pin { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime TimeCreated { get; set; }

    public virtual ICollection<StudentsCoursesXref> StudentsCoursesXrefs { get; set; } = new List<StudentsCoursesXref>();
}
