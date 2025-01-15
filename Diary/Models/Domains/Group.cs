using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Diary.Models.Domains
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }

        public Group()
        {
            Students = new Collection<Student>();
        }
    }
}
