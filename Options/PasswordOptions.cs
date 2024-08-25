namespace agency_portal_api.Options
{
    public class PasswordOptions
    {
        public int RequiredLength { get; set; } = 6;
        public bool RequireNonAlphanumeric { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;

    }

    public class PasswordValidator
    {
        private readonly PasswordOptions _passwordOptions;

        public PasswordValidator(PasswordOptions passwordOptions)
        {
            _passwordOptions = passwordOptions;
        }

        public bool ValidatePassword(string password)
        {
            if (password.Length < _passwordOptions.RequiredLength)
            {
                return false;
            }

            if (_passwordOptions.RequireDigit && !password.Any(char.IsDigit))
            {
                return false;
            }

            if (_passwordOptions.RequireLowercase && !password.Any(char.IsLower))
            {
                return false;
            }

            if (_passwordOptions.RequireUppercase && !password.Any(char.IsUpper))
            {
                return false;
            }

            if (_passwordOptions.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
            {
                return false;
            }

            return true;
        }
    }
}
