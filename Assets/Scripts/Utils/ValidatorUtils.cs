using System;

namespace Assets.Scripts.Utils
{
    public static class ValidatorUtils
    {
        public static string ValidateNullAtGameObject(string variable, string gameObjectName)
        {
            if (string.IsNullOrEmpty(variable)) throw new Exception("the value of variable cannot be null at ValidatorUtils.ValidateNullAtGameObject ");
            if (string.IsNullOrEmpty(gameObjectName)) throw new Exception("the value of variable cannot be null at ValidatorUtils.ValidateNullAtGameObject ");

            return $"The value of {variable} cannot be null at GameObject: {gameObjectName}!";
        }
    }
}
