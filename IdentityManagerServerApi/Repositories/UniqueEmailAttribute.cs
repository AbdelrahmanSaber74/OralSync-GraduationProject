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
        // Implement your logic to check if the email is unique in the database.
        // Return true if unique, false otherwise.
        // You might want to query your database or use some other mechanism to check uniqueness.
        // Replace this with your actual logic.

        // Example: Assuming a static list for simplicity.
        var existingEmails = new List<string> { "existing@email.com" };

        return !existingEmails.Contains(email);
    }
}
