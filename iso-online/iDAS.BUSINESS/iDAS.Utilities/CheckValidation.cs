using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public class CheckValidation
    {

        public static bool HasSpecialCharacters(string str, string special="")
        {
             string specialCharacters=@"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
             if (!special.Trim().Equals(""))
                 specialCharacters = @special;
            char[] specialCharactersArray = specialCharacters.ToCharArray();

            int index = str.IndexOfAny(specialCharactersArray);
            //index == -1 no special characters
            if (index == -1)
                return false;
            else
                return true;
        }
    }
}
