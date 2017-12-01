using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FormCapture;

namespace FormCapture.Migrations
{
    [DbContext(typeof(FormContext))]
    partial class FormContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("FormCapture.Applicant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Applicants");
                });

            modelBuilder.Entity("FormCapture.Interview2017", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApplicantId");

                    b.Property<int>("Attitude");

                    b.Property<string>("Comments");

                    b.Property<int>("Preparation");

                    b.Property<int>("Presentation");

                    b.Property<bool>("Recommend");

                    b.Property<string>("RecommendedPosition");

                    b.Property<int>("Spirit");

                    b.Property<string>("Team");

                    b.Property<int>("Understanding");

                    b.Property<int>("Uniform");

                    b.HasKey("Id");

                    b.ToTable("Interviews");
                });
        }
    }
}
