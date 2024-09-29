using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var email = value as string;

        // Check if email is unique (replace this with your logic).
        if (IsEmailUnique(email))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("User registered alread");
    }

    private bool IsEmailUnique(string email)
    {
      
        // Example: Assuming a static list for simplicity.
        var existingEmails = new List<string> { "existing@email.com" };

        return !existingEmails.Contains(email);
    }
}
