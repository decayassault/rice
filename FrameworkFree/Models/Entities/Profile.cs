using System.ComponentModel.DataAnnotations;
using System;
namespace Own.Database
{
    public partial class Profile
    {
        [Key]
        public long Id { get; set; }
        public long AccountId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PhotoBase64Jpeg { get; set; }
        public string AboutMe { get; set; }
        public long CanPlayChess { get; set; }
        public long WantToMeetInFirstWeek { get; set; }
        public long CanSupportALargeFamily { get; set; }
        public long HaveBadHabits { get; set; }
        public long HaveChildren { get; set; }
        public long IsAdult { get; set; }
        public long HadRelationship { get; set; }
        public long LikeReading { get; set; }
        public long HaveProfession { get; set; }
        public long HavePermanentResidenceInRussia { get; set; }
        public long DoPhysicalEducation { get; set; }
        public long HaveManyHobbies { get; set; }
        public long SpeakAForeignLanguage { get; set; }
        public long LikeDriving { get; set; }
        public long LikeTravelling { get; set; }
        public long PreferMindActivity { get; set; }
        public long CanMakeMinorRepairs { get; set; }
        public long IsOppositeGenderCute { get; set; }
        public long FollowADiet { get; set; }
        public long HavePets { get; set; }
        public long TakeCareOfPlants { get; set; }
        public long ReadyToWaitForALove { get; set; }
        public long ReadyToSaveFamilyForKids { get; set; }
        public long IsAltruist { get; set; }
        public long AcceptAgression { get; set; }
    }
}