using Own.Storage;
using Own.Types;
using Own.Permanent;
using System;
using Inclusions;
using Own.MarkupHandlers;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void HandleAndSaveProfilesByTimerVoid()
        {
            if (Fast.GetProfilesOnPreSaveLineCountLocked() > Constants.Zero)
            {
                PreProfile bag = Fast.PreSaveProfilesLineDequeueLocked();

                if (!string.IsNullOrWhiteSpace(bag.AboutMe)
                    && bag.AboutMe.Length > Constants.Zero
                    && bag.AboutMe.Length <= Constants.MaxReplyMessageTextLength
                    && bag.Flags.Length == Constants.ProfileQuestionsCount
                    && bag.File.Length < Constants.MaxProfileImageSizeBytes
                    && bag.File.Length > Constants.MinProfileImageSizeBytes
                    )
                {
                    Own.Database.Profile profile = new Own.Database.Profile
                    {
                        AccountId = bag.AccountId,
                        PhotoBase64Jpeg = Convert.ToBase64String(Graphics.GetScaledProfileImage(bag.File)),
                        AboutMe = bag.AboutMe,
                        PublicationDate = DateTime.Now,
                        CanPlayChess = bag.Flags[Constants.Zero] ? 1 : 0,
                        WantToMeetInFirstWeek = bag.Flags[Constants.One] ? 1 : 0,
                        CanSupportALargeFamily = bag.Flags[2] ? 1 : 0,
                        HaveBadHabits = bag.Flags[3] ? 1 : 0,
                        HaveChildren = bag.Flags[4] ? 1 : 0,
                        IsAdult = bag.Flags[5] ? 1 : 0,
                        HadRelationship = bag.Flags[6] ? 1 : 0,
                        LikeReading = bag.Flags[7] ? 1 : 0,
                        HaveProfession = bag.Flags[8] ? 1 : 0,
                        HavePermanentResidenceInRussia = bag.Flags[9] ? 1 : 0,
                        DoPhysicalEducation = bag.Flags[10] ? 1 : 0,
                        HaveManyHobbies = bag.Flags[11] ? 1 : 0,
                        SpeakAForeignLanguage = bag.Flags[12] ? 1 : 0,
                        LikeDriving = bag.Flags[13] ? 1 : 0,
                        LikeTravelling = bag.Flags[14] ? 1 : 0,
                        PreferMindActivity = bag.Flags[15] ? 1 : 0,
                        CanMakeMinorRepairs = bag.Flags[16] ? 1 : 0,
                        IsOppositeGenderCute = bag.Flags[17] ? 1 : 0,
                        FollowADiet = bag.Flags[18] ? 1 : 0,
                        HavePets = bag.Flags[19] ? 1 : 0,
                        TakeCareOfPlants = bag.Flags[20] ? 1 : 0,
                        ReadyToWaitForALove = bag.Flags[21] ? 1 : 0,
                        ReadyToSaveFamilyForKids = bag.Flags[22] ? 1 : 0,
                        IsAltruist = bag.Flags[23] ? 1 : 0,
                        AcceptAgression = bag.Flags[24] ? 1 : 0
                    };
                    Slow.PutProfileInBaseVoid(profile);
                    Fast.AddOrUpdateOwnProfilePageLocked((int)profile.AccountId,
                        Marker
                            .GetOwnProfileMarkupFromFilledEntity(profile));
                    Fast.AddOrUpdatePublicProfilePageLocked((int)profile.AccountId,
                        Marker
                            .GetPublicProfileMarkupFromFilledEntity(profile));
                }
            }
        }

        internal static void LoadProfilesVoid()
        {
            foreach (int accountId in Fast.IterateThroughAccountIdsLocked())
            {
                Own.Database.Profile profile = Slow.GetProfileOrNullByAccountIdNullable(accountId);

                if (profile != null)
                {
                    Fast.AddOrUpdateOwnProfilePageLocked(accountId,
                    Marker.GetOwnProfileMarkupFromFilledEntity(profile));
                    Fast.AddOrUpdatePublicProfilePageLocked(accountId, Marker
                    .GetPublicProfileMarkupFromFilledEntity(profile));
                }
                else
                    Fast.AddOrUpdateOwnProfilePageLocked(accountId,
                                Marker
                                    .GetOwnProfilePrimaryUnfilledMarkup(accountId));
            }
        }
    }
}