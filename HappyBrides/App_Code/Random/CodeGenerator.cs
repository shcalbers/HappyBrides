using System;

namespace HappyBrides.Random
{
    public static class CodeGenerator
    {

	private const int MAX_CODE_LENGTH = 16;
	private const string VALID_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	private static System.Random random = new System.Random();

	public static string GenerateCode()
	{
	    string code = "";

	    for (int i = 0; i < MAX_CODE_LENGTH; i++)
	    {
		code += GetRandomCharacter();
	    }

	    return code;
	}

	public static char GetRandomCharacter()
	{
	    return VALID_CHARACTERS[random.Next(0, VALID_CHARACTERS.Length)];
	}

    }
}
