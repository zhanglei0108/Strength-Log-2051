using System;
using System.Collections.Generic;
using System.Text;

namespace Strength_Log;
//Store data locally//
public static class UserProfile
{
    public static double Height { get; set; }
    public static double Weight { get; set; }
    public static string Gender { get; set; } = string.Empty;
    public static double BMI { get; set; }
}