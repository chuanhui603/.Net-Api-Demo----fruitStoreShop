using System.ComponentModel.DataAnnotations;
namespace 水水水果API.Models.ConfigurationModel
{
    public class AppsetttingModel : IValidatableObject
    {
        public const string AppSetting = "AppSetting";
        public string testValue { get; set; }
  
 

        /// <summary>
        /// 驗證Appsetting中的AppSetting的邏輯 可以寫複雜判斷
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            yield return ValidationResult.Success;
        }
    }

}