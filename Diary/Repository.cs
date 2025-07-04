﻿using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Diary.Models.Converters;
using Diary.Models;

namespace Diary
{
    internal class Repository
    {
        public List<Group> GetGroups()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.Groups.ToList();
                }
            }
            catch
            {
                return new List<Group>(); // Zwróć pustą listę w przypadku błędu
            }
        }

        public List<StudentWrapper> GetStudents(int groupId)
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var students = context
                        .Students
                        .Include(x => x.Group)
                        .Include(x => x.Ratings)
                        .AsQueryable();

                    if (groupId != 0)
                        students = students.Where(x => x.GroupId == groupId);

                    return students
                        .ToList()
                        .Select(x => x.ToWrapper())
                        .ToList();
                }
            }
            catch (Exception)
            {
                return new List<StudentWrapper>(); // Zwróć pustą listę w przypadku błędu
            }
        }

        public void DeleteStudent(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var studentToDelete = context.Students.Find(id);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDomain();
            var ratings = studentWrapper.ToRatingDomain();

            using (var context = new ApplicationDbContext())
            {
                UpdateStudentProperties(context, student);

                var studentsRatings = GetStudentsRatings(context, student);

                UpdateRate(student, ratings, context, studentsRatings, 
                    Subject.Math);
                UpdateRate(student, ratings, context, studentsRatings,
                    Subject.Technology);
                UpdateRate(student, ratings, context, studentsRatings, 
                    Subject.Physics);
                UpdateRate(student, ratings, context, studentsRatings, 
                    Subject.ForeignLang);
                UpdateRate(student, ratings, context, studentsRatings,  
                    Subject.PolishLang);

                context.SaveChanges();
            }
        }

        private static List<Rating> GetStudentsRatings(ApplicationDbContext context, Student student)
        =>  context .Ratings
                    .Where(x => x.StudentId == student.Id)
                    .ToList();

        private void UpdateStudentProperties(ApplicationDbContext context, 
            Student student)
        {
            var studentToUpdate = context.Students.Find(student.Id);
            studentToUpdate.Activities = student.Activities;
            studentToUpdate.Comments = student.Comments;
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.GroupId = student.GroupId;
        }

        private static void UpdateRate(Student student, List<Rating> newRatings, ApplicationDbContext context, List<Rating> studentsRatings, Subject subject)
        {
            var subRatings = studentsRatings
                    .Where(x => x.SubjectId == (int)subject)
                    .Select(x => x.Rate);

            var newSubRatings = newRatings
                .Where(x => x.SubjectId == (int)subject)
                .Select(x => x.Rate);

            var subRatingsToDelete = subRatings.Except(newSubRatings).ToList();
            var subhRatingsToAdd = newSubRatings.Except(subRatings).ToList();

            subRatingsToDelete.ForEach(x =>
            {
                var ratingToDelete = context.Ratings.First(y =>
                    y.Rate == x &&
                    y.StudentId == student.Id &&
                    y.SubjectId == (int)subject);

                context.Ratings.Remove(ratingToDelete);
            });

            subhRatingsToAdd.ForEach(x =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = x,
                    StudentId = student.Id,
                    SubjectId = (int)subject
                };
                context.Ratings.Add(ratingToAdd);
            });
        }

        public void AddStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDomain();
            var ratings = studentWrapper.ToRatingDomain();

            using (var context = new ApplicationDbContext())
            {
                var dbStudent = context.Students.Add(student);

                ratings.ForEach(x =>
                {
                    x.StudentId = dbStudent.Id;
                    context.Ratings.Add(x);
                });

                context.SaveChanges();
            }
        }
    }
}
