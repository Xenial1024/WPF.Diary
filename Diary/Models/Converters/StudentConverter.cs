using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Converters
{
    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student model)
        {
            return new StudentWrapper
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Comments = model.Comments,
                Activities = model.Activities,
                Group = new GroupWrapper
                {
                    Id = model.Group.Id,
                    Name = model.Group.Name
                },
                Math = GetSubjectRatings(model, Subject.Math),
                Physics = GetSubjectRatings(model, Subject.Physics),
                PolishLang = GetSubjectRatings(model, Subject.PolishLang),
                ForeignLang = GetSubjectRatings(model, Subject.ForeignLang),
                Technology = GetSubjectRatings(model, Subject.Technology)
            };
        }

        private static string GetSubjectRatings(Student model, Subject subject)
        {
            return string.Join(", ", model.Ratings
                .Where(y => y.SubjectId == (int)subject)
                .Select(y => y.Rate));
        }

        public static Student ToDao(this StudentWrapper model)
        {
            return new Student
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GroupId = model.Group.Id,
                Comments = model.Comments,
                Activities = model.Activities
            };
        }
        private static void SaveRatings(List<Rating> ratings, string subjectRatings, int studentId, Subject subject)
        {
            if (!string.IsNullOrWhiteSpace(subjectRatings))
            {
                subjectRatings.Split(',')
                    .ToList()
                    .ForEach(rate =>
                        ratings.Add(new Rating
                        {
                            Rate = int.Parse(rate),
                            StudentId = studentId,
                            SubjectId = (int)subject
                        }));
            }
        }

        public static List<Rating> ToRatingDao(this StudentWrapper model)
        {
            var ratings = new List<Rating>();

            SaveRatings(ratings, model.Math, model.Id, Subject.Math);
            SaveRatings(ratings, model.Physics, model.Id, Subject.Physics);
            SaveRatings(ratings, model.PolishLang, model.Id, Subject.PolishLang);
            SaveRatings(ratings, model.ForeignLang, model.Id, Subject.ForeignLang);
            SaveRatings(ratings, model.Technology, model.Id, Subject.Technology);

            return ratings;
        }
    }
}
