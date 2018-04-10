using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myFintess
{
    [Table("Exercise")]
    class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Int16 Weight { get; set; }
        public Int16 Iteration { get; set; }
    }

    [Table("Training")]
    class Training
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Int16 Weight { get; set; }
        public Int16 Iteration { get; set; }
        public int ExerciseId { get; set; }
        public Exercise ex { get; set; }
    }

   
}
