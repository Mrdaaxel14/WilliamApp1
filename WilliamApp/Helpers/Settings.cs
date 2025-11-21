using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamApp.Helpers
{
    public static class Settings
    {
        public static string Token
        {
            get => Preferences.Get("token", "");
            set => Preferences.Set("token", value);
        }
    }
}
