using System;
using System.Collections.Generic;

namespace ManagerProjects.Classes
{
    public enum Status
    {
        InDevelopment,
        InProduction,
        InClose,
        InArchive
    }

    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDevelopmentDate { get; set; }
        public DateTime StartProductionDate { get; set; }
        public string CurrentVersion { get; set; }
        public Status Status { get; set; }
        public string ReleasesUrl { get; set; }
        public string GitUrl { get; set; }
        public int AmountWorkplaces { get; set; }
        public string Comment { get; set; }
        public Developer ResponsibleDeveloper { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}