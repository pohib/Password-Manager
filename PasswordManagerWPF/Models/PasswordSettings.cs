using System.Runtime.Serialization;

namespace PasswordManagerWPF.Models
{
    [DataContract]
    public class PasswordSettings
    {
        [DataMember]
        public int Length { get; set; } = 12;

        [DataMember]
        public bool UseUpper { get; set; } = true;

        [DataMember]
        public bool UseLower { get; set; } = true;

        [DataMember]
        public bool UseNumbers { get; set; } = true;

        [DataMember]
        public bool UseSymbols { get; set; } = true;

        [DataMember]
        public bool ExcludeSimilar { get; set; } = true;

        [DataMember]
        public bool ExcludeAmbiguous { get; set; } = false;

        public PasswordSettings Clone()
        {
            return new PasswordSettings
            {
                Length = this.Length,
                UseUpper = this.UseUpper,
                UseLower = this.UseLower,
                UseNumbers = this.UseNumbers,
                UseSymbols = this.UseSymbols,
                ExcludeSimilar = this.ExcludeSimilar,
                ExcludeAmbiguous = this.ExcludeAmbiguous
            };
        }
    }
}
