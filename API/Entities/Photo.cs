using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } //url of where to find the photo
        public bool IsMain { get; set; } //to check whether this is the user's main photo
        public string PublicId { get; set; } //for storing the uploaded photos later on...
        public int AppUserId { get; set; } //=>one-to-many relationship;
        public AppUser AppUser { get; set; } //=>one-to-many relationship;
    }
}