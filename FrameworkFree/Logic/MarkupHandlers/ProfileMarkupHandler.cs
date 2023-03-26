using Models;
using Logic;
namespace MarkupHandlers
{
    internal sealed class ProfileMarkupHandler
    {
        internal string GetOwnProfileMarkupFromFilledEntity(in Profile profile)
        {
            return string.Concat("<div class='l'><p>Последний раз сохранено ",
                                    profile.PublicationDate.ToString("dd.MM.yyyy"),
                                    ".</p><br /><br /><img src='data:image/jpeg;base64,",
                                    profile.PhotoBase64Gif,
                                    "' alt='Фотография' class='x'/><br /><br />",
                                    "<p>Загрузите фотографию шириной 500 и высотой 809 пикселей (иначе могут исказиться пропорции), на которой хорошо видно Ваше лицо, размером не более 500 КБ в формате JPEG.",
                                    "<br /><input type='file' id='f' name='f' accept='image/jpeg'><br /><br />",
                                    "Умеете ли играть в шахматы?<br /><input type='radio' name='q1' value='1'",
                                    profile.CanPlayChess ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q1' value='0'",
                                    !profile.CanPlayChess ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Хотите ли встретиться в первую неделю знакомства?<br /><input type='radio' name='q2' value='1'",
                                    profile.WantToMeetInFirstWeek ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q2' value='0'",
                                    !profile.WantToMeetInFirstWeek ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Можете ли обеспечивать многодетную семью?<br /><input type='radio' name='q3' value='1'",
                                    profile.CanSupportALargeFamily ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q3' value='0'",
                                    !profile.CanSupportALargeFamily ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Имеете ли вредные привычки?<br /><input type='radio' name='q4' value='1'",
                                    profile.HaveBadHabits ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q4' value='0'",
                                    !profile.HaveBadHabits ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Есть ли у Вас дети?<br /><input type='radio' name='q5' value='1'",
                                    profile.HaveChildren ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q5' value='0'",
                                    !profile.HaveChildren ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Вы совершеннолетний(-яя)?<br /><input type='radio' name='q6' value='1'",
                                    profile.IsAdult ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q6' value='0'",
                                    !profile.IsAdult ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Состояли ли в отношениях?<br /><input type='radio' name='q7' value='1'",
                                    profile.HadRelationship ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q7' value='0'",
                                    !profile.HadRelationship ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Любите ли читать?<br /><input type='radio' name='q8' value='1'",
                                    profile.LikeReading ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q8' value='0'",
                                    !profile.LikeReading ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Имеете ли профессию?<br /><input type='radio' name='q9' value='1'",
                                    profile.HaveProfession ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q9' value='0'",
                                    !profile.HaveProfession ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Имеете ли постоянное место жительства в России?<br />",
                                    "<input type='radio' name='q10' value='1'",
                                    profile.HavePermanentResidenceInRussia ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q10' value='0'",
                                    !profile.HavePermanentResidenceInRussia ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Занимаетесь ли физкультурой?<br /><input type='radio' name='q11' value='1'",
                                    profile.DoPhysicalEducation ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q11' value='0'",
                                    !profile.DoPhysicalEducation ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Много ли у Вас увлечений?<br /><input type='radio' name='q12' value='1'",
                                    profile.HaveManyHobbies ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q12' value='0'",
                                    !profile.HaveManyHobbies ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Владеете ли иностранным языком?<br /><input type='radio' name='q13' value='1'",
                                    profile.SpeakAForeignLanguage ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q13' value='0'",
                                    !profile.SpeakAForeignLanguage ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Любите ли водить автомобиль?<br /><input type='radio' name='q14' value='1'",
                                    profile.LikeDriving ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q14' value='0'",
                                    !profile.LikeDriving ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Нравится ли путешествовать?<br /><input type='radio' name='q15' value='1'",
                                    profile.LikeTravelling ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q15' value='0'",
                                    !profile.LikeTravelling ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Предпочитаете ли умственную деятельность?<br /><input type='radio' name='q16' value='1'",
                                    profile.PreferMindActivity ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q16' value='0'",
                                    !profile.PreferMindActivity ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Умеете ли делать мелкий ремонт?<br /><input type='radio' name='q17' value='1'",
                                    profile.CanMakeMinorRepairs ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q17' value='0'",
                                    !profile.CanMakeMinorRepairs ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Симпатичен ли Вам противоположный пол?<br /><input type='radio' name='q18' value='1'",
                                    profile.IsOppositeGenderCute ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q18' value='0'",
                                    !profile.IsOppositeGenderCute ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Соблюдаете ли диету?<br /><input type='radio' name='q19' value='1'",
                                    profile.FollowADiet ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q19' value='0'",
                                    !profile.FollowADiet ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Держите ли дома животных?<br /><input type='radio' name='q20' value='1'",
                                    profile.HavePets ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q20' value='0'",
                                    !profile.HavePets ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Ухаживаете ли за растениями?<br /><input type='radio' name='q21' value='1'",
                                    profile.TakeCareOfPlants ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q21' value='0'",
                                    !profile.TakeCareOfPlants ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Готовы ли ждать, пока вспыхнет любовь?<br /><input type='radio' name='q22' value='1'",
                                    profile.ReadyToWaitForALove ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q22' value='0'",
                                    !profile.ReadyToWaitForALove ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Вы готовы сохранять семью ради детей?<br /><input type='radio' name='q23' value='1'",
                                    profile.ReadyToSaveFamilyForKids ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q23' value='0'",
                                    !profile.ReadyToSaveFamilyForKids ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Вы альтруист?<br /><input type='radio' name='q24' value='1'",
                                    profile.IsAltruist ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q24' value='0'",
                                    !profile.IsAltruist ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br />Приемлете ли агрессию по отношению к людям или животным?<br />",
                                    "<input type='radio' name='q25' value='1'",
                                    profile.AcceptAgression ? Constants.Checked : Constants.SE,
                                    ">Да<input type='radio' name='q25' value='0'",
                                    !profile.AcceptAgression ? Constants.Checked : Constants.SE,
                                    ">Нет<br /><br /></p><p>О себе:</p>",
                                    "<textarea id='x' maxlength='1000' wrap='soft' spellcheck='true'>",
                                    profile.AboutMe,
                                    "</textarea><br /><p>Перед сохранением анкета должна быть заполнена полностью.</p><br /><div id='a'><a onClick='p(",
                                    profile.AccountId,
                                    ");return false'>Сохранить</a></div></div>"
                                    );
        }
        internal string GetOwnProfilePrimaryUnfilledMarkup(in int accountId)
        {
            return string.Concat("<div class='l'><p>Загрузите фотографию шириной 500 и высотой 809 пикселей (иначе могут исказиться пропорции), на которой хорошо видно Ваше лицо, размером не более 500 КБ в формате JPEG.",
                                    "<input type='file' id='f' accept='image/jpeg'><br /><br />",
                                    "Умеете ли играть в шахматы?<br /><input type='radio' name='q1' value='1'",
                                    ">Да<input type='radio' name='q1' value='0'",
                                    ">Нет<br /><br />Хотите ли встретиться в первую неделю знакомства?<br /><input type='radio' name='q2' value='1'",
                                    ">Да<input type='radio' name='q2' value='0'",
                                    ">Нет<br /><br />Можете ли обеспечивать многодетную семью?<br /><input type='radio' name='q3' value='1'",
                                    ">Да<input type='radio' name='q3' value='0'",
                                    ">Нет<br /><br />Имеете ли вредные привычки?<br /><input type='radio' name='q4' value='1'",
                                    ">Да<input type='radio' name='q4' value='0'",
                                    ">Нет<br /><br />Есть ли у Вас дети?<br /><input type='radio' name='q5' value='1'",
                                    ">Да<input type='radio' name='q5' value='0'",
                                    ">Нет<br /><br />Вы совершеннолетний(-яя)?<br /><input type='radio' name='q6' value='1'",
                                    ">Да<input type='radio' name='q6' value='0'",
                                    ">Нет<br /><br />Состояли ли в отношениях?<br /><input type='radio' name='q7' value='1'",
                                    ">Да<input type='radio' name='q7' value='0'",
                                    ">Нет<br /><br />Любите ли читать?<br /><input type='radio' name='q8' value='1'",
                                    ">Да<input type='radio' name='q8' value='0'",
                                    ">Нет<br /><br />Имеете ли профессию?<br /><input type='radio' name='q9' value='1'",
                                    ">Да<input type='radio' name='q9' value='0'",
                                    ">Нет<br /><br />Имеете ли постоянное место жительства в России?<br />",
                                    "<input type='radio' name='q10' value='1'",
                                    ">Да<input type='radio' name='q10' value='0'",
                                    ">Нет<br /><br />Занимаетесь ли физкультурой?<br /><input type='radio' name='q11' value='1'",
                                    ">Да<input type='radio' name='q11' value='0'",
                                    ">Нет<br /><br />Много ли у Вас увлечений?<br /><input type='radio' name='q12' value='1'",
                                    ">Да<input type='radio' name='q12' value='0'",
                                    ">Нет<br /><br />Владеете ли иностранным языком?<br /><input type='radio' name='q13' value='1'",
                                    ">Да<input type='radio' name='q13' value='0'",
                                    ">Нет<br /><br />Любите ли водить автомобиль?<br /><input type='radio' name='q14' value='1'",
                                    ">Да<input type='radio' name='q14' value='0'",
                                    ">Нет<br /><br />Нравится ли путешествовать?<br /><input type='radio' name='q15' value='1'",
                                    ">Да<input type='radio' name='q15' value='0'",
                                    ">Нет<br /><br />Предпочитаете ли умственную деятельность?<br /><input type='radio' name='q16' value='1'",
                                    ">Да<input type='radio' name='q16' value='0'",
                                    ">Нет<br /><br />Умеете ли делать мелкий ремонт?<br /><input type='radio' name='q17' value='1'",
                                    ">Да<input type='radio' name='q17' value='0'",
                                    ">Нет<br /><br />Симпатичен ли Вам противоположный пол?<br /><input type='radio' name='q18' value='1'",
                                    ">Да<input type='radio' name='q18' value='0'",
                                    ">Нет<br /><br />Соблюдаете ли диету?<br /><input type='radio' name='q19' value='1'",
                                    ">Да<input type='radio' name='q19' value='0'",
                                    ">Нет<br /><br />Держите ли дома животных?<br /><input type='radio' name='q20' value='1'",
                                    ">Да<input type='radio' name='q20' value='0'",
                                    ">Нет<br /><br />Ухаживаете ли за растениями?<br /><input type='radio' name='q21' value='1'",
                                    ">Да<input type='radio' name='q21' value='0'",
                                    ">Нет<br /><br />Готовы ли ждать, пока вспыхнет любовь?<br /><input type='radio' name='q22' value='1'",
                                    ">Да<input type='radio' name='q22' value='0'",
                                    ">Нет<br /><br />Вы готовы сохранять семью ради детей?<br /><input type='radio' name='q23' value='1'",
                                    ">Да<input type='radio' name='q23' value='0'",
                                    ">Нет<br /><br />Вы альтруист?<br /><input type='radio' name='q24' value='1'",
                                    ">Да<input type='radio' name='q24' value='0'",
                                    ">Нет<br /><br />Приемлете ли агрессию по отношению к людям или животным?<br />",
                                    "<input type='radio' name='q25' value='1'",
                                    ">Да<input type='radio' name='q25' value='0'",
                                    ">Нет<br /><br /></p><p>О себе:</p>",
                                    "<textarea id='x' maxlength='1000' wrap='soft' spellcheck='true'>",
                                    "</textarea><br /><p>Перед сохранением анкета должна быть заполнена полностью.</p><br /><div id='a'><a onClick='p(",
                                    accountId,
                                    ");return false'>Сохранить</a></div></div>"
                                    );
        }
        internal string GetPublicProfileMarkupFromFilledEntity(in Profile profile)
        {
            return string.Concat("<div class='l'><p>Последний раз сохранено ",
                                    profile.PublicationDate.ToString("dd.MM.yyyy"),
                                    ".</p><br /><br /><img src='data:image/jpeg;base64,",
                                    profile.PhotoBase64Gif,
                                    "' alt='Фотография' class='x'/><br /><br /><p>Умеете ли играть в шахматы?",
                                    profile.CanPlayChess ? Constants.Yes : Constants.No,
                                    "<br /><br />Хотите ли встретиться в первую неделю знакомства?",
                                    profile.WantToMeetInFirstWeek ? Constants.Yes : Constants.No,
                                    "<br /><br />Можете ли обеспечивать многодетную семью?",
                                    profile.CanSupportALargeFamily ? Constants.Yes : Constants.No,
                                    "<br /><br />Имеете ли вредные привычки?",
                                    profile.HaveBadHabits ? Constants.Yes : Constants.No,
                                    "<br /><br />Есть ли у Вас дети?",
                                    profile.HaveChildren ? Constants.Yes : Constants.No,
                                    "<br /><br />Вы совершеннолетний(-яя)?",
                                    profile.IsAdult ? Constants.Yes : Constants.No,
                                    "<br /><br />Состояли ли в отношениях?",
                                    profile.HadRelationship ? Constants.Yes : Constants.No,
                                    "<br /><br />Любите ли читать?",
                                    profile.LikeReading ? Constants.Yes : Constants.No,
                                    "<br /><br />Имеете ли профессию?",
                                    profile.HaveProfession ? Constants.Yes : Constants.No,
                                    "<br /><br />Имеете ли постоянное место жительства в России?",
                                    profile.HavePermanentResidenceInRussia ? Constants.Yes : Constants.No,
                                    "<br /><br />Занимаетесь ли физкультурой?",
                                    profile.DoPhysicalEducation ? Constants.Yes : Constants.No,
                                    "<br /><br />Много ли у Вас увлечений?",
                                    profile.HaveManyHobbies ? Constants.Yes : Constants.No,
                                    "<br /><br />Владеете ли иностранным языком?",
                                    profile.SpeakAForeignLanguage ? Constants.Yes : Constants.No,
                                    "<br /><br />Любите ли водить автомобиль?",
                                    profile.LikeDriving ? Constants.Yes : Constants.No,
                                    "<br /><br />Нравится ли путешествовать?",
                                    profile.LikeTravelling ? Constants.Yes : Constants.No,
                                    "<br /><br />Предпочитаете ли умственную деятельность?",
                                    profile.PreferMindActivity ? Constants.Yes : Constants.No,
                                    "<br /><br />Умеете ли делать мелкий ремонт?",
                                    profile.CanMakeMinorRepairs ? Constants.Yes : Constants.No,
                                    "<br /><br />Симпатичен ли Вам противоположный пол?",
                                    profile.IsOppositeGenderCute ? Constants.Yes : Constants.No,
                                    "<br /><br />Соблюдаете ли диету?",
                                    profile.FollowADiet ? Constants.Yes : Constants.No,
                                    "<br /><br />Держите ли дома животных?",
                                    profile.HavePets ? Constants.Yes : Constants.No,
                                    "<br /><br />Ухаживаете ли за растениями?",
                                    profile.TakeCareOfPlants ? Constants.Yes : Constants.No,
                                    "<br /><br />Готовы ли ждать, пока вспыхнет любовь?",
                                    profile.ReadyToWaitForALove ? Constants.Yes : Constants.No,
                                    "<br /><br />Вы готовы сохранять семью ради детей?",
                                    profile.ReadyToSaveFamilyForKids ? Constants.Yes : Constants.No,
                                    "<br /><br />Вы альтруист?",
                                    profile.IsAltruist ? Constants.Yes : Constants.No,
                                    "<br /><br />Приемлете ли агрессию по отношению к людям или животным?",
                                    profile.AcceptAgression ? Constants.Yes : Constants.No,
                                    "<br /><br /></p><p>О себе:</p><p>",
                                    profile.AboutMe,
                                    "</p></div>"
                                    );
        }
    }
}