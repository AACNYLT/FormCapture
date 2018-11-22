using Microsoft.EntityFrameworkCore;

namespace FormCapture
{
    public class FormContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Interview2017> Interviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=interviews.db");
        }
    }

    public class Applicant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string FileName => FirstName + LastName + "-" + Id;

        public Applicant() { }
        public Applicant(string firstName, string lastName, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
        }

        public override string ToString()
        {
            return FullName;
        }
    }

    public class Interview2017
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }

        public int Uniform { get; set; }
        public int Spirit { get; set; }
        public int Presentation { get; set; }
        public int Preparation { get; set; }
        public int Attitude { get; set; }
        public int Understanding { get; set; }

        public string Comments { get; set; }

        public string Team { get; set; }

        public string RecommendedPosition { get; set; }
        public bool Recommend { get; set; }
    }
}
