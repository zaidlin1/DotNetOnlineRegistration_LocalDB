using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Courses.API.Models
{
    [DataContract]
    public partial class MyLesson
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? Title { get; set; }
        public string? Day { get; set; }
        public string? Time { get; set; }

        [JsonIgnore]
   
        public virtual Course? Course { get; set; }
    }
}
