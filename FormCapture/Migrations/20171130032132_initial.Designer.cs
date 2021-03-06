﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using FormCapture.Classes;

namespace FormCapture.Migrations
{
    [DbContext(typeof(FormContext))]
    [Migration("20171130032132_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
