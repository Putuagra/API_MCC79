﻿namespace API.Utilities.Handlers;

public class GenerateNik
{
    public static string Nik(string? lastNik = null)
    {
        if (lastNik is null)
        {
            return "111111"; // First employee
        }

        var generateNik = Convert.ToInt32(lastNik) + 1;

        return generateNik.ToString();
    }
}
