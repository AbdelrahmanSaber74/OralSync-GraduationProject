using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SharedClassLibrary.DTOs.ServiceResponses;
namespace IdentityManagerServerApi.Repositories
{
   public class AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccount
    {
        public Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        //public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        //{


        //    if (userDTO is null) return new GeneralResponse(false, "Model is empty");
        //    var newUser = new ApplicationUser()
        //    {
        //        Name = userDTO.Name,
        //        Email = userDTO.Email,
        //        PasswordHash = userDTO.Password,
        //        NormalizedUserName = userDTO.Email,
        //        PhoneNumber = userDTO.PhoneNumber,
        //        TimeAddUser = DateTime.Now,

        //    };
        //    var user = await userManager.FindByEmailAsync(newUser.Email);
        //    if (user is not null) return new GeneralResponse(false, "User registered already");

        //    var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
        //    if (!createUser.Succeeded) return new GeneralResponse(false, "Error occured.. please try again");




        //    Assign Default Role: Admin to first registrar; rest is user
        //    var checkAdmin = await roleManager.FindByNameAsync("Admin");
        //    if (checkAdmin is null)
        //    {
        //        await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
        //        await userManager.AddToRoleAsync(newUser, "Admin");
        //        return new GeneralResponse(true, "Account Created");
        //    }


        //    else if (userDTO.IsDoctor)
        //    {
        //        var checkUser = await roleManager.FindByNameAsync("Doctor");
        //        if (checkUser is null)
        //            await roleManager.CreateAsync(new IdentityRole() { Name = "Doctor" });


        //        await userManager.AddToRoleAsync(newUser, "Doctor");
        //        return new GeneralResponse(true, "Account Created");
        //    }
        //    else if (userDTO.IsStudent)
        //    {
        //        var checkUser = await roleManager.FindByNameAsync("Student");
        //        if (checkUser is null)
        //            await roleManager.CreateAsync(new IdentityRole() { Name = "Student" });

        //        await userManager.AddToRoleAsync(newUser, "Student");
        //        return new GeneralResponse(true, "Account Created");
        //    }

        //    else if (userDTO.IsPatient)
        //    {
        //        var checkUser = await roleManager.FindByNameAsync("Patient");
        //        if (checkUser is null)
        //            await roleManager.CreateAsync(new IdentityRole() { Name = "Patient" });

        //        await userManager.AddToRoleAsync(newUser, "Patient");
        //        return new GeneralResponse(true, "Account Created");
        //    }


        //    else
        //    {

        //        return new GeneralResponse(false, "Account Not Created Please Select User");

        //    }




        //}


        public async Task<GeneralResponse> CreateAccountSpecial(SpecialDTO specialDTO)
             {
                if (specialDTO is null)
                return new GeneralResponse(false, "Model is empty");



            string defaultImage = specialDTO.IsMale ? "male.png" : "female.png";



            var newUser = new ApplicationUser()
            {
                Name = specialDTO.FirstName + " " + specialDTO.LastName,
                Email = specialDTO.Email,
                PasswordHash = specialDTO.Password,
                PhoneNumber = specialDTO.PhoneNumber,
                UserName = specialDTO.Email,
                IsActive = true ,
                ProfileImage = $"/Profile/default/{defaultImage}" ,
                TimeAddUser = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
            };



            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return new GeneralResponse(false, "User registered already");

            //var createUser = await userManager.CreateAsync(newUser!, specialDTO.Password);
            //if (!createUser.Succeeded) return new GeneralResponse(false, "Error occured.. please try again");

            var createUser = await userManager.CreateAsync(newUser!, specialDTO.Password);

            if (!createUser.Succeeded)
            {
                // Build an error message with details of each error
                var errors = string.Join(", ", createUser.Errors.Select(e => e.Description));

                // Log or handle the errors as needed
                // For simplicity, let's just return the detailed error message
                return new GeneralResponse(false, $"Error occurred: {errors}");
            }



            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created");
            }


            else if (specialDTO.IsDoctor)
            {
                var checkUser = await roleManager.FindByNameAsync("Doctor");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Doctor" });


                await userManager.AddToRoleAsync(newUser, "Doctor");
                return new GeneralResponse(true, "Account Created");
            }
            else if (specialDTO.IsStudent)
            {
                var checkUser = await roleManager.FindByNameAsync("Student");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Student" });

                await userManager.AddToRoleAsync(newUser, "Student");
                return new GeneralResponse(true, "Account Created");
            }

            else if (specialDTO.IsPatient)
            {
                var checkUser = await roleManager.FindByNameAsync("Patient");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Patient" });

                await userManager.AddToRoleAsync(newUser, "Patient");
                return new GeneralResponse(true, "Account Created");
            }


            else
            {

                return new GeneralResponse(false, "Account Not Created Please Select User");

            }


        }









        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(false, null!, "Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!,  "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(50),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


      

    }
}
